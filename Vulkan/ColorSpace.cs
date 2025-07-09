namespace Vulkan;

public enum ColorSpace : int
{
	SRGBNonlinear = 0,
	DisplayP3NonlinearExt = 1000104001,
	ExtendedSRGBLinearExt = 1000104002,
	DisplayP3LinearExt = 1000104003,
	DciP3NonlinearExt = 1000104004,
	Bt709LinearExt = 1000104005,
	Bt709NonlinearExt = 1000104006,
	Bt2020LinearExt = 1000104007,
	HDR10ST2084Ext = 1000104008,
	DolbyvisionExt = 1000104009,
	HDR10HLGExt = 1000104010,
	AdobeRGBLinearExt = 1000104011,
	AdobeRGBNonlinearExt = 1000104012,
	PassThroughExt = 1000104013,
	ExtendedSRGBNonlinearExt = 1000104014,
	DisplayNativeAmd = 1000213000,
	RGBNonlinear = SRGBNonlinear,
	DciP3LinearExt = DisplayP3LinearExt,
}
