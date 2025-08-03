using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DeviceMemory : IDisposable
{
	private readonly DeviceMemoryHandle deviceMemory;
	private readonly Device device;
	private readonly Handle<AllocationCallbacks> allocator;

	internal DeviceMemoryHandle Handle => deviceMemory;

	public nint Map(DeviceSize size, DeviceSize offset, MemoryMapFlags flags) 
	{
		Result result = vkMapMemory(device.Handle, deviceMemory, offset, size, flags, out nint pointer);
		if (result != Result.Success) throw new VulkanException(result);

		return pointer;

		[DllImport(VK_LIB)] static extern Result vkMapMemory(DeviceHandle device, DeviceMemoryHandle deviceMemory, DeviceSize offset, DeviceSize size, MemoryMapFlags flags, out nint pDest);
	}

	public void Unmap() 
	{
		vkUnmapMemory(device.Handle, deviceMemory);

		[DllImport(VK_LIB)] static extern Result vkUnmapMemory(DeviceHandle device, DeviceMemoryHandle deviceMemory);
	}

	public void Dispose() 
	{
		vkFreeMemory(device.Handle, deviceMemory, allocator);

		[DllImport(VK_LIB)] static extern void vkFreeMemory(DeviceHandle device, DeviceMemoryHandle deviceMemory, nint allocator);
	}

	internal DeviceMemory(DeviceMemoryHandle deviceMemory, Device device, Handle<AllocationCallbacks> allocator) => 
		(this.deviceMemory, this.device, this.allocator) = (deviceMemory, device, allocator)
	;
}
