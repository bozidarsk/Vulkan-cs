using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Swapchain : IDisposable
{
	private readonly Device device;
	private readonly nint swapchain;
	private readonly Handle<AllocationCallbacks> allocator;

	public uint GetNextImage(Semaphore semaphore) 
	{
		Result result = vkAcquireNextImageKHR((nint)device, swapchain, ulong.MaxValue, (nint)semaphore, default, out uint imageIndex);
		if (result != Result.Success) throw new VulkanException(result);

		return imageIndex;

		[DllImport(VK_LIB)] static extern Result vkAcquireNextImageKHR(nint device, nint swapchain, ulong timeout, nint semaphore, nint fence, out uint imageIndex);
	}

	public Image[] GetImages() 
	{
		vkGetSwapchainImagesKHR((nint)device, swapchain, out uint count, default);
		Image[] images = new Image[count];

		Result result = vkGetSwapchainImagesKHR((nint)device, swapchain, out count, images.AsPointer());
		if (result != Result.Success) throw new VulkanException(result);

		return images;

		[DllImport(VK_LIB)] static extern Result vkGetSwapchainImagesKHR(nint device, nint swapchain, out uint count, nint pImages);
	}

	public static explicit operator nint (Swapchain x) => x.swapchain;

	public void Dispose() 
	{
		vkDestroySwapchainKHR((nint)device, swapchain, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroySwapchainKHR(nint device, nint swapchain, nint allocator);
	}

	private Swapchain(Device device, nint swapchain) => (this.device, this.swapchain) = (device, swapchain);
	internal Swapchain(Device device, nint swapchain, Handle<AllocationCallbacks> allocator) : this(device, swapchain) => this.allocator = allocator;
}
