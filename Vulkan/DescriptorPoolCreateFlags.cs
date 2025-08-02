namespace Vulkan;

[System.Flags]
public enum DescriptorPoolCreateFlags : uint
{
	FreeDescriptorSet = 0x00000001,
	UpdateAfterBind = 0x00000002,
	HostOnlyExt = 0x00000004,
	AllowOverallocationSetsNv = 0x00000008,
	AllowOverallocationPoolsNv = 0x00000010,
	UpdateAfterBindExt = UpdateAfterBind,
	HostOnlyValve = HostOnlyExt,
}
