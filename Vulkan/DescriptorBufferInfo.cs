using System;

namespace Vulkan;

public readonly struct DescriptorBufferInfo 
{
	private readonly nint buffer;
	public readonly DeviceSize Offset;
	public readonly DeviceSize Range;

	public Buffer Buffer => throw new NotImplementedException(); // cannot get allocator and device params

	public DescriptorBufferInfo(Buffer buffer, DeviceSize offset, DeviceSize range) => (this.buffer, this.Offset, this.Range) = ((nint)buffer, offset, range);
}
