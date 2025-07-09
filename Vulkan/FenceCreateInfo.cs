using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct FenceCreateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly FenceCreateFlags Flags;

	public Fence CreateFence(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateFence((nint)device, in this, allocator, out nint fenceHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, fenceHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateFence(nint device, in FenceCreateInfo createInfo, nint allocator, out nint fence);
	}

	public FenceCreateInfo(StructureType type, nint next, FenceCreateFlags flags) => (this.Type, this.Next, this.Flags) = (type, next, flags);
}
