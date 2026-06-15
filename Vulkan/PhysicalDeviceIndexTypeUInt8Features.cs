namespace Vulkan;

public readonly struct PhysicalDeviceIndexTypeUInt8Features
{
	public readonly StructureType Type = StructureType.PhysicalDeviceIndexTypeUInt8Features;
	public readonly nint Next;
	public readonly bool32 IndexTypeUInt8;

	public PhysicalDeviceIndexTypeUInt8Features(
		nint next,
		bool indexTypeUInt8
	)
	{
		this.Next = next;
		this.IndexTypeUInt8 = indexTypeUInt8;
	}
}
