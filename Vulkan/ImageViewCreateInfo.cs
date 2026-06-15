using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct ImageViewCreateInfo
{
	public readonly StructureType Type = StructureType.ImageViewCreateInfo;
	public readonly nint Next;
	public readonly ImageViewCreateFlags Flags;
	private readonly ImageHandle image;
	public readonly ImageViewType ViewType;
	public readonly Format Format;
	public readonly ComponentMapping Components;
	public readonly ImageSubresourceRange SubresourceRange;

	public Image Image => throw new NotImplementedException(); // cannot get allocator and device params

	public ImageView CreateImageView(Device device, AllocationCallbacks? allocator)
	{
		Result result = vkCreateImageView(device.Handle, in this, allocator?.Handle ?? default, out ImageViewHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetImageView(device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateImageView(DeviceHandle device, in ImageViewCreateInfo createInfo, AllocationCallbacksHandle allocator, out ImageViewHandle imageView);
	}

	public ImageViewCreateInfo(
		nint next,
		ImageViewCreateFlags flags,
		Image image,
		ImageViewType viewType,
		Format format,
		ComponentMapping components,
		ImageSubresourceRange subresourceRange
	)
	{
		this.Next = next;
		this.Flags = flags;
		this.image = image.Handle;
		this.ViewType = viewType;
		this.Format = format;
		this.Components = components;
		this.SubresourceRange = subresourceRange;
	}
}
