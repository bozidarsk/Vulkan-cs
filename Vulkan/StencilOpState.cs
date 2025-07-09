namespace Vulkan;

public readonly struct StencilOpState 
{
	public readonly StencilOp FailOp;
	public readonly StencilOp PassOp;
	public readonly StencilOp DepthFailOp;
	public readonly CompareOp CompareOp;
	public readonly uint CompareMask;
	public readonly uint WriteMask;
	public readonly uint Reference;

	public StencilOpState(
		StencilOp failOp,
		StencilOp passOp,
		StencilOp depthFailOp,
		CompareOp compareOp,
		uint compareMask,
		uint writeMask,
		uint reference
	)
	{
		this.FailOp = failOp;
		this.PassOp = passOp;
		this.DepthFailOp = depthFailOp;
		this.CompareOp = compareOp;
		this.CompareMask = compareMask;
		this.WriteMask = writeMask;
		this.Reference = reference;
	}
}
