namespace Vulkan;

[System.Flags]
public enum SurfaceTransformFlags : uint
{
	Identity = 0x00000001,
	Rotate90 = 0x00000002,
	Rotate180 = 0x00000004,
	Rotate270 = 0x00000008,
	HorizontalMirror = 0x00000010,
	HorizontalMirrorRotate90 = 0x00000020,
	HorizontalMirrorRotate180 = 0x00000040,
	HorizontalMirrorRotate270 = 0x00000080,
	Inherit = 0x00000100,
}
