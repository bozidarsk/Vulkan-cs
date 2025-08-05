namespace Vulkan;

[System.Flags]
public enum DependencyFlags : uint
{
	ByRegion = 0x00000001,
	DeviceGroup = 0x00000004,
	ViewLocal = 0x00000002,
	FeedbackLoopExt = 0x00000008,
	QueueFamilyOwnershipTransferUseAllStagesKhr = 0x00000020,
	AsymmetricEventKhr = 0x00000040,
	ViewLocalKhr = ViewLocal,
	DeviceGroupKhr = DeviceGroup,
}
