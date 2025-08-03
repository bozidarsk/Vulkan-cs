using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DescriptorSetLayout : IDisposable
{
	private readonly DescriptorSetLayoutHandle descriptorSetLayout;
	private readonly Device device;
	private readonly Handle<AllocationCallbacks> allocator;

	internal DescriptorSetLayoutHandle Handle => descriptorSetLayout;

	public void Dispose() 
	{
		vkDestroyDescriptorSetLayout(device.Handle, descriptorSetLayout, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyDescriptorSetLayout(DeviceHandle device, DescriptorSetLayoutHandle descriptorSetLayout, nint allocator);
	}

	internal DescriptorSetLayout(DescriptorSetLayoutHandle descriptorSetLayout, Device device, Handle<AllocationCallbacks> allocator) => 
		(this.descriptorSetLayout, this.device, this.allocator) = (descriptorSetLayout, device, allocator)
	;
}
