using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class PipelineLayout : IDisposable
{
	private readonly Device device;
	private readonly nint pipelineLayout;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (PipelineLayout x) => x.pipelineLayout;

	public void Dispose() 
	{
		vkDestroyPipelineLayout((nint)device, pipelineLayout, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyPipelineLayout(nint device, nint pipelineLayout, nint allocator);
	}

	private PipelineLayout(Device device, nint pipelineLayout) => (this.device, this.pipelineLayout) = (device, pipelineLayout);
	internal PipelineLayout(Device device, nint pipelineLayout, Handle<AllocationCallbacks> allocator) : this(device, pipelineLayout) => this.allocator = allocator;
}
