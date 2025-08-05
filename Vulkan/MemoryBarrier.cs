namespace Vulkan;

public readonly struct MemoryBarrier 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly Access SrcAccess;
	public readonly Access DstAccess;

	public MemoryBarrier(
		StructureType type,
		nint next,
		Access srcAccess,
		Access dstAccess
	)
	{
		this.Type = type;
		this.Next = next;
		this.SrcAccess = srcAccess;
		this.DstAccess = dstAccess;
	}
}
