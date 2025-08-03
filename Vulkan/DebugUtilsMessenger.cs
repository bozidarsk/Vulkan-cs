using System;

using static Vulkan.Constants;
using static Vulkan.ExtensionDelegates;

namespace Vulkan;

public sealed class DebugUtilsMessenger : IDisposable
{
	private readonly DebugUtilsMessengerHandle debugUtilsMessenger;
	private readonly Instance instance;
	private readonly Handle<AllocationCallbacks> allocator;

	internal DebugUtilsMessengerHandle Handle => debugUtilsMessenger;

	public void Dispose() 
	{
		vkDestroyDebugUtilsMessengerEXT(instance.Handle, debugUtilsMessenger, allocator);
	}

	internal DebugUtilsMessenger(DebugUtilsMessengerHandle debugUtilsMessenger, Instance instance, Handle<AllocationCallbacks> allocator) => 
		(this.debugUtilsMessenger, this.instance, this.allocator) = (debugUtilsMessenger, instance, allocator)
	;
}
