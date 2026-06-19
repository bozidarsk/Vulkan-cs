namespace Vulkan;

public readonly struct ColorBlendEquation
{
	public readonly BlendFactor SourceColorBlendFactor;
	public readonly BlendFactor DestinationColorBlendFactor;
	public readonly BlendOp ColorBlendOp;
	public readonly BlendFactor SourceAlphaBlendFactor;
	public readonly BlendFactor DestinationAlphaBlendFactor;
	public readonly BlendOp AlphaBlendOp;

	public ColorBlendEquation(BlendFactor sourceBlendFactor, BlendFactor destinationBlendFactor, BlendOp blendOp) :
		this(sourceBlendFactor, destinationBlendFactor, blendOp, sourceBlendFactor, destinationBlendFactor, blendOp)
	{
	}

	public ColorBlendEquation(
		BlendFactor sourceColorBlendFactor,
		BlendFactor destinationColorBlendFactor,
		BlendOp colorBlendOp,
		BlendFactor sourceAlphaBlendFactor,
		BlendFactor destinationAlphaBlendFactor,
		BlendOp alphaBlendOp
	)
	{
		this.SourceColorBlendFactor = sourceColorBlendFactor;
		this.DestinationColorBlendFactor = destinationColorBlendFactor;
		this.ColorBlendOp = colorBlendOp;
		this.SourceAlphaBlendFactor = sourceAlphaBlendFactor;
		this.DestinationAlphaBlendFactor = destinationAlphaBlendFactor;
		this.AlphaBlendOp = alphaBlendOp;
	}
}
