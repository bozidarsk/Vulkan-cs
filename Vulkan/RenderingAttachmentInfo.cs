using System;

namespace Vulkan;

public readonly struct RenderingAttachmentInfo
{
	public readonly StructureType Type = StructureType.RenderingAttachmentInfo;
	public readonly nint Next;
	private readonly ImageViewHandle imageView;
	public readonly ImageLayout ImageLayout;
	public readonly ResolveMode ResolveMode;
	private readonly ImageViewHandle resolveImageView;
	public readonly ImageLayout ResolveImageLayout;
	public readonly AttachmentLoadOp LoadOp;
	public readonly AttachmentStoreOp StoreOp;
	public readonly ClearValue ClearValue;

	public ImageView ImageView => throw new NotImplementedException(); // cannot get allocator and device params
	public ImageView ResolveImageView => throw new NotImplementedException(); // cannot get allocator and device params

	public RenderingAttachmentInfo(
		nint next,
		ImageView imageView,
		ImageLayout imageLayout,
		ResolveMode resolveMode,
		ImageView? resolveImageView,
		ImageLayout resolveImageLayout,
		AttachmentLoadOp loadOp,
		AttachmentStoreOp storeOp,
		ClearValue clearValue
	)
	{
		this.Next = next;
		this.imageView = imageView.Handle;
		this.ImageLayout = imageLayout;
		this.ResolveMode = resolveMode;
		this.resolveImageView = resolveImageView?.Handle ?? default;
		this.ResolveImageLayout = resolveImageLayout;
		this.LoadOp = loadOp;
		this.StoreOp = storeOp;
		this.ClearValue = clearValue;
	}
}
