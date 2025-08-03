using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct SemaphoreCreateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly SemaphoreCreateFlags Flags;

	public Semaphore CreateSemaphore(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateSemaphore(device.Handle, in this, allocator, out SemaphoreHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetSemaphore(device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateSemaphore(DeviceHandle device, in SemaphoreCreateInfo createInfo, nint allocator, out SemaphoreHandle semaphore);
	}

	public SemaphoreCreateInfo(StructureType type, nint next, SemaphoreCreateFlags flags) => (this.Type, this.Next, this.Flags) = (type, next, flags);
}
