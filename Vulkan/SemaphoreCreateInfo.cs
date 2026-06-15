using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct SemaphoreCreateInfo
{
	public readonly StructureType Type = StructureType.SemaphoreCreateInfo;
	public readonly nint Next;
	public readonly SemaphoreCreateFlags Flags;

	public Semaphore CreateSemaphore(Device device, AllocationCallbacksHandle allocator)
	{
		Result result = vkCreateSemaphore(device.Handle, in this, allocator, out SemaphoreHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetSemaphore(device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateSemaphore(DeviceHandle device, in SemaphoreCreateInfo createInfo, AllocationCallbacksHandle allocator, out SemaphoreHandle semaphore);
	}

	public SemaphoreCreateInfo(nint next, SemaphoreCreateFlags flags) => (this.Next, this.Flags) = (next, flags);
}
