using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct FramebufferCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly FramebufferCreateFlags Flags;
	private readonly nint renderPass;
	private readonly uint attachmentCount;
	private readonly Handle<nint> attachments;
	public readonly uint Width;
	public readonly uint Height;
	public readonly uint Layers;

	public RenderPass RenderPass => throw new NotImplementedException(); // cannot get allocator and device params
	public ImageView[]? Attachments => throw new NotImplementedException(); // cannot get allocator and device params

	public Framebuffer CreateFramebuffer(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateFramebuffer((nint)device, in this, allocator, out nint framebufferHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, framebufferHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateFramebuffer(nint device, in FramebufferCreateInfo createInfo, nint allocator, out nint framebuffer);
	}

	public void Dispose() 
	{
		attachments.Dispose();
	}

	public FramebufferCreateInfo(
		StructureType type,
		nint next,
		FramebufferCreateFlags flags,
		RenderPass renderPass,
		ImageView[]? attachments,
		uint width,
		uint height,
		uint layers
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.renderPass = (nint)renderPass;
		this.Width = width;
		this.Height = height;
		this.Layers = layers;

		this.attachmentCount = (uint)(attachments?.Length ?? 0);
		this.attachments = new(attachments?.Select(x => (nint)x).ToArray());
	}
}
