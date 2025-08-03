using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct DescriptorSetAllocateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	private readonly DescriptorPoolHandle descriptorPool;
	private readonly uint descriptorSetCount;
	private readonly Handle<DescriptorSetLayoutHandle> setLayouts;

	public DescriptorPool DescriptorPool => throw new NotImplementedException(); // cannot get allocator and device params
	public DescriptorSetLayout[]? SetLayouts => throw new NotImplementedException(); // cannot get allocator and device params

	public DescriptorSet[] CreateDescriptorSets(Device device, DescriptorPool descriptorPool) 
	{
		var descriptorSetHandles = new DescriptorSetHandle[descriptorSetCount];

		Result result = vkAllocateDescriptorSets(device.Handle, in this, ref MemoryMarshal.GetArrayDataReference(descriptorSetHandles));
		if (result != Result.Success) throw new VulkanException(result);

		return descriptorSetHandles.Select(x => x.GetDescriptorSet(device, descriptorPool)).ToArray();

		[DllImport(VK_LIB)] static extern Result vkAllocateDescriptorSets(DeviceHandle device, in DescriptorSetAllocateInfo createInfo, ref DescriptorSetHandle pDescriptorSets);
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
		this.descriptorPool = descriptorPool.Handle;

		this.descriptorSetCount = (uint)(setLayouts?.Length ?? 0);
		this.setLayouts = new(setLayouts?.Select(x => x.Handle).ToArray());
	}
}
