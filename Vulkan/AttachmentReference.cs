namespace Vulkan;

public readonly struct AttachmentReference 
{
	public readonly uint Attachment;
	public readonly ImageLayout Layout;

	public AttachmentReference(uint attachment, ImageLayout layout) => (this.Attachment, this.Layout) = (attachment, layout);
}
