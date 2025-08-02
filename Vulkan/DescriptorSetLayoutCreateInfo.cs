using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct DescriptorSetLayoutCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly DescriptorSetLayoutCreateFlags Flags;
	private readonly uint bindingCount;
	private readonly Handle<DescriptorSetLayoutBinding> bindings;

	public DescriptorSetLayoutBinding[]? Bindings => bindings.ToArray(bindingCount);

	public DescriptorSetLayout CreateDescriptorSetLayout(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateDescriptorSetLayout((nint)device, in this, allocator, out nint descriptorSetLayoutHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, descriptorSetLayoutHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateDescriptorSetLayout(nint device, in DescriptorSetLayoutCreateInfo createInfo, nint allocator, out nint descriptorSetLayout);
	}

	public void Dispose() 
	{
		bindings.Dispose();
	}

	public DescriptorSetLayoutCreateInfo(
		StructureType type,
		nint next,
		DescriptorSetLayoutCreateFlags flags,
		DescriptorSetLayoutBinding[]? bindings
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;

		this.bindingCount = (uint)(bindings?.Length ?? 0);
		this.bindings = new(bindings);
	}
}
