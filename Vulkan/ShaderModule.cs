using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class ShaderModule : IDisposable
{
	private readonly ShaderModuleHandle shaderModule;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal ShaderModuleHandle Handle => shaderModule;

	public void Dispose()
	{
		vkDestroyShaderModule(device.Handle, shaderModule, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyShaderModule(DeviceHandle device, ShaderModuleHandle shaderModule, AllocationCallbacksHandle allocator);
	}

	internal ShaderModule(ShaderModuleHandle shaderModule, Device device, AllocationCallbacks? allocator) =>
		(this.shaderModule, this.device, this.allocator) = (shaderModule, device, allocator)
	;
}
