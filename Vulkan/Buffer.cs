using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Buffer : IDisposable
{
	private readonly Device device;
	private readonly nint buffer;
	private readonly Handle<AllocationCallbacks> allocator;

	public MemoryRequirements MemoryRequirements 
	{
		get 
		{
			vkGetBufferMemoryRequirements((nint)device, buffer, out MemoryRequirements requirements);
			return requirements;

			[DllImport(VK_LIB)] static extern void vkGetBufferMemoryRequirements(nint device, nint buffer, out MemoryRequirements requirements);
		}
	}

	public void Bind(DeviceMemory memory, DeviceSize offset = default) 
	{
		Result result = vkBindBufferMemory((nint)device, (nint)this, (nint)memory, offset);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkBindBufferMemory(nint device, nint buffer, nint memory, DeviceSize offset);
	}

	public static explicit operator nint (Buffer x) => x.buffer;

	public void Dispose() 
	{
		vkDestroyBuffer((nint)device, buffer, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyBuffer(nint device, nint buffer, nint allocator);
	}

	private Buffer(Device device, nint buffer) => (this.device, this.buffer) = (device, buffer);
	internal Buffer(Device device, nint buffer, Handle<AllocationCallbacks> allocator) : this(device, buffer) => this.allocator = allocator;
}
