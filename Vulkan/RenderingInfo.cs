using System;

namespace Vulkan;

public readonly struct RenderingInfo : IDisposable
{
	public readonly StructureType Type = StructureType.RenderingInfo;
	public readonly nint Next;
	public readonly RenderingFlags Flags;
	public readonly Rect2D RenderArea;
	public readonly uint LayerCount;
	public readonly uint ViewMask;
	private readonly uint colorAttachmentCount;
	private readonly Box<RenderingAttachmentInfo> colorAttachments;
	private readonly Box<RenderingAttachmentInfo> depthAttachment;
	private readonly Box<RenderingAttachmentInfo> stencilAttachment;

	public RenderingAttachmentInfo[]? ColorAttachments => colorAttachments.ToArray(colorAttachmentCount);
	public RenderingAttachmentInfo DepthAttachment => depthAttachment;
	public RenderingAttachmentInfo StencilAttachment => stencilAttachment;

	public void Dispose()
	{
		colorAttachments.Dispose();
		depthAttachment.Dispose();
		stencilAttachment.Dispose();
	}

	public RenderingInfo(
		nint next,
		RenderingFlags flags,
		Rect2D renderArea,
		uint layerCount,
		uint viewMask,
		RenderingAttachmentInfo[]? colorAttachments,
		RenderingAttachmentInfo? depthAttachment,
		RenderingAttachmentInfo? stencilAttachment
	)
	{
		this.Next = next;
		this.Flags = flags;
		this.RenderArea = renderArea;
		this.LayerCount = layerCount;
		this.ViewMask = viewMask;

		this.colorAttachmentCount = (uint)(colorAttachments?.Length ?? 0);
		this.colorAttachments = new(colorAttachments);

		this.depthAttachment = (depthAttachment != null) ? new((RenderingAttachmentInfo)depthAttachment) : default;
		this.stencilAttachment = (stencilAttachment != null) ? new((RenderingAttachmentInfo)stencilAttachment) : default;
	}
}
