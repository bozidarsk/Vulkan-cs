using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DescriptorSetLayout : IDisposable
{
	private readonly Device device;
	private readonly nint descriptorSetLayout;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (DescriptorSetLayout x) => x.descriptorSetLayout;

	public void Dispose() 
	{
		vkDestroyDescriptorSetLayout((nint)device, descriptorSetLayout, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyDescriptorSetLayout(nint device, nint descriptorSetLayout, nint allocator);
	}

	private DescriptorSetLayout(Device device, nint descriptorSetLayout) => (this.device, this.descriptorSetLayout) = (device, descriptorSetLayout);
	internal DescriptorSetLayout(Device device, nint descriptorSetLayout, Handle<AllocationCallbacks> allocator) : this(device, descriptorSetLayout) => this.allocator = allocator;
}

