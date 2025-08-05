using System;

namespace Vulkan;

public readonly struct BufferMemoryBarrier 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly Access SrcAccess;
	public readonly Access DstAccess;
	public readonly uint SrcQueueFamilyIndex;
	public readonly uint DstQueueFamilyIndex;
	private readonly BufferHandle buffer;
	public readonly DeviceSize Offset;
	public readonly DeviceSize Size;

	public Buffer Buffer => throw new NotImplementedException(); // cannot get allocator and device params

	public BufferMemoryBarrier(
		StructureType type,
		nint next,
		Access srcAccess,
		Access dstAccess,
		uint srcQueueFamilyIndex,
		uint dstQueueFamilyIndex,
		Buffer buffer,
		DeviceSize offset,
		DeviceSize size
	)
	{
		this.Type = type;
		this.Next = next;
		this.SrcAccess = srcAccess;
		this.DstAccess = dstAccess;
		this.SrcQueueFamilyIndex = srcQueueFamilyIndex;
		this.DstQueueFamilyIndex = dstQueueFamilyIndex;
		this.buffer = buffer.Handle;
		this.Offset = offset;
		this.Size = size;
	}
}
