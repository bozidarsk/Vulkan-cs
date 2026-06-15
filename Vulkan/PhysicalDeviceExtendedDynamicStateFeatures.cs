namespace Vulkan;

public readonly struct PhysicalDeviceExtendedDynamicStateFeatures
{
	public readonly StructureType Type = StructureType.PhysicalDeviceExtendedDynamicStateFeatures;
	public readonly nint Next;
	public readonly bool32 ExtendedDynamicState;

	public PhysicalDeviceExtendedDynamicStateFeatures(
		nint next,
		bool extendedDynamicState
	)
	{
		this.Next = next;
		this.ExtendedDynamicState = extendedDynamicState;
	}
}
