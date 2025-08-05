using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Vulkan;

public partial class Program 
{
	protected void CreateBuffer(DeviceSize size, BufferUsage usage, MemoryProperty properties, out Buffer buffer, out DeviceMemory memory) 
	{
		using var createInfo = new BufferCreateInfo(
			type: StructureType.BufferCreateInfo,
			next: default,
			flags: default,
			size: size,
			usage: usage,
			sharingMode: SharingMode.Exclusive,
			queueFamilyIndices: default
		);

		buffer = createInfo.CreateBuffer(device, allocator);
		var memoryRequirements = buffer.MemoryRequirements;

		var allocateInfo = new MemoryAllocateInfo(
			type: StructureType.MemoryAllocateInfo,
			next: default,
			allocationSize: memoryRequirements.Size,
			memoryTypeIndex: FindMemoryType(memoryRequirements.MemoryType, properties)
		);

		memory = allocateInfo.CreateDeviceMemory(device, allocator);
		memory.BindBuffer(buffer);
	}

	protected void CopyBuffer(Buffer source, Buffer destination, DeviceSize size) 
	{
		var cmd = BeginSingleTimeCommand();
		cmd.CopyBuffer(source, destination, size);
		EndSingleTimeCommand(cmd);
	}

	protected void CopyBufferToImage(Buffer buffer, Image image, int width, int height) 
	{
		var cmd = BeginSingleTimeCommand();

		var region = new BufferImageCopy(
			bufferOffset: 0,
			bufferRowLength: 0,
			bufferImageHeight: 0,
			imageSubresource: new(
				aspect: ImageAspect.Color,
				mipLevel: 0,
				baseArrayLayer: 0,
				layerCount: 1
			),
			imageOffset: new(x: 0, y: 0, z: 0),
			imageExtent: new(width: (uint)width, height: (uint)height, depth: 1)
		);

		cmd.CopyBufferToImage(buffer, image, ImageLayout.TransferDstOptimal, region);

		EndSingleTimeCommand(cmd);
	}

	public unsafe void CreateVertexBuffer(Array data, out Buffer buffer, out DeviceMemory memory) 
	{
		DeviceSize size = (ulong)Marshal.SizeOf(data.GetValue(0)!.GetType()) * (ulong)data.LongLength;

		CreateBuffer(size, BufferUsage.TransferSrc, MemoryProperty.HostVisible | MemoryProperty.DeviceLocal, out Buffer staggingBuffer, out DeviceMemory staggingMemory);

		nint staggingLocation = staggingMemory.Map(size: size, offset: default, flags: default);
		Unsafe.CopyBlockUnaligned(ref Unsafe.AsRef<byte>((void*)staggingLocation), ref MemoryMarshal.GetArrayDataReference(data), checked((uint)size));

		CreateBuffer(size, BufferUsage.TransferDst | BufferUsage.VertexBuffer, MemoryProperty.HostVisible | MemoryProperty.DeviceLocal, out buffer, out memory);

		CopyBuffer(staggingBuffer, buffer, size);

		staggingMemory.Unmap();
		staggingBuffer.Dispose();
		staggingMemory.Dispose();
	}

	public unsafe void CreateIndexBuffer(Array data, out Buffer buffer, out DeviceMemory memory) 
	{
		DeviceSize size = (ulong)Marshal.SizeOf(data.GetValue(0)!.GetType()) * (ulong)data.LongLength;

		CreateBuffer(size, BufferUsage.TransferSrc, MemoryProperty.HostVisible | MemoryProperty.DeviceLocal, out Buffer staggingBuffer, out DeviceMemory staggingMemory);

		nint staggingLocation = staggingMemory.Map(size: size, offset: default, flags: default);
		Unsafe.CopyBlockUnaligned(ref Unsafe.AsRef<byte>((void*)staggingLocation), ref MemoryMarshal.GetArrayDataReference(data), checked((uint)size));

		CreateBuffer(size, BufferUsage.TransferDst | BufferUsage.IndexBuffer, MemoryProperty.HostVisible | MemoryProperty.DeviceLocal, out buffer, out memory);

		CopyBuffer(staggingBuffer, buffer, size);

		staggingMemory.Unmap();
		staggingBuffer.Dispose();
		staggingMemory.Dispose();
	}

