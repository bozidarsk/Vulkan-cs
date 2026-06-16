using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct DescriptorPoolCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.DescriptorPoolCreateInfo;
	public readonly nint Next;
	public readonly DescriptorPoolCreateFlags Flags;
	public readonly uint MaxSets;
	private readonly uint poolSizeCount;
	private readonly Box<DescriptorPoolSize> poolSizes;

	public DescriptorPoolSize[]? PoolSizes => poolSizes.ToArray(poolSizeCount);

	public DescriptorPool CreateDescriptorPool(Device device, AllocationCallbacks? allocator)
	{
		Result result = vkCreateDescriptorPool(device.Handle, in this, allocator?.Handle ?? default, out DescriptorPoolHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(handle, device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateDescriptorPool(DeviceHandle device, in DescriptorPoolCreateInfo createInfo, AllocationCallbacksHandle allocator, out DescriptorPoolHandle descriptorPool);
	}

	public void Dispose()
	{
		poolSizes.Dispose();
	}

	public DescriptorPoolCreateInfo(
		nint next,
		DescriptorPoolCreateFlags flags,
		uint maxSets,
		DescriptorPoolSize[]? poolSizes
	)
	{
		this.Next = next;
		this.Flags = flags;
		this.MaxSets = maxSets;

		this.poolSizeCount = (uint)(poolSizes?.Length ?? 0);
		this.poolSizes = new(poolSizes);
	}
}
