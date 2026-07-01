using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Framebuffer : IDisposable
{
	private readonly FramebufferHandle framebuffer;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal FramebufferHandle Handle => framebuffer;

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.Framebuffer,
				objectHandle: (ulong)(nint)framebuffer,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroyFramebuffer(device.Handle, framebuffer, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyFramebuffer(DeviceHandle device, FramebufferHandle framebuffer, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => framebuffer.ToString();

	internal Framebuffer(FramebufferHandle framebuffer, Device device, AllocationCallbacks? allocator) =>
		(this.framebuffer, this.device, this.allocator) = (framebuffer, device, allocator)
	;
}
