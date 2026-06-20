using System;

namespace Vulkan;

public unsafe struct PipelineColorBlendStateCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.PipelineColorBlendStateCreateInfo;
	public readonly nint Next;
	public readonly PipelineColorBlendStateCreateFlags Flags;
	public readonly bool32 LogicOpEnable;
	public readonly LogicOp LogicOp;
	private readonly uint attachmentCount;
	private readonly Box<PipelineColorBlendAttachmentState> attachments;
	private fixed float blendConstants[4];

	public PipelineColorBlendAttachmentState[]? Attachments => attachments.ToArray(attachmentCount);
	public (float r, float g, float b, float a) BlendConstants => (blendConstants[0], blendConstants[1], blendConstants[2], blendConstants[3]);

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
		(float r, float g, float b, float a) blendConstants
	)
	{
		this.Next = next;
		this.Flags = flags;
		this.LogicOpEnable = logicOpEnable;
		this.LogicOp = logicOp;

		(this.blendConstants[0], this.blendConstants[1], this.blendConstants[2], this.blendConstants[3]) = blendConstants;

		this.attachmentCount = (uint)(attachments?.Length ?? 0);
		this.attachments = new(attachments);
	}
}
