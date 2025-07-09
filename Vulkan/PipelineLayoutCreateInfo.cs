using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct PipelineLayoutCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly PipelineLayoutCreateFlags Flags;
	private readonly uint setLayoutCount;
	private readonly Handle<DescriptorSetLayout> setLayouts;
	private readonly uint pushConstantRangeCount;
	private readonly Handle<PushConstantRange> pushConstantRanges;

	public DescriptorSetLayout[]? SetLayouts => setLayouts.ToArray(setLayoutCount);
	public PushConstantRange[]? PushConstantRanges => pushConstantRanges.ToArray(pushConstantRangeCount);

	public PipelineLayout CreatePipelineLayout(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreatePipelineLayout((nint)device, in this, allocator, out nint pipelineLayoutHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, pipelineLayoutHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreatePipelineLayout(nint device, in PipelineLayoutCreateInfo createInfo, nint allocator, out nint pipelineLayout);
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
		this.setLayouts = new(setLayouts);

		this.pushConstantRangeCount = (uint)(pushConstantRanges?.Length ?? 0);
		this.pushConstantRanges = new(pushConstantRanges);
	}
}
