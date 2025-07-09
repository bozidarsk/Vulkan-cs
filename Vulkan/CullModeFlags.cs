namespace Vulkan;

[System.Flags]
public enum CullMode : uint
{
	None = 0,
	Front = 0x00000001,
	Back = 0x00000002,
	FrontAndBack = 0x00000003,
}
