namespace Vulkan;

public readonly struct ImageSubresourceRange 
{
	public readonly ImageAspect Aspect;
	public readonly uint BaseMipLevel;
	public readonly uint LevelCount;
	public readonly uint BaseArrayLayer;
	public readonly uint LayerCount;

	public ImageSubresourceRange(
		ImageAspect aspect,
		uint baseMipLevel,
		uint levelCount,
		uint baseArrayLayer,
		uint layerCount
	) => (this.Aspect, this.BaseMipLevel, this.LevelCount, this.BaseArrayLayer, this.LayerCount) = (aspect, baseMipLevel, levelCount, baseArrayLayer, layerCount);
}
