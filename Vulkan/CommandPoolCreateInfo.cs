using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct CommandPoolCreateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly CommandPoolCreateFlags Flags;
	public readonly uint QueueFamilyIndex;

	public CommandPool CreateCommandPool(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateCommandPool(device.Handle, in this, allocator, out CommandPoolHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetCommandPool(device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateCommandPool(DeviceHandle device, in CommandPoolCreateInfo createInfo, nint allocator, out CommandPoolHandle commandPool);
	}

	public CommandPoolCreateInfo(
		StructureType type,
		nint next,
		CommandPoolCreateFlags flags,
		uint queueFamilyIndex
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.QueueFamilyIndex = queueFamilyIndex;
	}
}
