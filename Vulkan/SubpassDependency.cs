namespace Vulkan;

public readonly struct SubpassDependency 
{
	public readonly uint SrcSubpass;
	public readonly uint DstSubpass;
	public readonly PipelineStage SrcStageMask;
	public readonly PipelineStage DstStageMask;
	public readonly Access SrcAccessMask;
	public readonly Access DstAccessMask;
	public readonly DependencyFlags DependencyFlags;

	public SubpassDependency(
		uint srcSubpass,
		uint dstSubpass,
		PipelineStage srcStageMask,
		PipelineStage dstStageMask,
		Access srcAccessMask,
		Access dstAccessMask,
		DependencyFlags dependencyFlags
	)
	{
		this.SrcSubpass = srcSubpass;
		this.DstSubpass = dstSubpass;
		this.SrcStageMask = srcStageMask;
		this.DstStageMask = dstStageMask;
		this.SrcAccessMask = srcAccessMask;
		this.DstAccessMask = dstAccessMask;
		this.DependencyFlags = dependencyFlags;
	}
}
