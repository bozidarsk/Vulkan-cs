namespace Vulkan;

public enum QueryResultFlags : uint
{
	Bits64 = 0x00000001,
	Wait = 0x00000002,
	WithAvailability = 0x00000004,
	Partial = 0x00000008,
	WithStatusKhr = 0x00000010,
}
