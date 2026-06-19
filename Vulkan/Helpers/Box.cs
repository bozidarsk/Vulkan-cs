global using Vulkan.Helpers;

using System;
using System.Runtime.InteropServices;

namespace Vulkan.Helpers;

internal unsafe readonly struct Box<T> : IDisposable where T : struct
{
	private readonly T* pointer;

	public bool IsNull => pointer == default;

	public static implicit operator T(Box<T> box) => !box.IsNull ? *box.pointer : throw new NullReferenceException();

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

	public override string ToString() => $"0x{(nint)pointer:x}";

	private Box(int length) => this.pointer = (length > 0) ? (T*)Marshal.AllocHGlobal(sizeof(T) * length) : default;

	public Box(T obj) : this(1)
	{
		*this.pointer = obj;
	}

	public Box(T[]? array) : this(array?.Length ?? 0)
	{
		for (int i = 0; i < (array?.Length ?? 0); i++)
			this.pointer[i] = array![i];
	}
}
