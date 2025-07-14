namespace Vulkan;

public readonly struct PhysicalDeviceIndexTypeUInt8Features 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly bool32 IndexTypeUInt8;

	public PhysicalDeviceIndexTypeUInt8Features(
		StructureType type,
		nint next,
		bool indexTypeUInt8
	)
	{
		this.Type = type;
		this.Next = next;
		this.IndexTypeUInt8 = indexTypeUInt8;
	}
}
