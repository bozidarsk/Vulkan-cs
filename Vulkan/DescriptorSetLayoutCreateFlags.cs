namespace Vulkan;

[System.Flags]
public enum DescriptorSetLayoutCreateFlags : uint
{
	UpdateAfterBindPool = 0x00000002,
	PushDescriptor = 0x00000001,
	DescriptorBufferExt = 0x00000010,
	EmbeddedImmutableSamplersExt = 0x00000020,
	IndirectBindableNv = 0x00000080,
	HostOnlyPoolExt = 0x00000004,
	PerStageNv = 0x00000040,
	PushDescriptorKhr = PushDescriptor,
	UpdateAfterBindPoolExt = UpdateAfterBindPool,
	HostOnlyPoolValve = HostOnlyPoolExt,
}
