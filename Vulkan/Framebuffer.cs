using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Framebuffer : IDisposable
{
	private readonly FramebufferHandle framebuffer;
	private readonly Device device;
	private readonly AllocationCallbacksHandle allocator;

	internal FramebufferHandle Handle => framebuffer;

	public void Dispose() 
	{
		vkDestroyFramebuffer(device.Handle, framebuffer, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyFramebuffer(DeviceHandle device, FramebufferHandle framebuffer, AllocationCallbacksHandle allocator);
	}

	internal Framebuffer(FramebufferHandle framebuffer, Device device, AllocationCallbacksHandle allocator) => 
		(this.framebuffer, this.device, this.allocator) = (framebuffer, device, allocator)
	;
}
