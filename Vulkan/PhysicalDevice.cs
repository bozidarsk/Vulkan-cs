using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct PhysicalDevice 
{
	private readonly nint handle;

	public PhysicalDeviceMemoryProperties MemoryProperties 
	{
		get 
		{
			vkGetPhysicalDeviceMemoryProperties(this, out PhysicalDeviceMemoryProperties properties);
			return properties;

			[DllImport(VK_LIB)] static extern void vkGetPhysicalDeviceMemoryProperties(PhysicalDevice physicalDevice, out PhysicalDeviceMemoryProperties properties);
		}
	}

	public PhysicalDeviceProperties Properties 
	{
		get 
		{
			vkGetPhysicalDeviceProperties(this, out PhysicalDeviceProperties properties);
			return properties;

			[DllImport(VK_LIB)] static extern void vkGetPhysicalDeviceProperties(PhysicalDevice physicalDevice, out PhysicalDeviceProperties properties);
		}
	}

	public PhysicalDeviceFeatures Features 
	{
		get 
		{
			vkGetPhysicalDeviceFeatures(this, out PhysicalDeviceFeaturesStruct features);
			return features;

			[DllImport(VK_LIB)] static extern void vkGetPhysicalDeviceFeatures(PhysicalDevice physicalDevice, out PhysicalDeviceFeaturesStruct features);
		}
	}

	public QueueFamilyProperties[] QueueFamilyProperties 
	{
		get 
		{
			vkGetPhysicalDeviceQueueFamilyProperties(this, out uint count, default);
			var queueFamilyProperties = new QueueFamilyProperties[count];

			vkGetPhysicalDeviceQueueFamilyProperties(this, out count, queueFamilyProperties.AsPointer());

			return queueFamilyProperties;

			[DllImport(VK_LIB)] static extern void vkGetPhysicalDeviceQueueFamilyProperties(PhysicalDevice physicalDevice, out uint count, nint pQueueFamilyProperties);
		}
	}

	public ExtensionProperties[] GetExtensionProperties(string? layerName) 
	{
		Result result;

		result = vkEnumerateDeviceExtensionProperties(this, layerName, out uint count, default);
		if (result != Result.Success) throw new VulkanException(result);

		var properties = new ExtensionProperties[count];

		result = vkEnumerateDeviceExtensionProperties(this, layerName, out count, properties.AsPointer());
		if (result != Result.Success) throw new VulkanException(result);

		return properties;

		[DllImport(VK_LIB)] static extern Result vkEnumerateDeviceExtensionProperties(PhysicalDevice physicalDevice, string? layerName, out uint count, nint pProperties);
	}

	public static bool operator == (PhysicalDevice a, PhysicalDevice b) => a.handle == b.handle;
	public static bool operator != (PhysicalDevice a, PhysicalDevice b) => a.handle != b.handle;
	public override bool Equals(object? other) => (other is PhysicalDevice x) ? x.handle == handle : false;

	public static implicit operator nint (PhysicalDevice x) => x.handle;
	public static implicit operator PhysicalDevice (nint x) => new(x);

	public override string ToString() => handle.ToString();
	public override int GetHashCode() => handle.GetHashCode();

	private PhysicalDevice(nint handle) => this.handle = handle;
}

