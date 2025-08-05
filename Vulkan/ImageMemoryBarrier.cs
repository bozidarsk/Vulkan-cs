using System;

namespace Vulkan;

public readonly struct ImageMemoryBarrier 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly Access SrcAccess;
	public readonly Access DstAccess;
	public readonly ImageLayout OldLayout;
	public readonly ImageLayout NewLayout;
	public readonly uint SrcQueueFamilyIndex;
	public readonly uint DstQueueFamilyIndex;
	private readonly ImageHandle image;
	public readonly ImageSubresourceRange SubresourceRange;

	public Image Image => throw new NotImplementedException(); // cannot get allocator and device params

	public ImageMemoryBarrier(
		StructureType type,
		nint next,
		Access srcAccess,
		Access dstAccess,
		ImageLayout oldLayout,
		ImageLayout newLayout,
		uint srcQueueFamilyIndex,
		uint dstQueueFamilyIndex,
		Image image,
		ImageSubresourceRange subresourceRange
	)
	{
		this.Type = type;
		this.Next = next;
		this.SrcAccess = srcAccess;
		this.DstAccess = dstAccess;
		this.OldLayout = oldLayout;
		this.NewLayout = newLayout;
		this.SrcQueueFamilyIndex = srcQueueFamilyIndex;
		this.DstQueueFamilyIndex = dstQueueFamilyIndex;
		this.image = image.Handle;
		this.SubresourceRange = subresourceRange;
	}
}
