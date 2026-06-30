using System;

namespace Vulkan;

public readonly struct BufferMemoryBarrier2
{
	public readonly StructureType Type = StructureType.BufferMemoryBarrier2;
	public readonly nint Next;
	public readonly PipelineStage2 SrcStage;
	public readonly Access2 SrcAccess;
	public readonly PipelineStage2 DstStage;
	public readonly Access2 DstAccess;
	public readonly uint SrcQueueFamilyIndex;
	public readonly uint DstQueueFamilyIndex;
	private readonly BufferHandle buffer;
	public readonly DeviceSize Offset;
	public readonly DeviceSize Size;

	public Buffer Buffer => throw new NotImplementedException(); // cannot get allocator and device params

	public BufferMemoryBarrier2(
		nint next,
		PipelineStage2 srcStage,
		Access2 srcAccess,
		PipelineStage2 dstStage,
		Access2 dstAccess,
		uint srcQueueFamilyIndex,
		uint dstQueueFamilyIndex,
		Buffer buffer,
		DeviceSize offset,
		DeviceSize size
	)
	{
		this.Next = next;
		this.SrcStage = srcStage;
		this.SrcAccess = srcAccess;
		this.DstStage = dstStage;
		this.DstAccess = dstAccess;
		this.SrcQueueFamilyIndex = srcQueueFamilyIndex;
		this.DstQueueFamilyIndex = dstQueueFamilyIndex;
		this.buffer = buffer.Handle;
		this.Offset = offset;
		this.Size = size;
	}
}
