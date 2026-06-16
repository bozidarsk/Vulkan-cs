using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Pipeline : IDisposable
{
	private readonly PipelineHandle pipeline;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal PipelineHandle Handle => pipeline;

	public void Dispose()
	{
		vkDestroyPipeline(device.Handle, pipeline, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyPipeline(DeviceHandle device, PipelineHandle pipeline, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => pipeline.ToString();

	internal Pipeline(PipelineHandle pipeline, Device device, AllocationCallbacks? allocator) =>
		(this.pipeline, this.device, this.allocator) = (pipeline, device, allocator)
	;
}
