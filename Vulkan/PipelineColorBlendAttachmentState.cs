namespace Vulkan;

public readonly struct PipelineColorBlendAttachmentState 
{
	public readonly bool32 BlendEnable;
	public readonly BlendFactor SrcColorBlendFactor;
	public readonly BlendFactor DstColorBlendFactor;
	public readonly BlendOp ColorBlendOp;
	public readonly BlendFactor SrcAlphaBlendFactor;
	public readonly BlendFactor DstAlphaBlendFactor;
	public readonly BlendOp AlphaBlendOp;
	public readonly ColorComponent ColorWriteMask;

	public PipelineColorBlendAttachmentState(
		bool blendEnable,
		BlendFactor srcColorBlendFactor,
		BlendFactor dstColorBlendFactor,
		BlendOp colorBlendOp,
		BlendFactor srcAlphaBlendFactor,
		BlendFactor dstAlphaBlendFactor,
		BlendOp alphaBlendOp,
		ColorComponent colorWriteMask
	)
	{
		this.BlendEnable = blendEnable;
		this.SrcColorBlendFactor = srcColorBlendFactor;
		this.DstColorBlendFactor = dstColorBlendFactor;
		this.ColorBlendOp = colorBlendOp;
		this.SrcAlphaBlendFactor = srcAlphaBlendFactor;
		this.DstAlphaBlendFactor = dstAlphaBlendFactor;
		this.AlphaBlendOp = alphaBlendOp;
		this.ColorWriteMask = colorWriteMask;
	}
}
