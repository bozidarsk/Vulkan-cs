using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class CommandPool : IDisposable
{
	private readonly Device device;
	private readonly nint commandPool;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (CommandPool x) => x.commandPool;

	public void Dispose() 
	{
		vkDestroyCommandPool((nint)device, commandPool, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyCommandPool(nint device, nint commandPool, nint allocator);
	}

	private CommandPool(Device device, nint commandPool) => (this.device, this.commandPool) = (device, commandPool);
	internal CommandPool(Device device, nint commandPool, Handle<AllocationCallbacks> allocator) : this(device, commandPool) => this.allocator = allocator;
}
