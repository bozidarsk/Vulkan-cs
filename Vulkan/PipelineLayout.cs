using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class PipelineLayout : IDisposable
{
	private readonly PipelineLayoutHandle pipelineLayout;
	private readonly Device device;
	private readonly AllocationCallbacksHandle allocator;

	internal PipelineLayoutHandle Handle => pipelineLayout;

	public void Dispose() 
	{
		vkDestroyPipelineLayout(device.Handle, pipelineLayout, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyPipelineLayout(DeviceHandle device, PipelineLayoutHandle pipelineLayout, AllocationCallbacksHandle allocator);
	}

	internal PipelineLayout(PipelineLayoutHandle pipelineLayout, Device device, AllocationCallbacksHandle allocator) => 
		(this.pipelineLayout, this.device, this.allocator) = (pipelineLayout, device, allocator)
	;
}
