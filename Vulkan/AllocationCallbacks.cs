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

public readonly struct AllocationCallbacks 
{
	public readonly nint UserData;
	public readonly AllocationFunctionDelegate AllocationFunction;
	public readonly ReallocationFunctionDelegate ReallocationFunction;
	public readonly FreeFunctionDelegate FreeFunction;
	public readonly InternalAllocationNotificationDelegate InternalAllocationNotification;
	public readonly InternalFreeNotificationDelegate InternalFreeNotification;
}

public readonly struct AllocationCallbacksHandle 
{
	private readonly nint value;

	public static bool operator == (AllocationCallbacksHandle a, AllocationCallbacksHandle b) => a.value == b.value;
	public static bool operator != (AllocationCallbacksHandle a, AllocationCallbacksHandle b) => a.value != b.value;
	public override bool Equals(object? other) => (other is AllocationCallbacksHandle x) ? x.value == value : false;

	public override string ToString() => value.ToString();
	public override int GetHashCode() => value.GetHashCode();

	public AllocationCallbacksHandle() => this.value = 0;

	public unsafe AllocationCallbacksHandle(ref AllocationCallbacks allocator) => 
		this.value = (nint)Unsafe.AsPointer<AllocationCallbacks>(ref allocator)
	;
}
