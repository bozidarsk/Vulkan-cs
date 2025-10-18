using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Sampler : IDisposable
{
	private readonly SamplerHandle sampler;
	private readonly Device device;
	private readonly AllocationCallbacksHandle allocator;

	internal SamplerHandle Handle => sampler;

	public void Dispose() 
	{
		vkDestroySampler(device.Handle, sampler, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroySampler(DeviceHandle device, SamplerHandle sampler, AllocationCallbacksHandle allocator);
	}

	internal Sampler(SamplerHandle sampler, Device device, AllocationCallbacksHandle allocator) => 
		(this.sampler, this.device, this.allocator) = (sampler, device, allocator)
	;
}
