using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct SwapchainCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly SwapchainCreateFlags Flags;
	public readonly Surface Surface;
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
	private readonly nint oldSwapchain;

	public Swapchain OldSwapchain => throw new NotImplementedException(); // cannot get device and allocator params

	public uint[]? QueueFamilyIndices => queueFamilyIndices.ToArray(queueFamilyIndexCount);

	public Swapchain CreateSwapchain(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateSwapchainKHR((nint)device, in this, allocator, out nint swapchainHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, swapchainHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateSwapchainKHR(nint device, in SwapchainCreateInfo createInfo, nint allocator, out nint swapchain);
	}

	public void Dispose() 
	{
		queueFamilyIndices.Dispose();
	}

	public SwapchainCreateInfo(
		StructureType type,
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
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.Surface = surface;
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
		this.oldSwapchain = (oldSwapchain != null) ? (nint)oldSwapchain : default;

		this.queueFamilyIndexCount = (uint)(queueFamilyIndices?.Length ?? 0);
		this.queueFamilyIndices = new(queueFamilyIndices);
	}
}
