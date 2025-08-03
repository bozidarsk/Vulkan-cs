using System;

namespace Vulkan;

public readonly struct DescriptorBufferInfo 
{
	private readonly BufferHandle buffer;
	public readonly DeviceSize Offset;
	public readonly DeviceSize Range;

	public Buffer Buffer => throw new NotImplementedException(); // cannot get allocator and device params

	public DescriptorBufferInfo(Buffer buffer, DeviceSize offset, DeviceSize range) => (this.buffer, this.Offset, this.Range) = (buffer.Handle, offset, range);
}
