using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DescriptorSet : IDisposable
{
	private readonly DescriptorSetHandle descriptorSet;
	private readonly Device device;
	private readonly DescriptorPool descriptorPool;

	internal DescriptorSetHandle Handle => descriptorSet;

	public void Dispose() 
	{
		vkFreeDescriptorSets(device.Handle, descriptorPool.Handle, 1, in descriptorSet);

		[DllImport(VK_LIB)] static extern void vkFreeDescriptorSets(DeviceHandle device, DescriptorPoolHandle descriptorPool, uint count, in DescriptorSetHandle pDescriptorSets);
	}

	internal DescriptorSet(DescriptorSetHandle descriptorSet, Device device, DescriptorPool descriptorPool) => 
		(this.descriptorSet, this.device, this.descriptorPool) = (descriptorSet, device, descriptorPool)
	;
}