	public DeviceSize CreateUniformsBuffer(IReadOnlyDictionary<string, object> data, out Buffer? buffer, out DeviceMemory? memory) 
	{
		DeviceSize size = 0;

		foreach ((var key, var value) in data)
			if (value.GetType().IsValueType && !value.GetType().IsGenericType)
				size += (ulong)Marshal.SizeOf(value.GetType());

		if (size == 0) 
		{
			(buffer, memory) = (null, null);
			return 0;
		}

		CreateBuffer(size, BufferUsage.UniformBuffer, MemoryProperty.HostVisible | MemoryProperty.HostCoherent, out buffer, out memory);
		nint mapped = memory.Map(size, offset: default, flags: default);

		foreach ((var key, var value) in data) 
		{
			if (!value.GetType().IsValueType && !value.GetType().IsGenericType)
				continue;

			Marshal.StructureToPtr(value, mapped, false);
			mapped += (nint)Marshal.SizeOf(value.GetType());
		}

		memory.Unmap();
		return size;
	}

	public unsafe void CreateTexture(ref byte data, int width, int height, ImageType type, Format format, out Image image, out ImageView imageView, out DeviceMemory memory, out Sampler sampler) 
	{
		DeviceSize stride = format switch 
		{
			Format.R8G8B8A8UNorm => 4,
			Format.B8G8R8A8UNorm => 4,
			_ => throw new InvalidOperationException($"Failed to map texture format '{format}' to its stride.")
		};

		DeviceSize size = (ulong)width * (ulong)height * stride;

		CreateBuffer(size, BufferUsage.TransferSrc, MemoryProperty.HostVisible | MemoryProperty.DeviceLocal, out Buffer staggingBuffer, out DeviceMemory staggingMemory);

		nint staggingLocation = staggingMemory.Map(size: size, offset: default, flags: default);
		Unsafe.CopyBlockUnaligned(ref Unsafe.AsRef<byte>((void*)staggingLocation), ref data, checked((uint)size));

		CreateImage(width, height, type, format, out image, out memory);

		TransitionImageLayout(image, format, ImageLayout.Undefined, ImageLayout.TransferDstOptimal);
		CopyBufferToImage(staggingBuffer, image, width, height);
		TransitionImageLayout(image, format, ImageLayout.TransferDstOptimal, ImageLayout.ShaderReadOnlyOptimal);

		CreateImageView(image, format, out imageView);
		CreateSampler(out sampler);

		staggingMemory.Unmap();
		staggingBuffer.Dispose();
		staggingMemory.Dispose();
	}

	protected void CreateImage(int width, int height, ImageType type, Format format, out Image image, out DeviceMemory memory) 
	{
		using var createInfo = new ImageCreateInfo(
			type: StructureType.ImageCreateInfo,
			next: default,
			flags: default,
			imageType: type,
			format: format,
			extent: new(width: checked((uint)width), height: checked((uint)height), depth: 1),
			mipLevels: 1,
			arrayLayers: 1,
			samples: SampleCount.Bit1,
			tiling: ImageTiling.Optimal,
			usage: ImageUsage.TransferDst | ImageUsage.Sampled,
			sharingMode: SharingMode.Exclusive,
			queueFamilyIndices: null,
			initialLayout: ImageLayout.Undefined
		);

		image = createInfo.CreateImage(device, allocator);
		var memoryRequirements = image.MemoryRequirements;

		var allocateInfo = new MemoryAllocateInfo(
			type: StructureType.MemoryAllocateInfo,
			next: default,
			allocationSize: memoryRequirements.Size,
			memoryTypeIndex: FindMemoryType(memoryRequirements.MemoryType, MemoryProperty.DeviceLocal)
		);

		memory = allocateInfo.CreateDeviceMemory(device, allocator);
		memory.BindImage(image);
	}

	protected void CreateImageView(Image image, Format format, out ImageView imageView) 
	{
		var createInfo = new ImageViewCreateInfo(
			type: StructureType.ImageViewCreateInfo,
			next: default,
			flags: default,
			image: image,
			viewType: ImageViewType.Generic2D,
			format: format,
			components: new(r: ComponentSwizzle.Identity, g: ComponentSwizzle.Identity, b: ComponentSwizzle.Identity, a: ComponentSwizzle.Identity),
			subresourceRange: new(
				aspect: ImageAspect.Color,
				baseMipLevel: 0,
				levelCount: 1,
				baseArrayLayer: 0,
				layerCount: 1
			)
		);

		imageView = createInfo.CreateImageView(device, allocator);
	}

