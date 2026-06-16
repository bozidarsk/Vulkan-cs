using System;
using System.Runtime.InteropServices;

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
