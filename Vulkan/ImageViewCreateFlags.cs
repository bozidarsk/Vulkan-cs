namespace Vulkan;

[System.Flags]
public enum ImageViewCreateFlags : uint
{
	FragmentDensityMapDynamic = 0x00000001,
	DescriptorBufferCaptureReplay = 0x00000004,
	FragmentDensityMapDeferred = 0x00000002,
}
