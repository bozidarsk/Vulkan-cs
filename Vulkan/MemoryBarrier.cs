namespace Vulkan;

public readonly struct MemoryBarrier
{
	public readonly StructureType Type = StructureType.MemoryBarrier;
	public readonly nint Next;
	public readonly Access SrcAccess;
	public readonly Access DstAccess;

	public MemoryBarrier(
		nint next,
		Access srcAccess,
		Access dstAccess
	)
	{
		this.Next = next;
		this.SrcAccess = srcAccess;
		this.DstAccess = dstAccess;
	}
}
