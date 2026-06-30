using System;

namespace Vulkan;

public readonly struct ImageMemoryBarrier2
{
	public readonly StructureType Type = StructureType.ImageMemoryBarrier2;
	public readonly nint Next;
	public readonly PipelineStage2 SrcStage;
	public readonly Access2 SrcAccess;
	public readonly PipelineStage2 DstStage;
	public readonly Access2 DstAccess;
	public readonly ImageLayout OldLayout;
	public readonly ImageLayout NewLayout;
	public readonly uint SrcQueueFamilyIndex;
	public readonly uint DstQueueFamilyIndex;
	private readonly ImageHandle image;
	public readonly ImageSubresourceRange SubresourceRange;

	public Image Image => throw new NotImplementedException(); // cannot get allocator and device params

	public ImageMemoryBarrier2(
		nint next,
		PipelineStage2 srcStage,
		Access2 srcAccess,
		PipelineStage2 dstStage,
		Access2 dstAccess,
		ImageLayout oldLayout,
		ImageLayout newLayout,
		uint srcQueueFamilyIndex,
		uint dstQueueFamilyIndex,
		Image image,
		ImageSubresourceRange subresourceRange
	)
	{
		this.Next = next;
		this.SrcStage = srcStage;
		this.SrcAccess = srcAccess;
		this.DstStage = dstStage;
		this.DstAccess = dstAccess;
		this.OldLayout = oldLayout;
		this.NewLayout = newLayout;
		this.SrcQueueFamilyIndex = srcQueueFamilyIndex;
		this.DstQueueFamilyIndex = dstQueueFamilyIndex;
		this.image = image.Handle;
		this.SubresourceRange = subresourceRange;
	}
}
