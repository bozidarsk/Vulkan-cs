using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DebugUtilsMessenger : IDisposable
{
	private readonly Instance instance;
	private readonly nint debugUtilsMessenger;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (DebugUtilsMessenger x) => x.debugUtilsMessenger;

	public void Dispose() 
	{
		var func = Marshal.GetDelegateForFunctionPointer<DestroyDebugUtilsMessengerDelegate>(vkGetInstanceProcAddr((nint)instance, "vkDestroyDebugUtilsMessengerEXT"));

		func((nint)instance, debugUtilsMessenger, allocator);

		[DllImport(VK_LIB)] static extern nint vkGetInstanceProcAddr(nint instance, string name);
	}

	private DebugUtilsMessenger(Instance instance, nint debugUtilsMessenger) => (this.instance, this.debugUtilsMessenger) = (instance, debugUtilsMessenger);
	internal DebugUtilsMessenger(Instance instance, nint debugUtilsMessenger, Handle<AllocationCallbacks> allocator) : this(instance, debugUtilsMessenger) => this.allocator = allocator;
}
