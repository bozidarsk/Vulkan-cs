using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Vulkan;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate nint AllocationFunctionDelegate(nint userData, nuint size, nuint alignment, SystemAllocationScope allocationScope);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate nint ReallocationFunctionDelegate(nint userData, nint original, nuint size, nuint alignment, SystemAllocationScope allocationScope);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void FreeFunctionDelegate(nint userData, nint memory);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void InternalAllocationNotificationDelegate(nint userData, nuint size, InternalAllocationType allocationType, SystemAllocationScope allocationScope);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void InternalFreeNotificationDelegate(nint userData, nuint size, InternalAllocationType allocationType, SystemAllocationScope allocationScope);

public sealed class AllocationCallbacks : IDisposable
{
	private readonly AllocationCallbacksHandle allocationCallbacks;

	internal AllocationCallbacksHandle Handle => allocationCallbacks;

	public unsafe void Dispose()
	{
		fixed (AllocationCallbacksHandle* pAllocationCallbacks = &allocationCallbacks)
			Marshal.FreeHGlobal(*(nint*)pAllocationCallbacks);
	}

	public override string ToString() => (allocationCallbacks != default) ? allocationCallbacks.ToString() : "null";

	public unsafe AllocationCallbacks(
		AllocationFunctionDelegate allocationFunction,
		ReallocationFunctionDelegate reallocationFunction,
		FreeFunctionDelegate freeFunction,
		InternalAllocationNotificationDelegate internalAllocationNotification,
		InternalFreeNotificationDelegate internalFreeNotification,
		nint userData = 0
	)
	{
		if (allocationFunction == null || reallocationFunction == null || freeFunction == null || internalAllocationNotification == null || internalFreeNotification == null)
			throw new ArgumentNullException();

		nint* data = (nint*)Marshal.AllocHGlobal(6 * sizeof(nint));

		// this is the layout of VkAllocationCallbacks:
		data[0] = userData;
		data[1] = Marshal.GetFunctionPointerForDelegate(allocationFunction!);
		data[2] = Marshal.GetFunctionPointerForDelegate(reallocationFunction!);
		data[3] = Marshal.GetFunctionPointerForDelegate(freeFunction!);
		data[4] = Marshal.GetFunctionPointerForDelegate(internalAllocationNotification!);
		data[5] = Marshal.GetFunctionPointerForDelegate(internalFreeNotification!);

		fixed (AllocationCallbacksHandle* pAllocationCallbacks = &allocationCallbacks)
			*(nint*)pAllocationCallbacks = (nint)data;
	}
}
