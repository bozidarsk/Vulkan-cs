#pragma warning disable CS0649

using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan.ShaderCompiler;

public readonly struct CompilationResult : IDisposable
{
	private readonly nint handle;

	public unsafe string Text 
	{
		get 
		{
			return new((sbyte*)shaderc_result_get_bytes(this), 0, (int)shaderc_result_get_length(this));

			[DllImport(SHADERC_LIB)] static extern nint shaderc_result_get_bytes(CompilationResult result);
			[DllImport(SHADERC_LIB)] static extern nuint shaderc_result_get_length(CompilationResult result);
		}
	}

	public byte[] Data 
	{
		get 
		{
			int length = (int)shaderc_result_get_length(this);
			var code = new byte[length];

			Marshal.Copy(shaderc_result_get_bytes(this), code, 0, length);

			return code;

			[DllImport(SHADERC_LIB)] static extern nint shaderc_result_get_bytes(CompilationResult result);
			[DllImport(SHADERC_LIB)] static extern nuint shaderc_result_get_length(CompilationResult result);
		}
	}

	public CompilationStatus Status 
	{
		get 
		{
			return shaderc_result_get_compilation_status(this);

			[DllImport(SHADERC_LIB)] static extern CompilationStatus shaderc_result_get_compilation_status(CompilationResult result);
		}
	}

	public unsafe string ErrorMessage 
	{
		get 
		{
			return new(shaderc_result_get_error_message(this));

			[DllImport(SHADERC_LIB)] static extern sbyte* shaderc_result_get_error_message(CompilationResult result);
		}
	}

	public void Dispose() 
	{
		shaderc_result_release(this);

		[DllImport(SHADERC_LIB)] static extern void shaderc_result_release(CompilationResult result);
	}

	public static bool operator == (CompilationResult a, CompilationResult b) => a.handle == b.handle;
	public static bool operator != (CompilationResult a, CompilationResult b) => a.handle != b.handle;
	public override bool Equals(object? other) => (other is CompilationResult x) ? x.handle == handle : false;

	public static implicit operator nint (CompilationResult x) => x.handle;

	public override string ToString() => handle.ToString();
	public override int GetHashCode() => handle.GetHashCode();
}
