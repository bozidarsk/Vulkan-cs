using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Semaphore : IDisposable
{
	private readonly SemaphoreHandle semaphore;
	private readonly Device device;
	private readonly AllocationCallbacksHandle allocator;

	internal SemaphoreHandle Handle => semaphore;

	public void Dispose() 
	{
		vkDestroySemaphore(device.Handle, semaphore, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroySemaphore(DeviceHandle device, SemaphoreHandle semaphore, AllocationCallbacksHandle allocator);
	}

	internal Semaphore(SemaphoreHandle semaphore, Device device, AllocationCallbacksHandle allocator) => 
		(this.semaphore, this.device, this.allocator) = (semaphore, device, allocator)
	;
}
