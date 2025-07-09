using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class RenderPass : IDisposable
{
	private readonly Device device;
	private readonly nint renderPass;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (RenderPass x) => x.renderPass;

	public void Dispose() 
	{
		vkDestroyRenderPass((nint)device, renderPass, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyRenderPass(nint device, nint renderPass, nint allocator);
	}

	private RenderPass(Device device, nint renderPass) => (this.device, this.renderPass) = (device, renderPass);
	internal RenderPass(Device device, nint renderPass, Handle<AllocationCallbacks> allocator) : this(device, renderPass) => this.allocator = allocator;
}
