namespace Vulkan;

public enum ResolveMode : uint
{
	None = 0,
	SampleZero = 0x00000001,
	Average = 0x00000002,
	Min = 0x00000004,
	Max = 0x00000008,
	ExternalFormatDownsampleAndroid = 0x00000010,
	CustomExt = 0x00000020,
	NoneKhr = None,
	SampleZeroKhr = SampleZero,
	AverageKhr = Average,
	MinKhr = Min,
	MaxKhr = Max,
}
