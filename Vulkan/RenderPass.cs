using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class RenderPass : IDisposable
{
	private readonly RenderPassHandle renderPass;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal RenderPassHandle Handle => renderPass;

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.RenderPass,
				objectHandle: (ulong)(nint)renderPass,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroyRenderPass(device.Handle, renderPass, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyRenderPass(DeviceHandle device, RenderPassHandle renderPass, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => renderPass.ToString();

	internal RenderPass(RenderPassHandle renderPass, Device device, AllocationCallbacks? allocator) =>
		(this.renderPass, this.device, this.allocator) = (renderPass, device, allocator)
	;
}
