using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct Surface 
{
	private readonly nint handle;

	public bool IsSupported(PhysicalDevice physicalDevice, uint queueFamilyIndex) 
	{
		Result result = vkGetPhysicalDeviceSurfaceSupportKHR((nint)physicalDevice, queueFamilyIndex, this, out bool32 x);
		if (result != Result.Success) throw new VulkanException(result);

		return x;

		[DllImport(VK_LIB)] static extern Result vkGetPhysicalDeviceSurfaceSupportKHR(PhysicalDevice physicalDevice, uint queueFamilyIndex, Surface surface, out bool32 supported);
	}

	public unsafe PresentMode[] GetSurfacePresentModes(PhysicalDevice physicalDevice) 
	{
		Result result;

		result = vkGetPhysicalDeviceSurfacePresentModesKHR((nint)physicalDevice, this, out uint count, ref Unsafe.AsRef<PresentMode>(default));
		if (result != Result.Success) throw new VulkanException(result);

		var modes = new PresentMode[count];

		result = vkGetPhysicalDeviceSurfacePresentModesKHR((nint)physicalDevice, this, out count, ref MemoryMarshal.GetArrayDataReference(modes));
		if (result != Result.Success) throw new VulkanException(result);

		return modes;

		[DllImport(VK_LIB)] static extern Result vkGetPhysicalDeviceSurfacePresentModesKHR(PhysicalDevice physicalDevice, Surface surface, out uint count, ref PresentMode pModes);
	}

	public unsafe SurfaceFormat[] GetSurfaceFormats(PhysicalDevice physicalDevice) 
	{
		Result result;

		result = vkGetPhysicalDeviceSurfaceFormatsKHR((nint)physicalDevice, this, out uint count, ref Unsafe.AsRef<SurfaceFormat>(default));
		if (result != Result.Success) throw new VulkanException(result);

		var formats = new SurfaceFormat[count];

		result = vkGetPhysicalDeviceSurfaceFormatsKHR((nint)physicalDevice, this, out count, ref MemoryMarshal.GetArrayDataReference(formats));
		if (result != Result.Success) throw new VulkanException(result);

		return formats;

		[DllImport(VK_LIB)] static extern Result vkGetPhysicalDeviceSurfaceFormatsKHR(PhysicalDevice physicalDevice, Surface surface, out uint count, ref SurfaceFormat pFormats);
	}

	public SurfaceCapabilities GetSurfaceCapabilities(PhysicalDevice physicalDevice) 
	{
		Result result = vkGetPhysicalDeviceSurfaceCapabilitiesKHR((nint)physicalDevice, this, out SurfaceCapabilities capabilities);
		if (result != Result.Success) throw new VulkanException(result);

		return capabilities;

		[DllImport(VK_LIB)] static extern Result vkGetPhysicalDeviceSurfaceCapabilitiesKHR(PhysicalDevice physicalDevice, Surface surface, out SurfaceCapabilities capabilities);
	}

	public static bool operator == (Surface a, Surface b) => a.handle == b.handle;
	public static bool operator != (Surface a, Surface b) => a.handle != b.handle;
	public override bool Equals(object? other) => (other is Surface x) ? x.handle == handle : false;

	public static implicit operator nint (Surface x) => x.handle;
	public static implicit operator Surface (nint x) => new(x);

	public override string ToString() => handle.ToString();
	public override int GetHashCode() => handle.GetHashCode();

	private Surface(nint handle) => this.handle = handle;
}