	protected void CreateSampler(out Sampler sampler) 
	{
		var createInfo = new SamplerCreateInfo(
			type: StructureType.SamplerCreateInfo,
			next: default,
			flags: default,
			magFilter: Filter.Linear,
			minFilter: Filter.Linear,
			mipmapMode: SamplerMipmapMode.Linear,
			addressModeU: SamplerAddressMode.Repeat,
			addressModeV: SamplerAddressMode.Repeat,
			addressModeW: SamplerAddressMode.Repeat,
			mipLodBias: 0f,
			anisotropyEnable: false,
			maxAnisotropy: 1f,
			compareEnable: false,
			compareOp: CompareOp.Always,
			minLod: 0f,
			maxLod: 0f,
			borderColor: BorderColor.FloatOpaqueBlack,
			unnormalizedCoordinates: false
		);

		sampler = createInfo.CreateSampler(device, allocator);
	}

	protected void TransitionImageLayout(Image image, Format format, ImageLayout from, ImageLayout to) 
	{
		Access sourceAccess, destinationAccess;
		PipelineStage sourceStage, destinationStage;

		if (from == ImageLayout.Undefined && to == ImageLayout.TransferDstOptimal) 
		{
			sourceAccess = 0;
			destinationAccess = Access.TransferWrite;

			sourceStage = PipelineStage.TopOfPipe;
			destinationStage = PipelineStage.Transfer;
		}
		else if (from == ImageLayout.TransferDstOptimal && to == ImageLayout.ShaderReadOnlyOptimal) 
		{
			sourceAccess = Access.TransferWrite;
			destinationAccess = Access.ShaderRead;

			sourceStage = PipelineStage.Transfer;
			destinationStage = PipelineStage.FragmentShader;
		}
		else
			throw new InvalidOperationException("Unsupported layer transition.");

		var cmd = BeginSingleTimeCommand();

		var barrier = new ImageMemoryBarrier(
			type: StructureType.ImageMemoryBarrier,
			next: default,
			srcAccess: sourceAccess,
			dstAccess: destinationAccess,
			oldLayout: from,
			newLayout: to,
			srcQueueFamilyIndex: unchecked((uint)-1),
			dstQueueFamilyIndex: unchecked((uint)-1),
			image: image,
			subresourceRange: new(
				aspect: ImageAspect.Color,
				baseMipLevel: 0,
				levelCount: 1,
				baseArrayLayer: 0,
				layerCount: 1
			)
		);

		cmd.PipelineBarrier(
			srcStage: sourceStage,
			dstStage: destinationStage,
			dependencyFlags: default,
			memoryBarriers: null,
			bufferMemoryBarriers: null,
			imageMemoryBarriers: [ barrier ]
		);

		EndSingleTimeCommand(cmd);
	}

	protected CommandBuffer BeginSingleTimeCommand() 
	{
		var allocateInfo = new CommandBufferAllocateInfo(
			type: StructureType.CommandBufferAllocateInfo,
			next: default,
			commandPool: commandPool,
			level: CommandBufferLevel.Primary,
			commandBufferCount: 1
		);

		var cmd = allocateInfo.CreateCommandBuffers(device, commandPool).Single();

		using var beginInfo = new CommandBufferBeginInfo(
			type: StructureType.CommandBufferBeginInfo,
			next: default,
			usage: CommandBufferUsage.OneTimeSubmit,
			inheritanceInfo: null
		);

		cmd.Begin(beginInfo);
		return cmd;
	}

	protected void EndSingleTimeCommand(CommandBuffer cmd) 
	{
		cmd.End();

		using var submitInfo = new SubmitInfo(
			type: StructureType.SubmitInfo,
			next: default,
			waitSemaphores: null,
			waitDstStageMasks: null,
			commandBuffers: [ cmd ],
			signalSemaphores: null
		);

		graphicsQueue.Submit(null, submitInfo);
		graphicsQueue.WaitIdle();

		cmd.Dispose();
	}
}
