using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Framebuffer : IDisposable
{
	private readonly Device device;
	private readonly nint framebuffer;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (Framebuffer x) => x.framebuffer;

	public void Dispose() 
	{
		vkDestroyFramebuffer((nint)device, framebuffer, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyFramebuffer(nint device, nint framebuffer, nint allocator);
	}

	private Framebuffer(Device device, nint framebuffer) => (this.device, this.framebuffer) = (device, framebuffer);
	internal Framebuffer(Device device, nint framebuffer, Handle<AllocationCallbacks> allocator) : this(device, framebuffer) => this.allocator = allocator;
}
