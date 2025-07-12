namespace Vulkan;

[System.Flags]
public enum BufferCreateFlags : uint
{
	SparseBinding = 0x00000001,
	SparseResidency = 0x00000002,
	SparseAliased = 0x00000004,
	Protected = 0x00000008,
	DeviceAddressCaptureReplay = 0x00000010,
	DescriptorBufferCaptureReplayExt = 0x00000020,
	VideoProfileIndependentKhr = 0x00000040,
	DeviceAddressCaptureReplayExt = DeviceAddressCaptureReplay,
	DeviceAddressCaptureReplayKhr = DeviceAddressCaptureReplay,
}
