using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Framebuffer : IDisposable
{
	private readonly FramebufferHandle framebuffer;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal FramebufferHandle Handle => framebuffer;

	public void Dispose()
	{
		vkDestroyFramebuffer(device.Handle, framebuffer, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyFramebuffer(DeviceHandle device, FramebufferHandle framebuffer, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => framebuffer.ToString();

	internal Framebuffer(FramebufferHandle framebuffer, Device device, AllocationCallbacks? allocator) =>
		(this.framebuffer, this.device, this.allocator) = (framebuffer, device, allocator)
	;
}
