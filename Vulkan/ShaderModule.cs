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

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.ShaderModule,
				objectHandle: (ulong)(nint)shaderModule,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroyShaderModule(device.Handle, shaderModule, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyShaderModule(DeviceHandle device, ShaderModuleHandle shaderModule, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => shaderModule.ToString();

	internal ShaderModule(ShaderModuleHandle shaderModule, Device device, AllocationCallbacks? allocator) =>
		(this.shaderModule, this.device, this.allocator) = (shaderModule, device, allocator)
	;
}
