namespace Vulkan;

public readonly struct AttachmentDescription 
{
	public readonly	AttachmentDescriptionFlags Flags;
	public readonly	Format Format;
	public readonly	SampleCount Samples;
	public readonly	AttachmentLoadOp LoadOp;
	public readonly	AttachmentStoreOp StoreOp;
	public readonly	AttachmentLoadOp StencilLoadOp;
	public readonly	AttachmentStoreOp StencilStoreOp;
	public readonly	ImageLayout InitialLayout;
	public readonly	ImageLayout FinalLayout;

	public AttachmentDescription(
		AttachmentDescriptionFlags flags,
		Format format,
		SampleCount samples,
		AttachmentLoadOp loadOp,
		AttachmentStoreOp storeOp,
		AttachmentLoadOp stencilLoadOp,
		AttachmentStoreOp stencilStoreOp,
		ImageLayout initialLayout,
		ImageLayout finalLayout
	)
	{
		this.Flags = flags;
		this.Format = format;
		this.Samples = samples;
		this.LoadOp = loadOp;
		this.StoreOp = storeOp;
		this.StencilLoadOp = stencilLoadOp;
		this.StencilStoreOp = stencilStoreOp;
		this.InitialLayout = initialLayout;
		this.FinalLayout = finalLayout;
	}
}
