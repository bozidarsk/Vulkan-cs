using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct MemoryAllocateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly DeviceSize AllocationSize;
	public readonly uint MemoryTypeIndex;

	public DeviceMemory CreateDeviceMemory(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkAllocateMemory((nint)device, in this, allocator, out nint deviceMemoryHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, deviceMemoryHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkAllocateMemory(nint device, in MemoryAllocateInfo createInfo, nint allocator, out nint deviceMemory);
	}

	public MemoryAllocateInfo(
		StructureType type,
		nint next,
		DeviceSize allocationSize,
		uint memoryTypeIndex
	)
	{
		this.Type = type;
		this.Next = next;
		this.AllocationSize = allocationSize;
		this.MemoryTypeIndex = memoryTypeIndex;
	}
}
