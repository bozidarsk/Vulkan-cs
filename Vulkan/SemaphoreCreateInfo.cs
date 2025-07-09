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
		Result result = vkCreateSemaphore((nint)device, in this, allocator, out nint semaphoreHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, semaphoreHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateSemaphore(nint device, in SemaphoreCreateInfo createInfo, nint allocator, out nint semaphore);
	}

	public SemaphoreCreateInfo(StructureType type, nint next, SemaphoreCreateFlags flags) => (this.Type, this.Next, this.Flags) = (type, next, flags);
}
