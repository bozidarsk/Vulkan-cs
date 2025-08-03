namespace Vulkan;

public sealed class Image 
{
	private readonly ImageHandle image;

	internal ImageHandle Handle => image;

	internal Image(ImageHandle image) => 
		this.image = image
	;
}
