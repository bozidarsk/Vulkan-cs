namespace Vulkan;

public readonly struct MemoryBarrier2
{
	public readonly StructureType Type = StructureType.MemoryBarrier2;
	public readonly nint Next;
	public readonly PipelineStage2 SrcStage;
	public readonly Access2 SrcAccess;
	public readonly PipelineStage2 DstStage;
	public readonly Access2 DstAccess;

	public MemoryBarrier2(
		nint next,
		PipelineStage2 srcStage,
		Access2 srcAccess,
		PipelineStage2 dstStage,
		Access2 dstAccess
	)
	{
		this.Next = next;
		this.SrcStage = srcStage;
		this.SrcAccess = srcAccess;
		this.DstStage = dstStage;
		this.DstAccess = dstAccess;
	}
}
