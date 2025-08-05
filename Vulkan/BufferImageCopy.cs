namespace Vulkan;

public readonly struct BufferImageCopy 
{
	public readonly DeviceSize BufferOffset;
	public readonly uint BufferRowLength;
	public readonly uint BufferImageHeight;
	public readonly ImageSubresourceLayers ImageSubresource;
	public readonly Offset3D ImageOffset;
	public readonly Extent3D ImageExtent;

	public BufferImageCopy(
		DeviceSize bufferOffset,
		uint bufferRowLength,
		uint bufferImageHeight,
		ImageSubresourceLayers imageSubresource,
		Offset3D imageOffset,
		Extent3D imageExtent
	)
	{
		this.BufferOffset = bufferOffset;
		this.BufferRowLength = bufferRowLength;
		this.BufferImageHeight = bufferImageHeight;
		this.ImageSubresource = imageSubresource;
		this.ImageOffset = imageOffset;
		this.ImageExtent = imageExtent;
	}
}
