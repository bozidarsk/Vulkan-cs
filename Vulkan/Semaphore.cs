using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Semaphore : IDisposable
{
	private readonly Device device;
	private readonly nint semaphore;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (Semaphore x) => x.semaphore;

	public void Dispose() 
	{
		vkDestroySemaphore((nint)device, semaphore, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroySemaphore(nint device, nint semaphore, nint allocator);
	}

	private Semaphore(Device device, nint semaphore) => (this.device, this.semaphore) = (device, semaphore);
	internal Semaphore(Device device, nint semaphore, Handle<AllocationCallbacks> allocator) : this(device, semaphore) => this.allocator = allocator;
}
