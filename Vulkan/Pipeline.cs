using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Pipeline : IDisposable
{
	private readonly Device device;
	private readonly nint pipeline;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (Pipeline x) => x.pipeline;

	public void Dispose() 
	{
		vkDestroyPipeline((nint)device, pipeline, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyPipeline(nint device, nint pipeline, nint allocator);
	}

	private Pipeline(Device device, nint pipeline) => (this.device, this.pipeline) = (device, pipeline);
	internal Pipeline(Device device, nint pipeline, Handle<AllocationCallbacks> allocator) : this(device, pipeline) => this.allocator = allocator;
}
