using System;

namespace Vulkan;

public readonly struct RenderPassBeginInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	private readonly nint renderPass;
	private readonly nint framebuffer;
	public readonly Rect2D RenderArea;
	private readonly uint clearValueCount;
	private readonly Handle<ClearValue> clearValues;

	public RenderPass RenderPass => throw new NotImplementedException(); // cannot get allocator and device params
	public Framebuffer Framebuffer => throw new NotImplementedException(); // cannot get allocator and device params

	public void Dispose() 
	{
		clearValues.Dispose();
	}

	public RenderPassBeginInfo(
		StructureType type,
		nint next,
		RenderPass renderPass,
		Framebuffer framebuffer,
		Rect2D renderArea,
		ClearValue[]? clearValues
	)
	{
		this.Type = type;
		this.Next = next;
		this.renderPass = (nint)renderPass;
		this.framebuffer = (nint)framebuffer;
		this.RenderArea = renderArea;

		this.clearValueCount = (uint)(clearValues?.Length ?? 0);
		this.clearValues = new(clearValues);
	}
}
