using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Sampler : IDisposable
{
	private readonly SamplerHandle sampler;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal SamplerHandle Handle => sampler;

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.Sampler,
				objectHandle: (ulong)(nint)sampler,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroySampler(device.Handle, sampler, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroySampler(DeviceHandle device, SamplerHandle sampler, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => sampler.ToString();

	internal Sampler(SamplerHandle sampler, Device device, AllocationCallbacks? allocator) =>
		(this.sampler, this.device, this.allocator) = (sampler, device, allocator)
	;
}
