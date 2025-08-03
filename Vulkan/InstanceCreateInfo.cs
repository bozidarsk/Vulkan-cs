using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct InstanceCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly InstanceCreateFlags Flags;
	private readonly Handle<ApplicationInfo> applicationInfo;
	private readonly uint enabledLayerCount;
	private readonly Handle<cstring> enabledLayerNames;
	private readonly uint enabledExtensionCount;
	private readonly Handle<cstring> enabledExtensionNames;

	public readonly ApplicationInfo ApplicationInfo => applicationInfo;

	public string?[]? EnabledLayerNames => enabledLayerNames.ToArray(enabledLayerCount)?.Select(x => (string?)x).ToArray();
	public string?[]? EnabledExtensionNames => enabledExtensionNames.ToArray(enabledExtensionCount)?.Select(x => (string?)x).ToArray();

	public static unsafe LayerProperties[] GetLayerProperties() 
	{
		Result result;

		result = vkEnumerateInstanceLayerProperties(out uint count, ref Unsafe.AsRef<LayerProperties>(default));
		if (result != Result.Success) throw new VulkanException(result);

		var properties = new LayerProperties[count];

		result = vkEnumerateInstanceLayerProperties(out count, ref MemoryMarshal.GetArrayDataReference(properties));
		if (result != Result.Success) throw new VulkanException(result);

		return properties;

		[DllImport(VK_LIB)] static extern Result vkEnumerateInstanceLayerProperties(out uint count, ref LayerProperties pProperties);
	}

	public static unsafe ExtensionProperties[] GetExtensionProperties(string? layerName) 
	{
		Result result;

		result = vkEnumerateInstanceExtensionProperties(layerName, out uint count, ref Unsafe.AsRef<ExtensionProperties>(default));
		if (result != Result.Success) throw new VulkanException(result);

		var extensions = new ExtensionProperties[count];

		result = result = vkEnumerateInstanceExtensionProperties(layerName, out count, ref MemoryMarshal.GetArrayDataReference(extensions));
		if (result != Result.Success) throw new VulkanException(result);

		return extensions;

		[DllImport(VK_LIB)] static extern Result vkEnumerateInstanceExtensionProperties(string? layerName, out uint count, ref ExtensionProperties pExtensions);
	}

	public Instance CreateInstance(Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateInstance(in this, allocator, out InstanceHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetInstance(allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateInstance(in InstanceCreateInfo info, nint allocator, out InstanceHandle instance);
	}

	public void Dispose() 
	{
		foreach (var x in enabledLayerNames.ToArray(enabledLayerCount) ?? [])
			x.Dispose();

		foreach (var x in enabledExtensionNames.ToArray(enabledExtensionCount) ?? [])
			x.Dispose();

		applicationInfo.Dispose();
		enabledLayerNames.Dispose();
		enabledExtensionNames.Dispose();
	}

	public InstanceCreateInfo(StructureType type, nint next, InstanceCreateFlags flags, ApplicationInfo applicationInfo, string?[]? enabledLayerNames, string?[]? enabledExtensionNames) 
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;

		this.applicationInfo = new(applicationInfo);

		this.enabledLayerCount = (uint)(enabledLayerNames?.Length ?? 0);
		this.enabledLayerNames = new(enabledLayerNames?.Select(x => (cstring)x).ToArray());

		this.enabledExtensionCount = (uint)(enabledExtensionNames?.Length ?? 0);
		this.enabledExtensionNames = new(enabledExtensionNames?.Select(x => (cstring)x).ToArray());
	}
}
