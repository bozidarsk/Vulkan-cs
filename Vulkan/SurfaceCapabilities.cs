namespace Vulkan;

public readonly struct SurfaceCapabilities 
{
	public readonly uint MinImageCount;
	public readonly uint MaxImageCount;
	public readonly Extent2D CurrentExtent;
	public readonly Extent2D MinImageExtent;
	public readonly Extent2D MaxImageExtent;
	public readonly uint MaxImageArrayLayers;
	public readonly SurfaceTransformFlags SupportedTransforms;
	public readonly SurfaceTransformFlags CurrentTransform;
	public readonly CompositeAlphaFlags SupportedCompositeAlpha;
	public readonly ImageUsageFlags SupportedUsageFlags;
}
