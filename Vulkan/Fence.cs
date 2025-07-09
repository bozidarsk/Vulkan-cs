using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Fence : IDisposable
{
	private readonly Device device;
	private readonly nint fence;
	private readonly Handle<AllocationCallbacks> allocator;

	public unsafe void Wait() 
	{
		fixed (nint* x = &fence)
			vkWaitForFences((nint)device, 1, x, true, ulong.MaxValue);

		[DllImport(VK_LIB)] static extern void vkWaitForFences(nint device, uint fenceCount, nint* pFences, bool32 waitAll, ulong timeout);
	}

	public unsafe void Reset() 
	{
		fixed (nint* x = &fence)
			vkResetFences((nint)device, 1, x);

		[DllImport(VK_LIB)] static extern void vkResetFences(nint device, uint fenceCount, nint* pFences);
	}

	public static explicit operator nint (Fence x) => x.fence;

	public void Dispose() 
	{
		vkDestroyFence((nint)device, fence, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyFence(nint device, nint fence, nint allocator);
	}

	private Fence(Device device, nint fence) => (this.device, this.fence) = (device, fence);
	internal Fence(Device device, nint fence, Handle<AllocationCallbacks> allocator) : this(device, fence) => this.allocator = allocator;
}

public static class FenceExtensions 
{
	public static void WaitAll(this Fence[]? fences, Device device) 
	{
		if (fences == null)
			throw new ArgumentNullException();

		vkWaitForFences((nint)device, (uint)(fences?.Length ?? 0), fences!.Select(x => (nint)x).ToArray().AsPointer(), true, ulong.MaxValue);

		[DllImport(VK_LIB)] static extern void vkWaitForFences(nint device, uint fenceCount, nint pFences, bool32 waitAll, ulong timeout);
	}

	public static void ResetAll(this Fence[]? fences, Device device) 
	{
		if (fences == null)
			throw new ArgumentNullException();

		vkResetFences((nint)device, (uint)(fences?.Length ?? 0), fences!.Select(x => (nint)x).ToArray().AsPointer());

		[DllImport(VK_LIB)] static extern void vkResetFences(nint device, uint fenceCount, nint pFences);
	}
}
