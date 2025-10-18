using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class ImageView : IDisposable
{
	private readonly ImageViewHandle imageView;
	private readonly Device device;
	private readonly AllocationCallbacksHandle allocator;

	internal ImageViewHandle Handle => imageView;

	public void Dispose() 
	{
		vkDestroyImageView(device.Handle, imageView, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyImageView(DeviceHandle device, ImageViewHandle imageView, AllocationCallbacksHandle allocator);
	}

	internal ImageView(ImageViewHandle imageView, Device device, AllocationCallbacksHandle allocator) => 
		(this.imageView, this.device, this.allocator) = (imageView, device, allocator)
	;
}
