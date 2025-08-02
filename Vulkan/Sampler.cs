using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Sampler : IDisposable
{
	private readonly Device device;
	private readonly nint sampler;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (Sampler x) => x.sampler;

	public void Dispose() 
	{
		vkDestroySampler((nint)device, sampler, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroySampler(nint device, nint sampler, nint allocator);
	}

	private Sampler(Device device, nint sampler) => (this.device, this.sampler) = (device, sampler);
	internal Sampler(Device device, nint sampler, Handle<AllocationCallbacks> allocator) : this(device, sampler) => this.allocator = allocator;
}
