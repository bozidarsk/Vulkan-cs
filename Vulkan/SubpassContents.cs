namespace Vulkan;

public enum SubpassContents : uint
{
	Inline = 0,
	SecondaryCommandBuffers = 1,
	InlineAndSecondaryCommandBuffersKhr = 1000451000,
	InlineAndSecondaryCommandBuffersExt = InlineAndSecondaryCommandBuffersKhr,
}
