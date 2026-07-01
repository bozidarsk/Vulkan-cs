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

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.DescriptorSet,
				objectHandle: (ulong)(nint)descriptorSet,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkFreeDescriptorSets(device.Handle, descriptorPool.Handle, 1, in descriptorSet);

		[DllImport(VK_LIB)] static extern void vkFreeDescriptorSets(DeviceHandle device, DescriptorPoolHandle descriptorPool, uint count, in DescriptorSetHandle pDescriptorSets);
	}

	public override string ToString() => descriptorSet.ToString();

	internal DescriptorSet(DescriptorSetHandle descriptorSet, Device device, DescriptorPool descriptorPool) =>
		(this.descriptorSet, this.device, this.descriptorPool) = (descriptorSet, device, descriptorPool)
	;
}
