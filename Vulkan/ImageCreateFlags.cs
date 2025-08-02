namespace Vulkan;

[System.Flags]
public enum ImageCreateFlags : uint
{
	SparseBinding = 0x00000001,
	SparseResidency = 0x00000002,
	SparseAliased = 0x00000004,
	MutableFormat = 0x00000008,
	CubeCompatible = 0x00000010,
	Alias = 0x00000400,
	SplitInstanceBindRegions = 0x00000040,
	TwoDimensionalArrayCompatible = 0x00000020,
	BlockTexelViewCompatible = 0x00000080,
	ExtendedUsage = 0x00000100,
	Protected = 0x00000800,
	Disjoint = 0x00000200,
	CornerSampledNv = 0x00002000,
	SampleLocationsCompatibleDepthExt = 0x00001000,
	SubsampledExt = 0x00004000,
	DescriptorBufferCaptureReplayExt = 0x00010000,
	MultisampledRenderToSingleSampledExt = 0x00040000,
	TwoDimensionalViewCompatibleExt = 0x00020000,
	VideoProfileIndependentKhr = 0x00100000,
	FragmentDensityMapOffsetExt = 0x00008000,
	SplitInstanceBindRegionsKhr = SplitInstanceBindRegions,
	TwoDimensionalArrayCompatibleKhr = TwoDimensionalArrayCompatible,
	BlockTexelViewCompatibleKhr = BlockTexelViewCompatible,
	ExtendedUsageKhr = ExtendedUsage,
	DisjointKhr = Disjoint,
	AliasKhr = Alias,
	FragmentDensityMapOffsetQcom = FragmentDensityMapOffsetExt,
}
