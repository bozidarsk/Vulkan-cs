using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Device : IDisposable
{
	private readonly nint device;
	private readonly Handle<AllocationCallbacks> allocator;

	public Queue GetQueue(uint queueFamilyIndex, uint queueIndex) 
	{
		vkGetDeviceQueue(device, queueFamilyIndex, queueIndex, out Queue queue);
		return queue;

		[DllImport(VK_LIB)] static extern void vkGetDeviceQueue(nint device, uint queueFamilyIndex, uint queueIndex, out Queue queue);
	}

	public static explicit operator nint (Device x) => x.device;

	public void Dispose() 
	{
		vkDestroyDevice(device, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyDevice(nint device, nint allocator);
	}

	private Device(nint device) => this.device = device;
	internal Device(nint device, Handle<AllocationCallbacks> allocator) : this(device) => this.allocator = allocator;
}
