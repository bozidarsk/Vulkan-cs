using System;

using static Vulkan.Constants;
using static Vulkan.ExtensionDelegates;

namespace Vulkan;

public sealed class DebugUtilsMessenger : IDisposable
{
	private readonly Instance instance;
	private readonly nint debugUtilsMessenger;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (DebugUtilsMessenger x) => x.debugUtilsMessenger;

	public void Dispose() 
	{
		vkDestroyDebugUtilsMessengerEXT((nint)instance, debugUtilsMessenger, allocator);
	}

	private DebugUtilsMessenger(Instance instance, nint debugUtilsMessenger) => (this.instance, this.debugUtilsMessenger) = (instance, debugUtilsMessenger);
	internal DebugUtilsMessenger(Instance instance, nint debugUtilsMessenger, Handle<AllocationCallbacks> allocator) : this(instance, debugUtilsMessenger) => this.allocator = allocator;
}
