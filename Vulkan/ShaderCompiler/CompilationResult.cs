using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan.ShaderCompiler;

public sealed class CompilationResult : IDisposable
{
	private readonly CompilationResultHandle compilationResult;

	internal CompilationResultHandle Handle => compilationResult;

	public unsafe string Text
	{
		get
		{
			return new(shaderc_result_get_bytes(compilationResult), 0, (int)shaderc_result_get_length(compilationResult));

			[DllImport(SHADERC_LIB)] static extern sbyte* shaderc_result_get_bytes(CompilationResultHandle compilationResult);
			[DllImport(SHADERC_LIB)] static extern nuint shaderc_result_get_length(CompilationResultHandle compilationResult);
		}
	}

	public unsafe byte[] Data
	{
		get
		{
			return new Span<byte>(shaderc_result_get_bytes(compilationResult), (int)shaderc_result_get_length(compilationResult)).ToArray();

			[DllImport(SHADERC_LIB)] static extern byte* shaderc_result_get_bytes(CompilationResultHandle compilationResult);
			[DllImport(SHADERC_LIB)] static extern nuint shaderc_result_get_length(CompilationResultHandle compilationResult);
		}
	}

	public CompilationStatus Status
	{
		get
		{
			return shaderc_result_get_compilation_status(compilationResult);

			[DllImport(SHADERC_LIB)] static extern CompilationStatus shaderc_result_get_compilation_status(CompilationResultHandle compilationResult);
		}
	}

	public unsafe string ErrorMessage
	{
		get
		{
			return new(shaderc_result_get_error_message(compilationResult));

			[DllImport(SHADERC_LIB)] static extern sbyte* shaderc_result_get_error_message(CompilationResultHandle compilationResult);
		}
	}

	public void Dispose()
	{
		shaderc_result_release(compilationResult);

		[DllImport(SHADERC_LIB)] static extern void shaderc_result_release(CompilationResultHandle compilationResult);
	}

	public override string ToString() => compilationResult.ToString();

	internal CompilationResult(CompilationResultHandle compilationResult) => this.compilationResult = compilationResult;
}
