using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct BufferCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly BufferCreateFlags Flags;
	public readonly DeviceSize Size;
	public readonly BufferUsage Usage;
	public readonly SharingMode SharingMode;
	private readonly uint queueFamilyIndexCount;
	private readonly Handle<uint> queueFamilyIndices;

	public uint[]? QueueFamilyIndices => queueFamilyIndices.ToArray(queueFamilyIndexCount);

	public Buffer CreateBuffer(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateBuffer((nint)device, in this, allocator, out nint bufferHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, bufferHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateBuffer(nint device, in BufferCreateInfo createInfo, nint allocator, out nint buffer);
	}

	public void Dispose() 
	{
		queueFamilyIndices.Dispose();
	}

	public BufferCreateInfo(
		StructureType type,
		nint next,
		BufferCreateFlags flags,
		DeviceSize size,
		BufferUsage usage,
		SharingMode sharingMode,
		uint[]? queueFamilyIndices
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.Size = size;
		this.Usage = usage;
		this.SharingMode = sharingMode;

		this.queueFamilyIndexCount = (uint)(queueFamilyIndices?.Length ?? 0);
		this.queueFamilyIndices = new(queueFamilyIndices);
	}
}
