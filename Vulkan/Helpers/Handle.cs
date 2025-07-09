global using Vulkan.Helpers;

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Vulkan.Helpers;

public unsafe readonly struct Handle<T> : IDisposable where T : struct
{
	private readonly T* pointer;

	public bool IsNull => pointer == default;

	public static implicit operator nint (Handle<T> handle) => (nint)handle.pointer;
	public static implicit operator T* (Handle<T> handle) => handle.pointer;
	public static implicit operator T (Handle<T> handle) => !handle.IsNull ? *handle.pointer : throw new NullReferenceException();

	public T[]? ToArray(uint length) 
	{
		if (IsNull)
			return null;

		T[] array = new T[length];

		for (int i = 0; i < length; i++)
			array[i] = pointer[i];

		return array;
	}

	public void Dispose() => Marshal.FreeHGlobal((nint)pointer);

	public override string ToString() => ((nint)pointer).ToString();

	public Handle(int length = 1) => this.pointer = (length > 0) ? (T*)Marshal.AllocHGlobal(sizeof(T) * length) : default;
	public Handle(uint length = 1) : this((int)length) {}
	public Handle(params T[]? array) : this(array?.Length ?? 0)
	{
		for (int i = 0; i < (array?.Length ?? 0); i++)
			this.pointer[i] = array![i];
	}
}

public unsafe static class HandleExtensions 
{
	public static nint AsPointer<T>(this T[]? array) => (array != null) ? (nint)Unsafe.AsPointer<T>(ref MemoryMarshal.GetArrayDataReference<T>(array)) : default;
}
