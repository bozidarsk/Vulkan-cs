using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct ImageCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly ImageCreateFlags Flags;
	public readonly ImageType ImageType;
	public readonly Format Format;
	public readonly Extent3D Extent;
	public readonly uint MipLevels;
	public readonly uint ArrayLayers;
	public readonly SampleCount Samples;
	public readonly ImageTiling Tiling;
	public readonly ImageUsage Usage;
	public readonly SharingMode SharingMode;
	private readonly uint queueFamilyIndexCount;
	private readonly Handle<uint> queueFamilyIndices;
	public readonly ImageLayout InitialLayout;

	public uint[]? QueueFamilyIndices => queueFamilyIndices.ToArray(queueFamilyIndexCount);

	public Image CreateImage(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateImage(device.Handle, in this, allocator, out ImageHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetImage();

		[DllImport(VK_LIB)] static extern Result vkCreateImage(DeviceHandle device, in ImageCreateInfo createInfo, nint allocator, out ImageHandle image);
	}

	public void Dispose() 
	{
		queueFamilyIndices.Dispose();
	}

	public ImageCreateInfo(
		StructureType type,
		nint next,
		ImageCreateFlags flags,
		ImageType imageType,
		Format format,
		Extent3D extent,
		uint mipLevels,
		uint arrayLayers,
		SampleCount samples,
		ImageTiling tiling,
		ImageUsage usage,
		SharingMode sharingMode,
		uint[]? queueFamilyIndices,
		ImageLayout initialLayout
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.ImageType = imageType;
		this.Format = format;
		this.Extent = extent;
		this.MipLevels = mipLevels;
		this.ArrayLayers = arrayLayers;
		this.Samples = samples;
		this.Tiling = tiling;
		this.Usage = usage;
		this.SharingMode = sharingMode;
		this.InitialLayout = initialLayout;

		this.queueFamilyIndexCount = (uint)(queueFamilyIndices?.Length ?? 0);
		this.queueFamilyIndices = new(queueFamilyIndices);
	}
}
