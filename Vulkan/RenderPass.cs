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

	public void Dispose()
	{
		vkDestroyRenderPass(device.Handle, renderPass, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyRenderPass(DeviceHandle device, RenderPassHandle renderPass, AllocationCallbacksHandle allocator);
	}

	internal RenderPass(RenderPassHandle renderPass, Device device, AllocationCallbacks? allocator) =>
		(this.renderPass, this.device, this.allocator) = (renderPass, device, allocator)
	;
}
