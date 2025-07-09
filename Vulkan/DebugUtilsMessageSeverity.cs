namespace Vulkan;

[System.Flags]
public enum DebugUtilsMessageSeverity : uint
{
	Verbose = 0x00000001,
	Info = 0x00000010,
	Warning = 0x00000100,
	Error = 0x00001000,
}
