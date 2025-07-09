using static Vulkan.Constants;

namespace Vulkan;

public struct PhysicalDeviceProperties 
{
	public readonly uint ApiVersion;
	public readonly uint DriverVersion;
	public readonly uint VendorID;
	public readonly uint DeviceID;
	public readonly PhysicalDeviceType DeviceType;
	private unsafe fixed sbyte deviceName[VK_MAX_PHYSICAL_DEVICE_NAME_SIZE];
	public readonly System.Guid PipelineCacheUUID;
	public readonly PhysicalDeviceLimits Limits;
	public readonly PhysicalDeviceSparseProperties SparseProperties;

	public unsafe string DeviceName { get { fixed (sbyte* x = deviceName) return new(x); } }

	public override string ToString() => DeviceName;
}
