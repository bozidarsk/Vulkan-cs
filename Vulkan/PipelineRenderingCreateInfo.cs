using System;

namespace Vulkan;

public readonly struct PipelineRenderingCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.PipelineRenderingCreateInfo;
	public readonly nint Next;
	public readonly uint ViewMask;
	private readonly uint colorAttachmentCount;
	private readonly Box<Format> colorAttachmentFormats;
	public readonly Format DepthAttachmentFormat;
	public readonly Format StencilAttachmentFormat;

	public Format[]? ColorAttachmentFormats => colorAttachmentFormats.ToArray(colorAttachmentCount);

	public void Dispose()
	{
		colorAttachmentFormats.Dispose();
	}

	public PipelineRenderingCreateInfo(
		nint next,
		uint viewMask,
		Format[]? colorAttachmentFormats,
		Format depthAttachmentFormat,
		Format stencilAttachmentFormat
	)
	{
		this.Next = next;
		this.ViewMask = viewMask;
		this.DepthAttachmentFormat = depthAttachmentFormat;
		this.StencilAttachmentFormat = stencilAttachmentFormat;

		this.colorAttachmentCount = (uint)(colorAttachmentFormats?.Length ?? 0);
		this.colorAttachmentFormats = new(colorAttachmentFormats);
	}
}
