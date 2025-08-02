using System;

namespace Vulkan;

public readonly struct DescriptorImageInfo 
{
	private readonly nint sampler;
	private readonly nint imageView;
	public readonly ImageLayout ImageLayout;

	public Sampler Sampler => throw new NotImplementedException(); // cannot get allocator and device params
	public ImageView ImageView => throw new NotImplementedException(); // cannot get allocator and device params

	public DescriptorImageInfo(Sampler sampler, ImageView imageView, ImageLayout imageLayout) => 
		(this.sampler, this.imageView, this.ImageLayout) = ((nint)sampler, (nint)imageView, imageLayout)
	;
}
