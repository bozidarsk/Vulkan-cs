using System;

namespace Vulkan;

public readonly struct SubpassDescription : IDisposable
{
	public readonly SubpassDescriptionFlags Flags;
	public readonly PipelineBindPoint PipelineBindPoint;
	private readonly uint inputAttachmentCount;
	private readonly Handle<AttachmentReference> inputAttachments;
	private readonly uint colorAttachmentCount;
	private readonly Handle<AttachmentReference> colorAttachments;
	private readonly Handle<AttachmentReference> resolveAttachments;
	private readonly Handle<AttachmentReference> depthStencilAttachment;
	private readonly uint preserveAttachmentCount;
	private readonly Handle<uint> preserveAttachments;

	public AttachmentReference[]? InputAttachments => inputAttachments.ToArray(inputAttachmentCount);
	public AttachmentReference[]? ColorAttachments => colorAttachments.ToArray(colorAttachmentCount);
	public AttachmentReference[]? ResolveAttachments => resolveAttachments.ToArray(colorAttachmentCount);
	public AttachmentReference DepthStencilAttachment => depthStencilAttachment;
	public uint[]? PreserveAttachments => preserveAttachments.ToArray(preserveAttachmentCount);

	public void Dispose() 
	{
		inputAttachments.Dispose();
		colorAttachments.Dispose();
		resolveAttachments.Dispose();
		depthStencilAttachment.Dispose();
		preserveAttachments.Dispose();
	}

	public SubpassDescription(
		SubpassDescriptionFlags flags,
		PipelineBindPoint pipelineBindPoint,
		AttachmentReference[]? inputAttachments,
		AttachmentReference[]? colorAttachments,
		AttachmentReference[]? resolveAttachments,
		AttachmentReference? depthStencilAttachment,
		uint[]? preserveAttachments
	)
	{
		this.Flags = flags;
		this.PipelineBindPoint = pipelineBindPoint;

		this.inputAttachmentCount = (uint)(inputAttachments?.Length ?? 0);
		this.inputAttachments = new(inputAttachments);

		this.colorAttachmentCount = (uint)(colorAttachments?.Length ?? 0);
		this.colorAttachments = new(colorAttachments);
		this.resolveAttachments = new(resolveAttachments);

		this.preserveAttachmentCount = (uint)(preserveAttachments?.Length ?? 0);
		this.preserveAttachments = new(preserveAttachments);

		this.depthStencilAttachment = (depthStencilAttachment is AttachmentReference x) ? new(x) : default;
	}
}
