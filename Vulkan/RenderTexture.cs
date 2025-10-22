#pragma warning disable CS8618

using System;

namespace Vulkan;

public class RenderTexture : IDisposable
{
	protected Extent2D extent;
	protected Format depthFormat, imageFormat;

	protected Framebuffer framebuffer;

	protected Image depthImage;
	protected ImageView depthImageView;
	protected DeviceMemory depthImageMemory;

	protected Image image;
	protected ImageView imageView;
	protected DeviceMemory imageMemory;
	protected Sampler imageSampler;

	public Extent2D Extent => extent;
	public Framebuffer Framebuffer => framebuffer;

	public Image Image => image;
	public Format ImageFormat => imageFormat;
	public ImageView ImageView => imageView;
	public Sampler Sampler => imageSampler;

	protected virtual void InitializeDepthImage(Program vk) 
	{
		depthFormat = vk.FindSupportedFormat(
			[ Format.D32SFloat, Format.D32SFloatS8UInt, Format.D24UNormS8UInt ],
			ImageTiling.Optimal,
			FormatFeatures.DepthStencilAttachment
		);

		using var imageCreateInfo = new ImageCreateInfo(
			type: StructureType.ImageCreateInfo,
			next: default,
			flags: default,
			imageType: ImageType.Generic2D,
			format: depthFormat,
			extent: new(extent.Width, extent.Height, 1),
			mipLevels: 1,
			arrayLayers: 1,
			samples: SampleCount.Bit1,
			tiling: ImageTiling.Optimal,
			usage: ImageUsage.DepthStencilAttachment,
			sharingMode: SharingMode.Exclusive,
			queueFamilyIndices: null,
			initialLayout: ImageLayout.Undefined
		);

		depthImage = imageCreateInfo.CreateImage(vk.Device, vk.Allocator);

		var memoryRequirements = depthImage.MemoryRequirements;
		var allocateInfo = new MemoryAllocateInfo(
			type: StructureType.MemoryAllocateInfo,
			next: default,
			allocationSize: memoryRequirements.Size,
			memoryTypeIndex: vk.FindMemoryType(memoryRequirements.MemoryType, MemoryProperty.DeviceLocal)
		);

		depthImageMemory = allocateInfo.CreateDeviceMemory(vk.Device, vk.Allocator);
		depthImageMemory.Bind(depthImage);

		var imageViewCreateInfo = new ImageViewCreateInfo(
			type: StructureType.ImageViewCreateInfo,
			next: default,
			flags: default,
			image: depthImage,
			viewType: ImageViewType.Generic2D,
			format: depthFormat,
			components: default,
			subresourceRange: new(
				aspect: ImageAspect.Depth,
				baseMipLevel: 0,
				levelCount: 1,
				baseArrayLayer: 0,
				layerCount: 1
			)
		);

		depthImageView = imageViewCreateInfo.CreateImageView(vk.Device, vk.Allocator);
	}

	protected virtual void InitializeFramebuffer(Program vk) 
	{
		using var framebufferCreateInfo = new FramebufferCreateInfo(
			type: StructureType.FramebufferCreateInfo,
			next: default,
			flags: default,
			renderPass: vk.RenderPass,
			attachments: [ imageView, depthImageView ],
			width: extent.Width,
			height: extent.Height,
			layers: 1
		);

		framebuffer = framebufferCreateInfo.CreateFramebuffer(vk.Device, vk.Allocator);
	}

	public void Dispose() 
	{
		framebuffer.Dispose();
		depthImage.Dispose();
		depthImageView.Dispose();
		depthImageMemory.Dispose();
		image.Dispose();
		imageView.Dispose();
		imageMemory.Dispose();
		imageSampler.Dispose();
	}

	public RenderTexture(Program vk, Extent2D extent, Format imageFormat) 
	{
		this.extent = extent;
		this.imageFormat = imageFormat;

		vk.CreateImage((int)extent.Width, (int)extent.Height, ImageType.Generic2D, ImageUsage.ColorAttachment | ImageUsage.Sampled, imageFormat, out this.image, out this.imageMemory);
		vk.CreateImageView(this.image, imageFormat, out this.imageView);
		vk.CreateSampler(out this.imageSampler);

		InitializeDepthImage(vk);
		InitializeFramebuffer(vk);

		vk.TransitionImageLayout(this.Image, ImageLayout.Undefined, ImageLayout.ShaderReadOnlyOptimal);
	}
}
