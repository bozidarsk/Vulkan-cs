using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class PipelineLayout : IDisposable
{
	private readonly PipelineLayoutHandle pipelineLayout;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal PipelineLayoutHandle Handle => pipelineLayout;

	public void Dispose()
	{
		vkDestroyPipelineLayout(device.Handle, pipelineLayout, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyPipelineLayout(DeviceHandle device, PipelineLayoutHandle pipelineLayout, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => pipelineLayout.ToString();

	internal PipelineLayout(PipelineLayoutHandle pipelineLayout, Device device, AllocationCallbacks? allocator) =>
		(this.pipelineLayout, this.device, this.allocator) = (pipelineLayout, device, allocator)
	;
}
