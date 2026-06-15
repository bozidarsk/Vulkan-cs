using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Buffer : IDisposable
{
	private readonly BufferHandle buffer;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

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

	public void Dispose()
	{
		vkDestroyBuffer(device.Handle, buffer, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyBuffer(DeviceHandle device, BufferHandle buffer, AllocationCallbacksHandle allocator);
	}

	internal Buffer(BufferHandle buffer, Device device, AllocationCallbacks? allocator) =>
		(this.buffer, this.device, this.allocator) = (buffer, device, allocator)
	;
}
