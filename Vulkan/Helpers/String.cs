#pragma warning disable CS8981

global using Vulkan.Helpers;
global using cstring = global::Vulkan.Helpers.String;

using System;
using System.Runtime.InteropServices;

namespace Vulkan.Helpers;

public readonly struct String : IDisposable
{
	private readonly nint x;

	public string? Value => ToString();

	public static implicit operator string? (cstring x) => x.ToString();
	public static implicit operator cstring (string? x) => new(x);

	public unsafe override string? ToString() => (x != default) ? new((sbyte*)x) : null;

	public void Dispose() => Marshal.FreeHGlobal(x);

	private String(string? x) => this.x = (x != null) ? Marshal.StringToHGlobalAnsi(x) : default;
}
