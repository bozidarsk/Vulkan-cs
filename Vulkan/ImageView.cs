using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class ImageView : IDisposable
{
	private readonly Device device;
	private readonly nint imageview;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (ImageView x) => x.imageview;

	public void Dispose() 
	{
		vkDestroyImageView((nint)device, imageview, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyImageView(nint device, nint imageview, nint allocator);
	}

	private ImageView(Device device, nint imageview) => (this.device, this.imageview) = (device, imageview);
	internal ImageView(Device device, nint imageview, Handle<AllocationCallbacks> allocator) : this(device, imageview) => this.allocator = allocator;
}
