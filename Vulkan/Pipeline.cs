using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Pipeline : IDisposable
{
	private readonly PipelineHandle pipeline;
	private readonly Device device;
	private readonly Handle<AllocationCallbacks> allocator;

	internal PipelineHandle Handle => pipeline;

	public void Dispose() 
	{
		vkDestroyPipeline(device.Handle, pipeline, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyPipeline(DeviceHandle device, PipelineHandle pipeline, nint allocator);
	}

	internal Pipeline(PipelineHandle pipeline, Device device, Handle<AllocationCallbacks> allocator) => 
		(this.pipeline, this.device, this.allocator) = (pipeline, device, allocator)
	;
}
