namespace Vulkan;

[System.Flags]
public enum PipelineDepthStencilStateCreateFlags : uint
{
	RasterizationOrderAttachmentDepthAccessExt = 0x00000001,
	RasterizationOrderAttachmentStencilAccessExt = 0x00000002,
	RasterizationOrderAttachmentDepthAccessArm = RasterizationOrderAttachmentDepthAccessExt,
	RasterizationOrderAttachmentStencilAccessArm = RasterizationOrderAttachmentStencilAccessExt,
}
