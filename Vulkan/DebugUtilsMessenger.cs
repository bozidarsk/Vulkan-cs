using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DebugUtilsMessenger : IDisposable
{
	private readonly DebugUtilsMessengerHandle debugUtilsMessenger;
	private readonly Instance instance;
	private readonly AllocationCallbacksHandle allocator;

	internal DebugUtilsMessengerHandle Handle => debugUtilsMessenger;

	public void Dispose()
	{
		var vkDestroyDebugUtilsMessengerEXT = Marshal.GetDelegateForFunctionPointer<DestroyDebugUtilsMessengerExtensionDelegate>(vkGetInstanceProcAddr(instance.Handle, "vkDestroyDebugUtilsMessengerEXT"));

		vkDestroyDebugUtilsMessengerEXT(instance.Handle, debugUtilsMessenger, allocator);

		[DllImport(VK_LIB)] static extern nint vkGetInstanceProcAddr(InstanceHandle instance, string name);
	}

	internal DebugUtilsMessenger(DebugUtilsMessengerHandle debugUtilsMessenger, Instance instance, AllocationCallbacksHandle allocator) =>
		(this.debugUtilsMessenger, this.instance, this.allocator) = (debugUtilsMessenger, instance, allocator)
	;
}
