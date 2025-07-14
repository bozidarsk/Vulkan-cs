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

	public void Map(Array src, DeviceSize offset, DeviceSize size) => Map(ref MemoryMarshal.GetArrayDataReference((Array)src), offset, size);
	public unsafe void Map(ref byte src, DeviceSize offset, DeviceSize size) 
	{
		Result result;
		void* dest = default;

		result = vkMapMemory((nint)device, (nint)deviceMemory, offset, size, default, &dest);
		if (result != Result.Success) throw new VulkanException(result);

		System.Buffer.MemoryCopy(Unsafe.AsPointer<byte>(ref src), dest, size, size);

		result = vkUnmapMemory((nint)device, (nint)deviceMemory);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkMapMemory(nint device, nint deviceMemory, DeviceSize offset, DeviceSize size, MemoryMapFlags flags, void** ppDest);
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
