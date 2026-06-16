using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct BufferCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.BufferCreateInfo;
	public readonly nint Next;
	public readonly BufferCreateFlags Flags;
	public readonly DeviceSize Size;
	public readonly BufferUsage Usage;
	public readonly SharingMode SharingMode;
	private readonly uint queueFamilyIndexCount;
	private readonly Handle<uint> queueFamilyIndices;

	public uint[]? QueueFamilyIndices => queueFamilyIndices.ToArray(queueFamilyIndexCount);

	public Buffer CreateBuffer(Device device, AllocationCallbacks? allocator)
	{
		Result result = vkCreateBuffer(device.Handle, in this, allocator?.Handle ?? default, out BufferHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(handle, device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateBuffer(DeviceHandle device, in BufferCreateInfo createInfo, AllocationCallbacksHandle allocator, out BufferHandle buffer);
	}

	public void Dispose()
	{
		queueFamilyIndices.Dispose();
	}

	public BufferCreateInfo(
		nint next,
		BufferCreateFlags flags,
		DeviceSize size,
		BufferUsage usage,
		SharingMode sharingMode,
		uint[]? queueFamilyIndices
	)
	{
		this.Next = next;
		this.Flags = flags;
		this.Size = size;
		this.Usage = usage;
		this.SharingMode = sharingMode;

		this.queueFamilyIndexCount = (uint)(queueFamilyIndices?.Length ?? 0);
		this.queueFamilyIndices = new(queueFamilyIndices);
	}
}
