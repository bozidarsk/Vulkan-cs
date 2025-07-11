using System;
using System.IO;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan.ShaderCompiler;

public readonly struct Compiler : IDisposable
{
	private readonly nint handle;

	public static string Preprocess(string filename, ShaderStage stage, string entryPoint = "main", Compiler? compiler = null, CompilerOptions? options = null) 
	{
		if (filename == null || entryPoint == null)
			throw new ArgumentNullException();

		bool disposeCompiler = compiler == null;
		bool disposeOptions = options == null;

		compiler ??= new Compiler();
		options ??= new CompilerOptions() 
		{
			TargetEnvironment = (TargetEnvironment.Vulkan, EnvironmentVersion.Vulkan14),
			ShaderLanguage = Enum.Parse<ShaderLanguage>(Path.GetExtension(filename).TrimStart('.').ToUpper()),
			IncludeDirectories = [ @"Shaders" ]
		};

		var c = (Compiler)compiler;
		var o = (CompilerOptions)options;

		var source = File.ReadAllText(filename);
		var kind = stage.GetShaderKind();

		using CompilationResult result = shaderc_compile_into_preprocessed_text(c, source, (nuint)source.Length, kind, filename, entryPoint, o);

		if (result.Status != CompilationStatus.Success)
			throw new VulkanException($"Failed to preprocess shader '{filename}' - {result.Status}.\n{result.ErrorMessage}");

		if (disposeCompiler)
			c.Dispose();

		if (disposeOptions)
			o.Dispose();

		return result.Text;

		[DllImport(SHADERC_LIB)]
		static extern CompilationResult shaderc_compile_into_preprocessed_text(
			Compiler compiler,
			string source,
			nuint sourceLength,
			ShaderKind kind,
			string filename,
			string entryPoint,
			CompilerOptions options
		);
	}

	public static byte[] Compile(string filename, ShaderStage stage, string entryPoint = "main") 
	{
		if (filename == null || entryPoint == null)
			throw new ArgumentNullException();

		using var compiler = new Compiler();
		using var options = new CompilerOptions() 
		{
			TargetEnvironment = (TargetEnvironment.Vulkan, EnvironmentVersion.Vulkan14),
			ShaderLanguage = Enum.Parse<ShaderLanguage>(Path.GetExtension(filename).TrimStart('.').ToUpper()),
			IncludeDirectories = [ @"Shaders" ]
		};

		var source = Preprocess(filename, stage, entryPoint, compiler, options);
		var kind = stage.GetShaderKind();

		using CompilationResult result = shaderc_compile_into_spv(compiler, source, (nuint)source.Length, kind, filename, entryPoint, options);

		if (result.Status != CompilationStatus.Success)
			throw new VulkanException($"Failed to compile shader '{filename}'.\n{result.ErrorMessage}");

		return result.Data;

		[DllImport(SHADERC_LIB)]
		static extern CompilationResult shaderc_compile_into_spv(
			Compiler compiler,
			string source,
			nuint length,
			ShaderKind kind,
			string filename,
			string entryPoint,
			CompilerOptions options
		);
	}

	public void Dispose() 
	{
		shaderc_compiler_release(this);

		[DllImport(SHADERC_LIB)] static extern void shaderc_compiler_release(Compiler compiler);
	}

	public static bool operator == (Compiler a, Compiler b) => a.handle == b.handle;
	public static bool operator != (Compiler a, Compiler b) => a.handle != b.handle;
	public override bool Equals(object? other) => (other is Compiler x) ? x.handle == handle : false;

	public static implicit operator nint (Compiler x) => x.handle;

	public override string ToString() => handle.ToString();
	public override int GetHashCode() => handle.GetHashCode();

	public Compiler() 
	{
		this.handle = shaderc_compiler_initialize();

		[DllImport(SHADERC_LIB)] static extern Compiler shaderc_compiler_initialize();
	}
}
