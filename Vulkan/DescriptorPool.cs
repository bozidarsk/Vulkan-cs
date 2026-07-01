using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DescriptorPool : IDisposable
{
	private readonly DescriptorPoolHandle descriptorPool;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal DescriptorPoolHandle Handle => descriptorPool;

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.DescriptorPool,
				objectHandle: (ulong)(nint)descriptorPool,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroyDescriptorPool(device.Handle, descriptorPool, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyDescriptorPool(DeviceHandle device, DescriptorPoolHandle descriptorPool, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => descriptorPool.ToString();

	internal DescriptorPool(DescriptorPoolHandle descriptorPool, Device device, AllocationCallbacks? allocator) =>
		(this.descriptorPool, this.device, this.allocator) = (descriptorPool, device, allocator)
	;
}
