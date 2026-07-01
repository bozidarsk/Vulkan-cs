using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DescriptorSetLayout : IDisposable
{
	private readonly DescriptorSetLayoutHandle descriptorSetLayout;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal DescriptorSetLayoutHandle Handle => descriptorSetLayout;

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.DescriptorSetLayout,
				objectHandle: (ulong)(nint)descriptorSetLayout,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroyDescriptorSetLayout(device.Handle, descriptorSetLayout, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyDescriptorSetLayout(DeviceHandle device, DescriptorSetLayoutHandle descriptorSetLayout, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => descriptorSetLayout.ToString();

	internal DescriptorSetLayout(DescriptorSetLayoutHandle descriptorSetLayout, Device device, AllocationCallbacks? allocator) =>
		(this.descriptorSetLayout, this.device, this.allocator) = (descriptorSetLayout, device, allocator)
	;
}
