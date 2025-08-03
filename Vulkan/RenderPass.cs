using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class RenderPass : IDisposable
{
	private readonly RenderPassHandle renderPass;
	private readonly Device device;
	private readonly Handle<AllocationCallbacks> allocator;

	internal RenderPassHandle Handle => renderPass;

	public void Dispose() 
	{
		vkDestroyRenderPass(device.Handle, renderPass, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyRenderPass(DeviceHandle device, RenderPassHandle renderPass, nint allocator);
	}

	internal RenderPass(RenderPassHandle renderPass, Device device, Handle<AllocationCallbacks> allocator) => 
		(this.renderPass, this.device, this.allocator) = (renderPass, device, allocator)
	;
}
