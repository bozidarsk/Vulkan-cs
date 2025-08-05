namespace Vulkan;

public enum SamplerAddressMode : uint
{
	Repeat = 0,
	MirroredRepeat = 1,
	ClampToEdge = 2,
	ClampToBorder = 3,
	MirrorClampToEdge = 4,
	MirrorClampToEdgeKhr = MirrorClampToEdge,
}
