using System;
using System.IO;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

using Compiler = nint;
using CompileOptions = nint;
using ShaderKind = int;
using CompilationResult = nint;
using CompilationStatus = int;

public static class ShaderCompiler 
{
	private static ShaderKind GetShaderKind(ShaderStage stage) => stage switch 
	{
		ShaderStage.Vertex => shaderc_vertex_shader,
		ShaderStage.Fragment => shaderc_fragment_shader,
		ShaderStage.Compute => shaderc_compute_shader,
		ShaderStage.Geometry => shaderc_geometry_shader,
		ShaderStage.TessellationControl => shaderc_tess_control_shader,
		ShaderStage.TessellationEvaluation => shaderc_tess_evaluation_shader,
		_ => throw new ArgumentOutOfRangeException()
	};

	public static unsafe byte[] Compile(string filename, ShaderStage stage, string entryPoint = "main") 
	{
		if (filename == null || entryPoint == null)
			throw new ArgumentNullException();

		Compiler compiler = shaderc_compiler_initialize();
		CompileOptions options = shaderc_compile_options_initialize();
		ShaderKind kind = GetShaderKind(stage);

		string source = File.ReadAllText(filename);
		CompilationResult result = shaderc_compile_into_spv(compiler, source, (nuint)source.Length, kind, filename, entryPoint, options);

		if (shaderc_result_get_compilation_status(result) != shaderc_compilation_status_success)
			throw new VulkanException($"Failed to compile shader '{filename}'.\n{new string(shaderc_result_get_error_message(result))}");

		int length = (int)shaderc_result_get_length(result);
		var code = new byte[length];

		Marshal.Copy(shaderc_result_get_bytes(result), code, 0, length);

		shaderc_result_release(result);
		shaderc_compiler_release(compiler);
		shaderc_compile_options_release(options);

		return code;

		[DllImport(SHADERC_LIB)] static extern Compiler shaderc_compiler_initialize();
		[DllImport(SHADERC_LIB)] static extern CompileOptions shaderc_compile_options_initialize();
		[DllImport(SHADERC_LIB)] static extern CompilationResult shaderc_compile_into_spv(Compiler compiler, string source, nuint length, ShaderKind kind, string filename, string entryPoint, CompileOptions options);
		[DllImport(SHADERC_LIB)] static extern CompilationStatus shaderc_result_get_compilation_status(CompilationResult result);
		[DllImport(SHADERC_LIB)] static extern nint shaderc_result_get_bytes(CompilationResult result);
		[DllImport(SHADERC_LIB)] static extern nuint shaderc_result_get_length(CompilationResult result);
		[DllImport(SHADERC_LIB)] static extern sbyte* shaderc_result_get_error_message(CompilationResult result);
		[DllImport(SHADERC_LIB)] static extern void shaderc_result_release(CompilationResult result);
		[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_release(CompileOptions options);
		[DllImport(SHADERC_LIB)] static extern void shaderc_compiler_release(Compiler compiler);
	}
}
