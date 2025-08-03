using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct DeviceCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly DeviceCreateFlags Flags;
	private readonly uint queueCreateInfoCount;
	private readonly Handle<DeviceQueueCreateInfo> queueCreateInfos;
	private readonly uint enabledLayerCount;
	private readonly Handle<cstring> enabledLayerNames;
	private readonly uint enabledExtensionCount;
	private readonly Handle<cstring> enabledExtensionNames;
	private readonly Handle<PhysicalDeviceFeaturesStruct> enabledFeatures;

	public string?[]? EnabledLayerNames => enabledLayerNames.ToArray(enabledLayerCount)?.Select(x => (string?)x).ToArray();
	public string?[]? EnabledExtensionNames => enabledExtensionNames.ToArray(enabledExtensionCount)?.Select(x => (string?)x).ToArray();

	public DeviceQueueCreateInfo[]? QueueCreateInfos => queueCreateInfos.ToArray(queueCreateInfoCount);
	public PhysicalDeviceFeatures EnabledFeatures => (PhysicalDeviceFeaturesStruct)enabledFeatures;

	public Device CreateDevice(PhysicalDevice physicalDevice, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateDevice(physicalDevice, in this, allocator, out DeviceHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetDevice(allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateDevice(PhysicalDevice physicalDevice, in DeviceCreateInfo createInfo, nint allocator, out DeviceHandle device);
	}

	public void Dispose() 
	{
		foreach (var x in enabledLayerNames.ToArray(enabledLayerCount) ?? [])
			x.Dispose();

		foreach (var x in enabledExtensionNames.ToArray(enabledExtensionCount) ?? [])
			x.Dispose();

		enabledLayerNames.Dispose();
		enabledExtensionNames.Dispose();
		enabledFeatures.Dispose();
		queueCreateInfos.Dispose();
	}

	public DeviceCreateInfo(StructureType type, nint next, DeviceCreateFlags flags, DeviceQueueCreateInfo[]? queueCreateInfos, string?[]? enabledLayerNames, string?[]? enabledExtensionNames, PhysicalDeviceFeatures enabledFeatures) 
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;

		this.queueCreateInfoCount = (uint)(queueCreateInfos?.Length ?? 0);
		this.queueCreateInfos = new(queueCreateInfos);

		this.enabledFeatures = new Handle<PhysicalDeviceFeaturesStruct>(enabledFeatures);

		this.enabledLayerCount = (uint)(enabledLayerNames?.Length ?? 0);
		this.enabledLayerNames = new(enabledLayerNames?.Select(x => (cstring)x).ToArray());

		this.enabledExtensionCount = (uint)(enabledExtensionNames?.Length ?? 0);
		this.enabledExtensionNames = new(enabledExtensionNames?.Select(x => (cstring)x).ToArray());
	}
}
