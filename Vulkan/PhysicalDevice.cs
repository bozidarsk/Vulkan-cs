using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class PhysicalDevice
{
	private readonly PhysicalDeviceHandle physicalDevice;

	internal PhysicalDeviceHandle Handle => physicalDevice;

	public PhysicalDeviceMemoryProperties MemoryProperties
	{
		get
		{
			vkGetPhysicalDeviceMemoryProperties(physicalDevice, out PhysicalDeviceMemoryProperties properties);
			return properties;

			[DllImport(VK_LIB)] static extern void vkGetPhysicalDeviceMemoryProperties(PhysicalDeviceHandle physicalDevice, out PhysicalDeviceMemoryProperties properties);
		}
	}

	public PhysicalDeviceProperties Properties
	{
		get
		{
			vkGetPhysicalDeviceProperties(physicalDevice, out PhysicalDeviceProperties properties);
			return properties;

			[DllImport(VK_LIB)] static extern void vkGetPhysicalDeviceProperties(PhysicalDeviceHandle physicalDevice, out PhysicalDeviceProperties properties);
		}
	}

	public PhysicalDeviceFeatures Features
	{
		get
		{
			vkGetPhysicalDeviceFeatures(physicalDevice, out PhysicalDeviceFeaturesStruct features);
			return features;

			[DllImport(VK_LIB)] static extern void vkGetPhysicalDeviceFeatures(PhysicalDeviceHandle physicalDevice, out PhysicalDeviceFeaturesStruct features);
		}
	}

	public unsafe QueueFamilyProperties[] QueueFamilyProperties
	{
		get
		{
			vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, out uint count, ref Unsafe.AsRef<QueueFamilyProperties>(default));
			var queueFamilyProperties = new QueueFamilyProperties[count];

			vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, out count, ref MemoryMarshal.GetArrayDataReference(queueFamilyProperties));

			return queueFamilyProperties;

			[DllImport(VK_LIB)] static extern void vkGetPhysicalDeviceQueueFamilyProperties(PhysicalDeviceHandle physicalDevice, out uint count, ref QueueFamilyProperties pQueueFamilyProperties);
		}
	}

	public FormatProperties GetFormatProperties(Format format)
	{
		vkGetPhysicalDeviceFormatProperties(physicalDevice, format, out FormatProperties properties);
		return properties;

		[DllImport(VK_LIB)] static extern void vkGetPhysicalDeviceFormatProperties(PhysicalDeviceHandle physicalDevice, Format format, out FormatProperties properties);
	}

	public unsafe ExtensionProperties[] GetExtensionProperties(string? layerName)
	{
		Result result;

		result = vkEnumerateDeviceExtensionProperties(physicalDevice, layerName, out uint count, ref Unsafe.AsRef<ExtensionProperties>(default));
		if (result != Result.Success) throw new VulkanException(result);

		var properties = new ExtensionProperties[count];

		result = vkEnumerateDeviceExtensionProperties(physicalDevice, layerName, out count, ref MemoryMarshal.GetArrayDataReference(properties));
		if (result != Result.Success) throw new VulkanException(result);

		return properties;

		[DllImport(VK_LIB)] static extern Result vkEnumerateDeviceExtensionProperties(PhysicalDeviceHandle physicalDevice, string? layerName, out uint count, ref ExtensionProperties pProperties);
	}

	internal PhysicalDevice(PhysicalDeviceHandle physicalDevice) => this.physicalDevice = physicalDevice;
}
