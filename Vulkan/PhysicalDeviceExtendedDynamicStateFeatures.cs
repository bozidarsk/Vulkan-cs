namespace Vulkan;

public readonly struct PhysicalDeviceExtendedDynamicStateFeatures 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly bool32 ExtendedDynamicState;

	public PhysicalDeviceExtendedDynamicStateFeatures(
		StructureType type,
		nint next,
		bool extendedDynamicState
	)
	{
		this.Type = type;
		this.Next = next;
		this.ExtendedDynamicState = extendedDynamicState;
	}
}
