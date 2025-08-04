using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Vulkan;

public partial class Program 
{
	public void CreateBuffer(DeviceSize size, BufferUsage usage, MemoryProperty properties, out Buffer buffer, out DeviceMemory memory) 
	{
		using var bufferCreateInfo = new BufferCreateInfo(
			type: StructureType.BufferCreateInfo,
			next: default,
			flags: default,
			size: size,
			usage: usage,
			sharingMode: SharingMode.Exclusive,
			queueFamilyIndices: default
		);

		buffer = bufferCreateInfo.CreateBuffer(device, allocator);

		var allocateInfo = new MemoryAllocateInfo(
			type: StructureType.MemoryAllocateInfo,
			next: default,
			allocationSize: buffer.MemoryRequirements.Size,
			memoryTypeIndex: FindMemoryType(buffer.MemoryRequirements.MemoryType, properties)
		);

		memory = allocateInfo.CreateDeviceMemory(device, allocator);

		buffer.Bind(memory);
	}

	public void CopyBuffer(Buffer source, Buffer destination, DeviceSize size) 
	{
		var allocateInfo = new CommandBufferAllocateInfo(
			type: StructureType.CommandBufferAllocateInfo,
			next: default,
			commandPool: commandPool,
			level: CommandBufferLevel.Primary,
			commandBufferCount: 1
		);

		using var beginInfo = new CommandBufferBeginInfo(
			type: StructureType.CommandBufferBeginInfo,
			next: default,
			usage: CommandBufferUsage.OneTimeSubmit,
			inheritanceInfo: null
		);

		CommandBuffer[] commandBuffers = allocateInfo.CreateCommandBuffers(device);
		var cmd = commandBuffers.Single();

		cmd.Begin(beginInfo);
		cmd.CopyBuffer(source, destination, size);
		cmd.End();

		using var submitInfo = new SubmitInfo(
			type: StructureType.SubmitInfo,
			next: default,
			waitSemaphores: null,
			waitDstStageMasks: null,
			commandBuffers: commandBuffers,
			signalSemaphores: null
		);

		graphicsQueue.Submit(null, submitInfo);
		graphicsQueue.WaitIdle();

		commandPool.FreeCommandBuffers(commandBuffers);
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

	protected DeviceSize CreateUniformsBuffer(IReadOnlyDictionary<string, object> data, out Buffer buffer, out DeviceMemory memory) 
	{
		DeviceSize size = default;

		foreach ((var key, var value) in data)
			size += (ulong)Marshal.SizeOf(value.GetType());

		CreateBuffer(size, BufferUsage.UniformBuffer, MemoryProperty.HostVisible | MemoryProperty.HostCoherent, out buffer, out memory);
		nint mapped = memory.Map(size, offset: default, flags: default);

		foreach ((var key, var value) in data) 
		{
			Marshal.StructureToPtr(value, mapped, false);
			mapped += (nint)Marshal.SizeOf(value.GetType());
		}

		memory.Unmap();
		return size;
	}
}
