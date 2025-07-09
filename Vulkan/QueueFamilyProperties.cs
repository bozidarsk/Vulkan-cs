namespace Vulkan;

public readonly struct QueueFamilyProperties 
{
	public readonly QueueFlags QueueFlags;
	public readonly uint QueueCount;
	public readonly uint TimestampValidBits;
	public readonly Extent3D MinImageTransferGranularity;
}
