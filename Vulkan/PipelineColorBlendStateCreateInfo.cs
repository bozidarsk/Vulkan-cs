using System;

namespace Vulkan;

public readonly struct PipelineColorBlendStateCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.PipelineColorBlendStateCreateInfo;
	public readonly nint Next;
	public readonly PipelineColorBlendStateCreateFlags Flags;
	public readonly bool32 LogicOpEnable;
	public readonly LogicOp LogicOp;
	private readonly uint attachmentCount;
	private readonly Box<PipelineColorBlendAttachmentState> attachments;
	public readonly Color BlendConstants;

	public PipelineColorBlendAttachmentState[]? Attachments => attachments.ToArray(attachmentCount);

	public void Dispose()
	{
		attachments.Dispose();
	}

	public PipelineColorBlendStateCreateInfo(
		nint next,
		PipelineColorBlendStateCreateFlags flags,
		bool logicOpEnable,
		LogicOp logicOp,
		PipelineColorBlendAttachmentState[] attachments,
		Color blendConstants
	)
	{
		this.Next = next;
		this.Flags = flags;
		this.LogicOpEnable = logicOpEnable;
		this.LogicOp = logicOp;
		this.BlendConstants = blendConstants;

		this.attachmentCount = (uint)(attachments?.Length ?? 0);
		this.attachments = new(attachments);
	}
}
