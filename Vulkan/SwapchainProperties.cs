using System;
using System.Linq;

namespace Vulkan;

public sealed class SwapchainProperties 
{
	public SurfaceCapabilities Capabilities { get; }
	private SurfaceFormat[] formats { get; }
	private PresentMode[] presentModes { get; }

	public SurfaceFormat GetSurfaceFormat(Format format, ColorSpace colorSpace) 
	{
		var x = formats.Where(x => x.Format == format && x.ColorSpace == colorSpace);
		int count = x.Count();

		return count switch 
		{
			0 => throw new VulkanException($"Does not have SurfaceFormat with '{format}' and '{colorSpace}'."),
			1 => x.Single(),
			_ => throw new VulkanException($"Does not have SurfaceFormat with '{format}' and '{colorSpace}'.")
		};
	}

	public PresentMode GetPresentMode(PresentMode mode) 
	{
		var x = presentModes.Where(x => x == mode);
		int count = x.Count();

		return count switch 
		{
			0 => throw new VulkanException($"Does not have PresentMode '{mode}'."),
			1 => x.Single(),
			_ => throw new VulkanException($"Does not have PresentMode '{mode}'.")
		};
	}

	public Extent2D GetExtent(int width, int height) 
	{
		if (Capabilities.CurrentExtent.Width != uint.MaxValue)
			return Capabilities.CurrentExtent;

		return new(
			Math.Clamp((uint)width, Capabilities.MinImageExtent.Width, Capabilities.MaxImageExtent.Width),
			Math.Clamp((uint)height, Capabilities.MinImageExtent.Height, Capabilities.MaxImageExtent.Height)
		);
	}

	public SwapchainProperties(SurfaceCapabilities capabilities, SurfaceFormat[] formats, PresentMode[] presenModes) 
	{
		this.Capabilities = capabilities;
		this.formats = formats ?? throw new ArgumentNullException();
		this.presentModes = presenModes ?? throw new ArgumentNullException();
	}

	public SwapchainProperties(PhysicalDevice physicalDevice, Surface surface) 
	{
		this.Capabilities = surface.GetSurfaceCapabilities(physicalDevice);
		this.formats = surface.GetSurfaceFormats(physicalDevice);
		this.presentModes = surface.GetSurfacePresentModes(physicalDevice);
	}
}
