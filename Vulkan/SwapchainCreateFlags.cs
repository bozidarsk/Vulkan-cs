namespace Vulkan;

[System.Flags]
public enum SwapchainCreateFlags : uint
{
	SplitInstanceBindRegions = 0x00000001,
	Protected = 0x00000002,
	MutableFormat = 0x00000004,
	DeferredMemoryAllocation = 0x00000008,
	PresentId2 = 0x00000040,
	PresentWait2 = 0x00000080,
}
