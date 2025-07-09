namespace Vulkan;

public enum PresentMode : uint
{
	Immediate = 0,
	Mailbox = 1,
	Fifo = 2,
	FifoRelaxed = 3,
	SharedDemandRefresh = 1000111000,
	SharedContinuousRefresh = 1000111001,
	FifoLatestReadyExt = 1000361000,
}
