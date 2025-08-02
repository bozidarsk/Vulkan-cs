using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DescriptorPool : IDisposable
{
	private readonly Device device;
	private readonly nint descriptorPool;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (DescriptorPool x) => x.descriptorPool;

	public void Dispose() 
	{
		vkDestroyDescriptorPool((nint)device, descriptorPool, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyDescriptorPool(nint device, nint descriptorPool, nint allocator);
	}

	private DescriptorPool(Device device, nint descriptorPool) => (this.device, this.descriptorPool) = (device, descriptorPool);
	internal DescriptorPool(Device device, nint descriptorPool, Handle<AllocationCallbacks> allocator) : this(device, descriptorPool) => this.allocator = allocator;
}

