using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct FenceCreateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly FenceCreateFlags Flags;

	public Fence CreateFence(Device device, AllocationCallbacksHandle allocator) 
	{
		Result result = vkCreateFence(device.Handle, in this, allocator, out FenceHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetFence(device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateFence(DeviceHandle device, in FenceCreateInfo createInfo, AllocationCallbacksHandle allocator, out FenceHandle fence);
	}

	public FenceCreateInfo(StructureType type, nint next, FenceCreateFlags flags) => (this.Type, this.Next, this.Flags) = (type, next, flags);
}
