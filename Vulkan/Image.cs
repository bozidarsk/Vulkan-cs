using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Image : IDisposable
{
	private readonly ImageHandle image;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal ImageHandle Handle => image;

	public MemoryRequirements MemoryRequirements
	{
		get
		{
			vkGetImageMemoryRequirements(device.Handle, image, out MemoryRequirements requirements);
			return requirements;

			[DllImport(VK_LIB)] static extern void vkGetImageMemoryRequirements(DeviceHandle device, ImageHandle image, out MemoryRequirements requirements);
		}
	}

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.Image,
				objectHandle: (ulong)(nint)image,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroyImage(device.Handle, image, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyImage(DeviceHandle device, ImageHandle image, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => image.ToString();

	internal Image(ImageHandle image, Device device, AllocationCallbacks? allocator) =>
		(this.image, this.device, this.allocator) = (image, device, allocator)
	;
}
