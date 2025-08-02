using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct DescriptorSetAllocateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	private readonly nint descriptorPool;
	private readonly uint descriptorSetCount;
	private readonly Handle<nint> setLayouts;

	public DescriptorPool DescriptorPool => throw new NotImplementedException(); // cannot get allocator and device params
	public DescriptorSetLayout[]? SetLayouts => throw new NotImplementedException(); // cannot get allocator and device params

	public DescriptorSet[] CreateDescriptorSets(Device device, DescriptorPool descriptorPool) 
	{
		var descriptorSetHandles = new nint[descriptorSetCount];

		Result result = vkAllocateDescriptorSets((nint)device, in this, descriptorSetHandles.AsPointer());
		if (result != Result.Success) throw new VulkanException(result);

		return descriptorSetHandles.Select(x => new DescriptorSet(device, x, descriptorPool)).ToArray();

		[DllImport(VK_LIB)] static extern Result vkAllocateDescriptorSets(nint device, in DescriptorSetAllocateInfo createInfo, nint pDescriptorSets);
	}

	public void Dispose() 
	{
		setLayouts.Dispose();
	}

	public DescriptorSetAllocateInfo(
		StructureType type,
		nint next,
		DescriptorPool descriptorPool,
		DescriptorSetLayout[]? setLayouts
	)
	{
		this.Type = type;
		this.Next = next;
		this.descriptorPool = (nint)descriptorPool;

		this.descriptorSetCount = (uint)(setLayouts?.Length ?? 0);
		this.setLayouts = new(setLayouts?.Select(x => (nint)x).ToArray());
	}
}
