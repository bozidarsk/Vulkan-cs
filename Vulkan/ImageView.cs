using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class ImageView : IDisposable
{
	private readonly ImageViewHandle imageView;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal ImageViewHandle Handle => imageView;

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.ImageView,
				objectHandle: (ulong)(nint)imageView,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroyImageView(device.Handle, imageView, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyImageView(DeviceHandle device, ImageViewHandle imageView, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => imageView.ToString();

	internal ImageView(ImageViewHandle imageView, Device device, AllocationCallbacks? allocator) =>
		(this.imageView, this.device, this.allocator) = (imageView, device, allocator)
	;
}
