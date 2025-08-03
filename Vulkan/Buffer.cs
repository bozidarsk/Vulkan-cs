using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Buffer : IDisposable
{
	private readonly BufferHandle buffer;
	private readonly Device device;
	private readonly Handle<AllocationCallbacks> allocator;

	internal BufferHandle Handle => buffer;

	public MemoryRequirements MemoryRequirements 
	{
		get 
		{
			vkGetBufferMemoryRequirements(device.Handle, buffer, out MemoryRequirements requirements);
			return requirements;

			[DllImport(VK_LIB)] static extern void vkGetBufferMemoryRequirements(DeviceHandle device, BufferHandle buffer, out MemoryRequirements requirements);
		}
	}

	public void Bind(DeviceMemory memory, DeviceSize offset = default) 
	{
		Result result = vkBindBufferMemory(device.Handle, buffer, memory.Handle, offset);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkBindBufferMemory(DeviceHandle device, BufferHandle buffer, DeviceMemoryHandle memory, DeviceSize offset);
	}

	public void Dispose() 
	{
		vkDestroyBuffer(device.Handle, buffer, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyBuffer(DeviceHandle device, BufferHandle buffer, nint allocator);
	}

	internal Buffer(BufferHandle buffer, Device device, Handle<AllocationCallbacks> allocator) => 
		(this.buffer, this.device, this.allocator) = (buffer, device, allocator)
	;
}
