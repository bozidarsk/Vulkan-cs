namespace Vulkan;

[System.Flags]
public enum SubpassDescriptionFlags : uint
{
	PerViewAttributesNvx = 0x00000001,
	PerViewPositionXOnlyNvx = 0x00000002,
	FragmentRegionQcom = 0x00000004,
	ShaderResolveQcom = 0x00000008,
	TileShadingApronQcom = 0x00000100,
	RasterizationOrderAttachmentColorAccessExt = 0x00000010,
	RasterizationOrderAttachmentDepthAccessExt = 0x00000020,
	RasterizationOrderAttachmentStencilAccessExt = 0x00000040,
	EnableLegacyDitheringExt = 0x00000080,
	RasterizationOrderAttachmentColorAccessArm = RasterizationOrderAttachmentColorAccessExt,
	RasterizationOrderAttachmentDepthAccessArm = RasterizationOrderAttachmentDepthAccessExt,
	RasterizationOrderAttachmentStencilAccessArm = RasterizationOrderAttachmentStencilAccessExt,
}
