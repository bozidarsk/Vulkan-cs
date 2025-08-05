using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class CommandPool : IDisposable
{
	private readonly CommandPoolHandle commandPool;
	private readonly Device device;
	private readonly Handle<AllocationCallbacks> allocator;

	internal CommandPoolHandle Handle => commandPool;

	public void FreeCommandBuffers(params CommandBuffer[] buffers) 
	{
		if (buffers == null)
			throw new ArgumentNullException();

		vkFreeCommandBuffers(device.Handle, commandPool, (uint)buffers.Length, ref MemoryMarshal.GetArrayDataReference(buffers.Select(x => x.Handle).ToArray()));

		[DllImport(VK_LIB)] static extern void vkFreeCommandBuffers(DeviceHandle device, CommandPoolHandle commandPool, uint bufferCount, ref CommandBufferHandle pBuffers);
	}

	public void Dispose() 
	{
		vkDestroyCommandPool(device.Handle, commandPool, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyCommandPool(DeviceHandle device, CommandPoolHandle commandPool, nint allocator);
	}

	internal CommandPool(CommandPoolHandle commandPool, Device device, Handle<AllocationCallbacks> allocator) => 
		(this.commandPool, this.device, this.allocator) = (commandPool, device, allocator)
	;
}
