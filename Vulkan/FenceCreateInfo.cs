using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct FenceCreateInfo
{
	public readonly StructureType Type = StructureType.FenceCreateInfo;
	public readonly nint Next;
	public readonly FenceCreateFlags Flags;

	public Fence CreateFence(Device device, AllocationCallbacks? allocator)
	{
		Result result = vkCreateFence(device.Handle, in this, allocator?.Handle ?? default, out FenceHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetFence(device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateFence(DeviceHandle device, in FenceCreateInfo createInfo, AllocationCallbacksHandle allocator, out FenceHandle fence);
	}

	public FenceCreateInfo(nint next, FenceCreateFlags flags) => (this.Next, this.Flags) = (next, flags);
}
