namespace Vulkan;

public enum DescriptorType : uint
{
	Sampler = 0,
	CombinedImageSampler = 1,
	SampledImage = 2,
	StorageImage = 3,
	UniformTexelBuffer = 4,
	StorageTexelBuffer = 5,
	UniformBuffer = 6,
	StorageBuffer = 7,
	UniformBufferDynamic = 8,
	StorageBufferDynamic = 9,
	InputAttachment = 10,
	InlineUniformBlock = 1000138000,
	AccelerationStructureKhr = 1000150000,
	AccelerationStructureNv = 1000165000,
	SampleWeightImageQcom = 1000440000,
	BlockMatchImageQcom = 1000440001,
	TensorArm = 1000460000,
	MutableExt = 1000351000,
	PartitionedAccelerationStructureNv = 1000570000,
	InlineUniformBlockExt = InlineUniformBlock,
	MutableValve = MutableExt,
}
