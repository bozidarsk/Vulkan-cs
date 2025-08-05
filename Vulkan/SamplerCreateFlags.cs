namespace Vulkan;

[System.Flags]
public enum SamplerCreateFlags : uint
{
	Subsampled = 0x00000001,
	SubsampledCoarseReconstruction = 0x00000002,
	DescriptorBufferCaptureReplay = 0x00000008,
	NonSeamlessCubeMap = 0x00000004,
	ImageProcessingQcom = 0x00000010,
}
