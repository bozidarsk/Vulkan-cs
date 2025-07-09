namespace Vulkan;

public readonly struct PipelineDepthStencilStateCreateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly PipelineDepthStencilStateCreateFlags Flags;
	public readonly bool32 DepthTestEnable;
	public readonly bool32 DepthWriteEnable;
	public readonly CompareOp DepthCompareOp;
	public readonly bool32 DepthBoundsTestEnable;
	public readonly bool32 StencilTestEnable;
	public readonly StencilOpState Front;
	public readonly StencilOpState Back;
	public readonly float MinDepthBounds;
	public readonly float MaxDepthBounds;

	public PipelineDepthStencilStateCreateInfo(
		StructureType type,
		nint next,
		PipelineDepthStencilStateCreateFlags flags,
		bool depthTestEnable,
		bool depthWriteEnable,
		CompareOp depthCompareOp,
		bool depthBoundsTestEnable,
		bool stencilTestEnable,
		StencilOpState front,
		StencilOpState back,
		float minDepthBounds,
		float maxDepthBounds
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.DepthTestEnable = depthTestEnable;
		this.DepthWriteEnable = depthWriteEnable;
		this.DepthCompareOp = depthCompareOp;
		this.DepthBoundsTestEnable = depthBoundsTestEnable;
		this.StencilTestEnable = stencilTestEnable;
		this.Front = front;
		this.Back = back;
		this.MinDepthBounds = minDepthBounds;
		this.MaxDepthBounds = maxDepthBounds;
	}
}
