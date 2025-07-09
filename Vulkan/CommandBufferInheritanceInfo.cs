using System;

namespace Vulkan;

public readonly struct CommandBufferInheritanceInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	private readonly nint renderPass;
	public readonly uint Subpass;
	private readonly nint framebuffer;
	public readonly bool32 OcclusionQueryEnable;
	public readonly QueryControlFlags QueryFlags;
	public readonly QueryPipelineStatisticFlags PipelineStatistics;

	public RenderPass RenderPass => throw new NotImplementedException(); // cannot get allocator and device params
	public Framebuffer Framebuffer => throw new NotImplementedException(); // cannot get allocator and device params

	public CommandBufferInheritanceInfo(
		StructureType type,
		nint next,
		RenderPass renderPass,
		uint subpass,
		Framebuffer framebuffer,
		bool occlusionQueryEnable,
		QueryControlFlags queryFlags,
		QueryPipelineStatisticFlags pipelineStatistics
	)
	{
		this.Type = type;
		this.Next = next;
		this.renderPass = (nint)renderPass;
		this.Subpass = subpass;
		this.framebuffer = (nint)framebuffer;
		this.OcclusionQueryEnable = occlusionQueryEnable;
		this.QueryFlags = queryFlags;
		this.PipelineStatistics = pipelineStatistics;
	}
}
