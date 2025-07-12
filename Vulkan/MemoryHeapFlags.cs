namespace Vulkan;

[System.Flags]
public enum MemoryHeapFlags : uint
{
	DeviceLocal = 0x00000001,
	MultiInstance = 0x00000002,
	TileMemoryQcom = 0x00000008,
	MultiInstanceKhr = MultiInstance,
}
