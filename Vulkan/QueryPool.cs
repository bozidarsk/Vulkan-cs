using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class QueryPool : IDisposable
{
	private readonly QueryPoolHandle queryPool;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal QueryPoolHandle Handle => queryPool;

	public void GetPoolResults(uint firstQuery, uint queryCount, int dataSize, ref byte data, int stride, QueryResultFlags flags)
	{
		vkGetQueryPoolResults(device.Handle, queryPool, firstQuery, queryCount, (nuint)dataSize, ref data, (ulong)stride, flags);

		[DllImport(VK_LIB)] static extern Result vkGetQueryPoolResults(DeviceHandle device, QueryPoolHandle queryPool, uint firstQuery, uint queryCount, nuint dataSize, ref byte data, DeviceSize stride, QueryResultFlags flags);
	}

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.QueryPool,
				objectHandle: (ulong)(nint)queryPool,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroyQueryPool(device.Handle, queryPool, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyQueryPool(DeviceHandle device, QueryPoolHandle framebuffer, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => queryPool.ToString();

	internal QueryPool(QueryPoolHandle framebuffer, Device device, AllocationCallbacks? allocator) =>
		(this.queryPool, this.device, this.allocator) = (framebuffer, device, allocator)
	;
}
