using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DescriptorSet : IDisposable
{
	private readonly Device device;
	private readonly nint descriptorSet;
	private readonly DescriptorPool descriptorPool;

	public static explicit operator nint (DescriptorSet x) => x.descriptorSet;

	public void Dispose() 
	{
		vkFreeDescriptorSets((nint)device, (nint)descriptorPool, 1, in descriptorSet);

		[DllImport(VK_LIB)] static extern void vkFreeDescriptorSets(nint device, nint descriptorPool, uint count, in nint pDescriptorSets);
	}

	internal DescriptorSet(Device device, nint descriptorSet, DescriptorPool descriptorPool) => 
		(this.device, this.descriptorSet, this.descriptorPool) = (device, descriptorSet, descriptorPool)
	;
}
