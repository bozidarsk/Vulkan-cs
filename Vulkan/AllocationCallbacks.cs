using System.Runtime.InteropServices;

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

public readonly struct AllocationCallbacks 
{
	public readonly nint UserData;
	public readonly AllocationFunctionDelegate AllocationFunction;
	public readonly ReallocationFunctionDelegate ReallocationFunction;
	public readonly FreeFunctionDelegate FreeFunction;
	public readonly InternalAllocationNotificationDelegate InternalAllocationNotification;
	public readonly InternalFreeNotificationDelegate InternalFreeNotification;
}
