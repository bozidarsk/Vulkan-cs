using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Surface : IDisposable
{
	private readonly SurfaceHandle surface;
	private readonly Instance instance;
	private readonly AllocationCallbacks? allocator;

	internal SurfaceHandle Handle => surface;

	public bool IsSupported(PhysicalDevice physicalDevice, uint queueFamilyIndex)
	{
		Result result = vkGetPhysicalDeviceSurfaceSupportKHR(physicalDevice.Handle, queueFamilyIndex, surface, out bool32 x);
		if (result != Result.Success) throw new VulkanException(result);

		return x;

		[DllImport(VK_LIB)] static extern Result vkGetPhysicalDeviceSurfaceSupportKHR(PhysicalDeviceHandle physicalDevice, uint queueFamilyIndex, SurfaceHandle surface, out bool32 supported);
	}

	public unsafe PresentMode[] GetSurfacePresentModes(PhysicalDevice physicalDevice)
	{
		Result result;

		result = vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice.Handle, surface, out uint count, ref Unsafe.AsRef<PresentMode>(default));
		if (result != Result.Success) throw new VulkanException(result);

		var modes = new PresentMode[count];

		result = vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice.Handle, surface, out count, ref MemoryMarshal.GetArrayDataReference(modes));
		if (result != Result.Success) throw new VulkanException(result);

		return modes;

		[DllImport(VK_LIB)] static extern Result vkGetPhysicalDeviceSurfacePresentModesKHR(PhysicalDeviceHandle physicalDevice, SurfaceHandle surface, out uint count, ref PresentMode pModes);
	}

	public unsafe SurfaceFormat[] GetSurfaceFormats(PhysicalDevice physicalDevice)
	{
		Result result;

		result = vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice.Handle, surface, out uint count, ref Unsafe.AsRef<SurfaceFormat>(default));
		if (result != Result.Success) throw new VulkanException(result);

		var formats = new SurfaceFormat[count];

		result = vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice.Handle, surface, out count, ref MemoryMarshal.GetArrayDataReference(formats));
		if (result != Result.Success) throw new VulkanException(result);

		return formats;

		[DllImport(VK_LIB)] static extern Result vkGetPhysicalDeviceSurfaceFormatsKHR(PhysicalDeviceHandle physicalDevice, SurfaceHandle surface, out uint count, ref SurfaceFormat pFormats);
	}

	public SurfaceCapabilities GetSurfaceCapabilities(PhysicalDevice physicalDevice)
	{
		Result result = vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice.Handle, surface, out SurfaceCapabilities capabilities);
		if (result != Result.Success) throw new VulkanException(result);

		return capabilities;

		[DllImport(VK_LIB)] static extern Result vkGetPhysicalDeviceSurfaceCapabilitiesKHR(PhysicalDeviceHandle physicalDevice, SurfaceHandle surface, out SurfaceCapabilities capabilities);
	}

	public void Dispose()
	{
		vkDestroySurfaceKHR(instance.Handle, surface, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroySurfaceKHR(InstanceHandle instance, SurfaceHandle surface, AllocationCallbacksHandle allocator);
	}

	internal Surface(SurfaceHandle surface, Instance instance, AllocationCallbacks? allocator) =>
		(this.surface, this.instance, this.allocator) = (surface, instance, allocator)
	;
}
