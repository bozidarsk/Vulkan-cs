using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct ImageViewCreateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly ImageViewCreateFlags Flags;
	public readonly Image Image;
	public readonly ImageViewType ViewType;
	public readonly Format Format;
	public readonly ComponentMapping Components;
	public readonly ImageSubresourceRange SubresourceRange;

	public ImageView CreateImageView(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateImageView((nint)device, in this, allocator, out nint imageViewHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, imageViewHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateImageView(nint device, in ImageViewCreateInfo createInfo, nint allocator, out nint imageView);
	}

	public ImageViewCreateInfo(
		StructureType type,
		nint next,
		ImageViewCreateFlags flags,
		Image image,
		ImageViewType viewType,
		Format format,
		ComponentMapping components,
		ImageSubresourceRange subresourceRange
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.Image = image;
		this.ViewType = viewType;
		this.Format = format;
		this.Components = components;
		this.SubresourceRange = subresourceRange;
	}
}
