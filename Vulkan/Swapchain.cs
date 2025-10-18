using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Swapchain : IDisposable
{
	private readonly SwapchainHandle swapchain;
	private readonly Device device;
	private readonly AllocationCallbacksHandle allocator;

	internal SwapchainHandle Handle => swapchain;

	public uint GetNextImage(Semaphore semaphore) 
	{
		Result result = vkAcquireNextImageKHR(device.Handle, swapchain, ulong.MaxValue, semaphore.Handle, default, out uint imageIndex);
		if (result != Result.Success) throw new VulkanException(result);

		return imageIndex;

		[DllImport(VK_LIB)] static extern Result vkAcquireNextImageKHR(DeviceHandle device, SwapchainHandle swapchain, ulong timeout, SemaphoreHandle semaphore, FenceHandle fence, out uint imageIndex);
	}

	public unsafe Image[] GetImages() 
	{
		vkGetSwapchainImagesKHR(device.Handle, swapchain, out uint count, ref Unsafe.AsRef<ImageHandle>(default));
		var images = new ImageHandle[count];

		Result result = vkGetSwapchainImagesKHR(device.Handle, swapchain, out count, ref MemoryMarshal.GetArrayDataReference(images));
		if (result != Result.Success) throw new VulkanException(result);

		return images.Select(x => x.GetImage(device, allocator)).ToArray();

		[DllImport(VK_LIB)] static extern Result vkGetSwapchainImagesKHR(DeviceHandle device, SwapchainHandle swapchain, out uint count, ref ImageHandle pImages);
	}

	public void Dispose() 
	{
		vkDestroySwapchainKHR(device.Handle, swapchain, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroySwapchainKHR(DeviceHandle device, SwapchainHandle swapchain, AllocationCallbacksHandle allocator);
	}

	internal Swapchain(SwapchainHandle swapchain, Device device, AllocationCallbacksHandle allocator) => 
		(this.swapchain, this.device, this.allocator) = (swapchain, device, allocator)
	;
}
