using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class CommandPool : IDisposable
{
	private readonly CommandPoolHandle commandPool;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal CommandPoolHandle Handle => commandPool;

	public void FreeCommandBuffers(params CommandBuffer[] buffers)
	{
		if (buffers == null)
			throw new ArgumentNullException();

		vkFreeCommandBuffers(device.Handle, commandPool, (uint)buffers.Length, ref MemoryMarshal.GetArrayDataReference(buffers.Select(x => x.Handle).ToArray()));

		[DllImport(VK_LIB)] static extern void vkFreeCommandBuffers(DeviceHandle device, CommandPoolHandle commandPool, uint bufferCount, ref CommandBufferHandle pBuffers);
	}

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.CommandPool,
				objectHandle: (ulong)(nint)commandPool,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroyCommandPool(device.Handle, commandPool, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyCommandPool(DeviceHandle device, CommandPoolHandle commandPool, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => commandPool.ToString();

	internal CommandPool(CommandPoolHandle commandPool, Device device, AllocationCallbacks? allocator) =>
		(this.commandPool, this.device, this.allocator) = (commandPool, device, allocator)
	;
}
