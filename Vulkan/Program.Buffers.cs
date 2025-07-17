using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vulkan;

public partial class Program 
{
	protected void CreateBuffer(DeviceSize size, BufferUsage usage, MemoryProperty properties, out Buffer buffer, out DeviceMemory memory) 
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
			memoryTypeIndex: findMemoryType(buffer.MemoryRequirements.MemoryType, properties)
		);

		memory = allocateInfo.CreateDeviceMemory(device, allocator);

		buffer.Bind(memory);

		uint findMemoryType(uint typeFilter, MemoryProperty properties) 
		{
			var memProperties = physicalDevice.MemoryProperties;
			int i = 0;

			foreach (var x in memProperties.MemoryTypes) 
			{
				if ((typeFilter & (1 << i)) != 0 && x.Properties.HasFlag(properties))
					return (uint)i;

				i++;
			}

			throw new VulkanException("Failed to find suitable memory type.");
		}
	}

	protected void CopyBuffer(Buffer source, Buffer destination, DeviceSize size) 
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

	public void CreateVertexBuffer(Array data, out Buffer buffer, out DeviceMemory memory) 
	{
		DeviceSize size = (ulong)Marshal.SizeOf(data.GetValue(0)!.GetType()) * (ulong)data.LongLength;

		CreateBuffer(size, BufferUsage.TransferSrc, MemoryProperty.HostVisible | MemoryProperty.DeviceLocal, out Buffer staggingBuffer, out DeviceMemory staggingMemory);
		staggingMemory.Map(data, offset: 0, size: size);

		CreateBuffer(size, BufferUsage.TransferDst | BufferUsage.VertexBuffer, MemoryProperty.HostVisible | MemoryProperty.DeviceLocal, out buffer, out memory);

		CopyBuffer(staggingBuffer, buffer, size);

		staggingBuffer.Dispose();
		staggingMemory.Dispose();
	}

	public void CreateIndexBuffer(Array data, out Buffer buffer, out DeviceMemory memory) 
	{
		DeviceSize size = (ulong)Marshal.SizeOf(data.GetValue(0)!.GetType()) * (ulong)data.LongLength;

		CreateBuffer(size, BufferUsage.TransferSrc, MemoryProperty.HostVisible | MemoryProperty.DeviceLocal, out Buffer staggingBuffer, out DeviceMemory staggingMemory);
		staggingMemory.Map(data, offset: 0, size: size);

		CreateBuffer(size, BufferUsage.TransferDst | BufferUsage.IndexBuffer, MemoryProperty.HostVisible | MemoryProperty.DeviceLocal, out buffer, out memory);

		CopyBuffer(staggingBuffer, buffer, size);

		staggingBuffer.Dispose();
		staggingMemory.Dispose();
	}
}
