using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class PipelineLayout : IDisposable
{
	private readonly PipelineLayoutHandle pipelineLayout;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal PipelineLayoutHandle Handle => pipelineLayout;

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.PipelineLayout,
				objectHandle: (ulong)(nint)pipelineLayout,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroyPipelineLayout(device.Handle, pipelineLayout, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyPipelineLayout(DeviceHandle device, PipelineLayoutHandle pipelineLayout, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => pipelineLayout.ToString();

	internal PipelineLayout(PipelineLayoutHandle pipelineLayout, Device device, AllocationCallbacks? allocator) =>
		(this.pipelineLayout, this.device, this.allocator) = (pipelineLayout, device, allocator)
	;
}
