namespace Vulkan;

[System.Flags]
public enum ImageUsage : uint
{
	TransferSrc = 0x00000001,
	TransferDst = 0x00000002,
	Sampled = 0x00000004,
	Storage = 0x00000008,
	ColorAttachment = 0x00000010,
	DepthStencilAttachment = 0x00000020,
	TransientAttachment = 0x00000040,
	InputAttachment = 0x00000080,
	HostTransfer = 0x00400000,
	VideoDecodeDst = 0x00000400,
	VideoDecodeSrc = 0x00000800,
	VideoDecodeDpb = 0x00001000,
	FragmentDensityMap = 0x00000200,
	FragmentShadingRateAttachment = 0x00000100,
	VideoEncodeDst = 0x00002000,
	VideoEncodeSrc = 0x00004000,
	VideoEncodeDpb = 0x00008000,
	AttachmentFeedbackLoopBitExt = 0x00080000,
	InvocationMaskBitHuawei = 0x00040000,
	SampleWeightBitQcom = 0x00100000,
	SampleBlockMatchBitQcom = 0x00200000,
	TensorAliasingBitArm = 0x00800000,
	TileMemoryBitQcom = 0x08000000,
	VideoEncodeQuantizationDeltaMap = 0x02000000,
	VideoEncodeEmphasisMap = 0x04000000,
	ShadingRateImageBitNv = FragmentShadingRateAttachment,
	HostTransferBitExt = HostTransfer,
}
