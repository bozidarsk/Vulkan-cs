namespace Vulkan;

public readonly struct ImageSubresourceRange 
{
	public readonly ImageAspectFlags AspectMask;
	public readonly uint BaseMipLevel;
	public readonly uint LevelCount;
	public readonly uint BaseArrayLayer;
	public readonly uint LayerCount;

	public ImageSubresourceRange(
		ImageAspectFlags aspectMask,
		uint baseMipLevel,
		uint levelCount,
		uint baseArrayLayer,
		uint layerCount
	) => (this.AspectMask, this.BaseMipLevel, this.LevelCount, this.BaseArrayLayer, this.LayerCount) = (aspectMask, baseMipLevel, levelCount, baseArrayLayer, layerCount);
}
