using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct DescriptorPoolCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly DescriptorPoolCreateFlags Flags;
	public readonly uint MaxSets;
	private readonly uint poolSizeCount;
	private readonly Handle<DescriptorPoolSize> poolSizes;

	public DescriptorPoolSize[]? PoolSizes => poolSizes.ToArray(poolSizeCount);

	public DescriptorPool CreateDescriptorPool(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateDescriptorPool((nint)device, in this, allocator, out nint descriptorPoolHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, descriptorPoolHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateDescriptorPool(nint device, in DescriptorPoolCreateInfo createInfo, nint allocator, out nint descriptorPool);
	}

	public void Dispose() 
	{
		poolSizes.Dispose();
	}

	public DescriptorPoolCreateInfo(
		StructureType type,
		nint next,
		DescriptorPoolCreateFlags flags,
		uint maxSets,
		DescriptorPoolSize[]? poolSizes
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.MaxSets = maxSets;

		this.poolSizeCount = (uint)(poolSizes?.Length ?? 0);
		this.poolSizes = new(poolSizes);
	}
}
