using System;

namespace Vulkan;

public readonly struct DescriptorSetLayoutBindingFlagsCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.DescriptorSetLayoutBindingFlagsCreateInfo;
	public readonly nint Next;
	private readonly uint bindingCount;
	private readonly Box<DescriptorBindingFlags> bindingFlags;

	public DescriptorBindingFlags[]? BindingFlags => bindingFlags.ToArray(bindingCount);

	public void Dispose()
	{
		bindingFlags.Dispose();
	}

	public DescriptorSetLayoutBindingFlagsCreateInfo(
		nint next,
		DescriptorBindingFlags[]? bindingFlags
	)
	{
		this.Next = next;
		this.bindingCount = (uint)(bindingFlags?.Length ?? 0);
		this.bindingFlags = new(bindingFlags);
	}
}
