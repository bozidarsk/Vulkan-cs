using System;

namespace Vulkan;

public readonly struct PipelineViewportStateCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.PipelineViewportStateCreateInfo;
	public readonly nint Next;
	public readonly PipelineViewportStateCreateFlags Flags;
	private readonly uint viewportCount;
	private readonly Box<Viewport> viewports;
	private readonly uint scissorCount;
	private readonly Box<Rect2D> scissors;

	public Viewport[]? Viewports => viewports.ToArray(viewportCount);
	public Rect2D[]? Scissors => scissors.ToArray(scissorCount);

	public void Dispose()
	{
		viewports.Dispose();
		scissors.Dispose();
	}

	public PipelineViewportStateCreateInfo(nint next, PipelineViewportStateCreateFlags flags, Viewport[]? viewports, Rect2D[]? scissors)
	{
		this.Next = next;
		this.Flags = flags;

		this.viewportCount = (uint)(viewports?.Length ?? 0);
		this.viewports = new(viewports);

		this.scissorCount = (uint)(scissors?.Length ?? 0);
		this.scissors = new(scissors);
	}
}
