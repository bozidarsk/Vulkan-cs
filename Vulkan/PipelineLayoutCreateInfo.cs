using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct PipelineLayoutCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.PipelineLayoutCreateInfo;
	public readonly nint Next;
	public readonly PipelineLayoutCreateFlags Flags;
	private readonly uint setLayoutCount;
	private readonly Box<DescriptorSetLayoutHandle> setLayouts;
	private readonly uint pushConstantRangeCount;
	private readonly Box<PushConstantRange> pushConstantRanges;

	public DescriptorSetLayout[]? SetLayouts => throw new NotImplementedException(); // cannot get allocator and device params
	public PushConstantRange[]? PushConstantRanges => pushConstantRanges.ToArray(pushConstantRangeCount);

	public PipelineLayout CreatePipelineLayout(Device device, AllocationCallbacks? allocator)
	{
		Result result = vkCreatePipelineLayout(device.Handle, in this, allocator?.Handle ?? default, out PipelineLayoutHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(handle, device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreatePipelineLayout(DeviceHandle device, in PipelineLayoutCreateInfo createInfo, AllocationCallbacksHandle allocator, out PipelineLayoutHandle pipelineLayout);
	}

	public void Dispose()
	{
		setLayouts.Dispose();
		pushConstantRanges.Dispose();
	}

	public PipelineLayoutCreateInfo(
		nint next,
		PipelineLayoutCreateFlags flags,
		DescriptorSetLayout[]? setLayouts,
		PushConstantRange[]? pushConstantRanges
	)
	{
		this.Next = next;
		this.Flags = flags;

		this.setLayoutCount = (uint)(setLayouts?.Length ?? 0);
		this.setLayouts = new(setLayouts?.Select(x => x.Handle).ToArray());

		this.pushConstantRangeCount = (uint)(pushConstantRanges?.Length ?? 0);
		this.pushConstantRanges = new(pushConstantRanges);
	}
}
