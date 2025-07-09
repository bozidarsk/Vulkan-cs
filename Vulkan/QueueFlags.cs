namespace Vulkan;

[System.Flags]
public enum QueueFlags : uint
{
	Graphics = 0x00000001,
	Compute = 0x00000002,
	Transfer = 0x00000004,
	SparseBinding = 0x00000008,
	Protected = 0x00000010,
	VideoDecode = 0x00000020,
	VideoEncode = 0x00000040,
	OpticalFlowNv = 0x00000100,
	DataGraphArm = 0x00000400,
}
