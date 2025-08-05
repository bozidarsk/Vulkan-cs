namespace Vulkan;

public readonly struct ImageSubresourceLayers 
{
	public readonly ImageAspect Aspect;
	public readonly uint MipLevel;
	public readonly uint BaseArrayLayer;
	public readonly uint LayerCount;

	public ImageSubresourceLayers(
		ImageAspect aspect,
		uint mipLevel,
		uint baseArrayLayer,
		uint layerCount
	)
	{
		this.Aspect = aspect;
		this.MipLevel = mipLevel;
		this.BaseArrayLayer = baseArrayLayer;
		this.LayerCount = layerCount;
	}
}
