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
		Result result = vkAllocateMemory(device.Handle, in this, allocator, out DeviceMemoryHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetDeviceMemory(device, allocator);

		[DllImport(VK_LIB)] static extern Result vkAllocateMemory(DeviceHandle device, in MemoryAllocateInfo createInfo, nint allocator, out DeviceMemoryHandle deviceMemory);
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
