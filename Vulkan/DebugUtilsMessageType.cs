namespace Vulkan;

[System.Flags]
public enum DebugUtilsMessageType : uint
{
	General = 0x00000001,
	Validation = 0x00000002,
	Performance = 0x00000004,
	DeviceAddressBinding = 0x00000008,
}
