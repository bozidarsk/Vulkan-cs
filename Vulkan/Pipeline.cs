using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Pipeline : IDisposable
{
	private readonly PipelineHandle pipeline;
	private readonly Device device;
	private readonly AllocationCallbacksHandle allocator;

	internal PipelineHandle Handle => pipeline;

	public void Dispose() 
	{
		vkDestroyPipeline(device.Handle, pipeline, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyPipeline(DeviceHandle device, PipelineHandle pipeline, AllocationCallbacksHandle allocator);
	}

	internal Pipeline(PipelineHandle pipeline, Device device, AllocationCallbacksHandle allocator) => 
		(this.pipeline, this.device, this.allocator) = (pipeline, device, allocator)
	;
}
