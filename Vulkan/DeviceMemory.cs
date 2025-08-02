using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class DeviceMemory : IDisposable
{
	private readonly Device device;
	private readonly nint deviceMemory;
	private readonly Handle<AllocationCallbacks> allocator;

	public nint Map(DeviceSize size, DeviceSize offset, MemoryMapFlags flags) 
	{
		Result result = vkMapMemory((nint)device, (nint)deviceMemory, offset, size, flags, out nint pointer);
		if (result != Result.Success) throw new VulkanException(result);

		return pointer;

		[DllImport(VK_LIB)] static extern Result vkMapMemory(nint device, nint deviceMemory, DeviceSize offset, DeviceSize size, MemoryMapFlags flags, out nint pDest);
	}

	public void Unmap() 
	{
		vkUnmapMemory((nint)device, (nint)deviceMemory);

		[DllImport(VK_LIB)] static extern Result vkUnmapMemory(nint device, nint deviceMemory);
	}

	public static explicit operator nint (DeviceMemory x) => x.deviceMemory;

	public void Dispose() 
	{
		vkFreeMemory((nint)device, deviceMemory, allocator);

		[DllImport(VK_LIB)] static extern void vkFreeMemory(nint device, nint deviceMemory, nint allocator);
	}

	private DeviceMemory(Device device, nint deviceMemory) => (this.device, this.deviceMemory) = (device, deviceMemory);
	internal DeviceMemory(Device device, nint deviceMemory, Handle<AllocationCallbacks> allocator) : this(device, deviceMemory) => this.allocator = allocator;
}
