using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct SwapchainCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.SwapchainCreateInfo;
	public readonly nint Next;
	public readonly SwapchainCreateFlags Flags;
	private readonly SurfaceHandle surface;
	public readonly uint MinImageCount;
	public readonly Format ImageFormat;
	public readonly ColorSpace ImageColorSpace;
	public readonly Extent2D ImageExtent;
	public readonly uint ImageArrayLayers;
	public readonly ImageUsage ImageUsage;
	public readonly SharingMode ImageSharingMode;
	private readonly uint queueFamilyIndexCount;
	private readonly Handle<uint> queueFamilyIndices;
	public readonly SurfaceTransformFlags PreTransform;
	public readonly CompositeAlphaFlags CompositeAlpha;
	public readonly PresentMode PresentMode;
	public readonly bool32 Clipped;
	private readonly SwapchainHandle oldSwapchain;

	public Surface Surface => throw new NotImplementedException(); // cannot get instance and allocator params
	public Swapchain OldSwapchain => throw new NotImplementedException(); // cannot get device and allocator params

	public uint[]? QueueFamilyIndices => queueFamilyIndices.ToArray(queueFamilyIndexCount);

	public Swapchain CreateSwapchain(Device device, AllocationCallbacks? allocator)
	{
		Result result = vkCreateSwapchainKHR(device.Handle, in this, allocator?.Handle ?? default, out SwapchainHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(handle, device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateSwapchainKHR(DeviceHandle device, in SwapchainCreateInfo createInfo, AllocationCallbacksHandle allocator, out SwapchainHandle swapchain);
	}

	public void Dispose()
	{
		queueFamilyIndices.Dispose();
	}

	public SwapchainCreateInfo(
		nint next,
		SwapchainCreateFlags flags,
		Surface surface,
		uint minImageCount,
		Format imageFormat,
		ColorSpace imageColorSpace,
		Extent2D imageExtent,
		uint imageArrayLayers,
		ImageUsage imageUsage,
		SharingMode imageSharingMode,
		uint[]? queueFamilyIndices,
		SurfaceTransformFlags preTransform,
		CompositeAlphaFlags compositeAlpha,
		PresentMode presentMode,
		bool clipped,
		Swapchain? oldSwapchain
	)
	{
		this.Next = next;
		this.Flags = flags;
		this.surface = surface.Handle;
		this.MinImageCount = minImageCount;
		this.ImageFormat = imageFormat;
		this.ImageColorSpace = imageColorSpace;
		this.ImageExtent = imageExtent;
		this.ImageArrayLayers = imageArrayLayers;
		this.ImageUsage = imageUsage;
		this.ImageSharingMode = imageSharingMode;
		this.PreTransform = preTransform;
		this.CompositeAlpha = compositeAlpha;
		this.PresentMode = presentMode;
		this.Clipped = clipped;
		this.oldSwapchain = (oldSwapchain != null) ? oldSwapchain.Handle : default;

		this.queueFamilyIndexCount = (uint)(queueFamilyIndices?.Length ?? 0);
		this.queueFamilyIndices = new(queueFamilyIndices);
	}
}
