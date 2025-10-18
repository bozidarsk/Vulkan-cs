using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct PipelineLayoutCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly PipelineLayoutCreateFlags Flags;
	private readonly uint setLayoutCount;
	private readonly Handle<DescriptorSetLayoutHandle> setLayouts;
	private readonly uint pushConstantRangeCount;
	private readonly Handle<PushConstantRange> pushConstantRanges;

	public DescriptorSetLayout[]? SetLayouts => throw new NotImplementedException(); // cannot get allocator and device params
	public PushConstantRange[]? PushConstantRanges => pushConstantRanges.ToArray(pushConstantRangeCount);

	public PipelineLayout CreatePipelineLayout(Device device, AllocationCallbacksHandle allocator) 
	{
		Result result = vkCreatePipelineLayout(device.Handle, in this, allocator, out PipelineLayoutHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetPipelineLayout(device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreatePipelineLayout(DeviceHandle device, in PipelineLayoutCreateInfo createInfo, AllocationCallbacksHandle allocator, out PipelineLayoutHandle pipelineLayout);
	}

	public void Dispose() 
	{
		setLayouts.Dispose();
		pushConstantRanges.Dispose();
	}

	public PipelineLayoutCreateInfo(
		StructureType type,
		nint next,
		PipelineLayoutCreateFlags flags,
		DescriptorSetLayout[]? setLayouts,
		PushConstantRange[]? pushConstantRanges
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;

		this.setLayoutCount = (uint)(setLayouts?.Length ?? 0);
		this.setLayouts = new(setLayouts?.Select(x => x.Handle).ToArray());

		this.pushConstantRangeCount = (uint)(pushConstantRanges?.Length ?? 0);
		this.pushConstantRanges = new(pushConstantRanges);
	}
}
