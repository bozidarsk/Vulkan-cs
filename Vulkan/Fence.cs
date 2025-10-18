using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Fence : IDisposable
{
	private readonly FenceHandle fence;
	private readonly Device device;
	private readonly AllocationCallbacksHandle allocator;

	internal FenceHandle Handle => fence;

	public void Wait() 
	{
		vkWaitForFences(device.Handle, 1, in fence, true, ulong.MaxValue);

		[DllImport(VK_LIB)] static extern void vkWaitForFences(DeviceHandle device, uint fenceCount, in FenceHandle pFences, bool32 waitAll, ulong timeout);
	}

	public void Reset() 
	{
		vkResetFences(device.Handle, 1, in fence);

		[DllImport(VK_LIB)] static extern void vkResetFences(DeviceHandle device, uint fenceCount, in FenceHandle pFences);
	}

	public void Dispose() 
	{
		vkDestroyFence(device.Handle, fence, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyFence(DeviceHandle device, FenceHandle fence, AllocationCallbacksHandle allocator);
	}

	internal Fence(FenceHandle fence, Device device, AllocationCallbacksHandle allocator) => 
		(this.fence, this.device, this.allocator) = (fence, device, allocator)
	;
}

public static class FenceExtensions 
{
	public static void WaitAll(this Fence[] fences, Device device) 
	{
		if (fences == null)
			throw new ArgumentNullException();

		vkWaitForFences(device.Handle, (uint)fences.Length, ref MemoryMarshal.GetArrayDataReference(fences.Select(x => x.Handle).ToArray()), true, ulong.MaxValue);

		[DllImport(VK_LIB)] static extern void vkWaitForFences(DeviceHandle device, uint fenceCount, ref FenceHandle pFences, bool32 waitAll, ulong timeout);
	}

	public static void ResetAll(this Fence[] fences, Device device) 
	{
		if (fences == null)
			throw new ArgumentNullException();

		vkResetFences(device.Handle, (uint)fences.Length, ref MemoryMarshal.GetArrayDataReference(fences.Select(x => x.Handle).ToArray()));

		[DllImport(VK_LIB)] static extern void vkResetFences(DeviceHandle device, uint fenceCount, ref FenceHandle pFences);
	}
}
