namespace Vulkan;

public readonly struct BufferCopy 
{
	public readonly DeviceSize SourceOffset;
	public readonly DeviceSize DestinationOffset;
	public readonly DeviceSize Size;

	public BufferCopy(DeviceSize sourceOffset, DeviceSize destinationOffset, DeviceSize size) => 
		(this.SourceOffset, this.DestinationOffset, this.Size) = (sourceOffset, destinationOffset, size)
	;
}
