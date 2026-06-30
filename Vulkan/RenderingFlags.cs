namespace Vulkan;

[System.Flags]
public enum RenderingFlags : uint
{
	ContentsSecondaryCommandBuffers = 0x00000001,
	Suspending = 0x00000002,
	Resuming = 0x00000004,
	EnableLegacyDitheringExt = 0x00000008,
	ContentsInlineKhr = 0x00000010,
	PerLayerFragmentDensityValve = 0x00000020,
	FragmentRegionExt = 0x00000040,
	CustomResolveExt = 0x00000080,
	LocalReadConcurrentAccessControlKhr = 0x00000100,
	ContentsSecondaryCommandBuffersKhr = ContentsSecondaryCommandBuffers,
	SuspendingKhr = Suspending,
	ResumingKhr = Resuming,
	ContentsInlineExt = ContentsInlineKhr,
}
