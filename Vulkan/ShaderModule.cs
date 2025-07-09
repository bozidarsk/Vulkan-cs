using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class ShaderModule : IDisposable
{
	private readonly Device device;
	private readonly nint shaderModule;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (ShaderModule x) => x.shaderModule;

	public void Dispose() 
	{
		vkDestroyShaderModule((nint)device, shaderModule, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyShaderModule(nint device, nint shaderModule, nint allocator);
	}

	private ShaderModule(Device device, nint shaderModule) => (this.device, this.shaderModule) = (device, shaderModule);
	internal ShaderModule(Device device, nint shaderModule, Handle<AllocationCallbacks> allocator) : this(device, shaderModule) => this.allocator = allocator;
}
