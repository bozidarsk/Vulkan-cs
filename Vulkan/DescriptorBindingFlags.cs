namespace Vulkan;

[System.Flags]
public enum DescriptorBindingFlags : uint
{
	UpdateAfterBind = 0x00000001,
	UpdateUnusedWhilePending = 0x00000002,
	PartiallyBound = 0x00000004,
	VariableDescriptorCount = 0x00000008,
	UpdateAfterBindExt = UpdateAfterBind,
	UpdateUnusedWhilePendingExt = UpdateUnusedWhilePending,
	PartiallyBoundExt = PartiallyBound,
	VariableDescriptorCountExt = VariableDescriptorCount,
}
