namespace Vulkan;

[System.Flags]
public enum FramebufferCreateFlags : uint
{
	Imageless = 0x00000001,
	ImagelessKhr = Imageless,
}
