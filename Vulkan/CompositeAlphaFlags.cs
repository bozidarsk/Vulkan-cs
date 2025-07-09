namespace Vulkan;

[System.Flags]
public enum CompositeAlphaFlags : uint
{
	Opaque = 0x00000001,
	PreMultiplied = 0x00000002,
	PostMultiplied = 0x00000004,
	Inherit = 0x00000008,
}
