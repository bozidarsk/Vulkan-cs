using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DescriptorPool : IDisposable
{
	private readonly DescriptorPoolHandle descriptorPool;
	private readonly Device device;
	private readonly AllocationCallbacksHandle allocator;

	internal DescriptorPoolHandle Handle => descriptorPool;

	public void Dispose() 
	{
		vkDestroyDescriptorPool(device.Handle, descriptorPool, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyDescriptorPool(DeviceHandle device, DescriptorPoolHandle descriptorPool, AllocationCallbacksHandle allocator);
	}

	internal DescriptorPool(DescriptorPoolHandle descriptorPool, Device device, AllocationCallbacksHandle allocator) => 
		(this.descriptorPool, this.device, this.allocator) = (descriptorPool, device, allocator)
	;
}
