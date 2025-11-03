using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan.ShaderCompiler;

public readonly struct Compiler : IDisposable
{
	private readonly nint handle;

	private static Dictionary<string, string> GetShaderProperties(string filename) 
	{
		var properties = new Dictionary<string, string>();

		foreach (var line in File.ReadAllLines(filename)) 
		{
			if (!line.TrimStart().StartsWith("#pragma"))
				continue;

			var tokens = line.Split(' ').Where(x => !string.IsNullOrEmpty(x));
			properties[tokens.ElementAt(1).ToLower()] = tokens.Skip(2).Aggregate((current, next) => $"{current} {next}").ToLower();
		}

		return properties;
	}

	private static string Preprocess(string filename, ShaderKind kind, string entryPoint, Compiler compiler, CompilerOptions options) 
	{
		var source = File.ReadAllText(filename);

		using CompilationResult result = shaderc_compile_into_preprocessed_text(compiler, source, (nuint)source.Length, kind, filename, entryPoint, options);

		if (result.Status != CompilationStatus.Success)
			throw new VulkanException($"Failed to preprocess shader '{filename}' - {result.Status}.\n{result.ErrorMessage}");

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

	public static byte[] Compile(ref ShaderInfo info) 
	{
		if (info == null || info.File == null)
			throw new ArgumentNullException();

		if (Enum.TryParse<ShaderLanguage>(Path.GetExtension(info.File).TrimStart('.').ToUpper(), out ShaderLanguage language))
			info.Language = language;

		foreach (var x in GetShaderProperties(info.File)) 
		{
			// TODO: TryParse else goto case null
			switch (x.Key) 
			{
				case "stage":
					var tokens = x.Value.Split(' ');
					info.Stage = Enum.Parse<ShaderStage>(tokens[0], true);
					info.EntryPoint = (tokens.Length == 2) ? tokens[1] : "main";
					break;
				case "language":
					info.Language = Enum.Parse<ShaderLanguage>(x.Value, true);
					break;
				case "cull":
					info.CullMode = Enum.Parse<CullMode>(x.Value, true);
					break;
				case "frontface":
					info.FrontFace = Enum.Parse<FrontFace>(x.Value, true);
					break;
				case "blend":
					var factors = x.Value.Split(' ');
					if (factors.Length == 3) 
					{
						info.SourceBlendFactor = Enum.Parse<BlendFactor>(factors[0], true);
						info.BlendOp = Enum.Parse<BlendOp>(factors[1], true);
						info.DestinationBlendFactor = Enum.Parse<BlendFactor>(factors[2], true);
					}
					else if (factors.Length == 2)
					{
						info.SourceBlendFactor = Enum.Parse<BlendFactor>(factors[0], true);
						info.DestinationBlendFactor = Enum.Parse<BlendFactor>(factors[1], true);
					}
					else goto case null;
					break;
				case null:
					throw new VulkanException($"Failed to process shader properties in '{info.File}'.");
			}
		}

		using var compiler = new Compiler();
		using var options = new CompilerOptions() 
		{
			TargetEnvironment = (TargetEnvironment.Vulkan, EnvironmentVersion.Vulkan10),
			IncludeDirectories = [ SHADER_INCLUDE_DIR ],
			ShaderLanguage = (ShaderLanguage)info.Language!
		};

		var kind = ((ShaderStage)info.Stage!).GetShaderKind();
		var source = Preprocess(info.File, kind, info.EntryPoint!, compiler, options);

		using CompilationResult result = shaderc_compile_into_spv(compiler, source, (nuint)source.Length, kind, info.File, info.EntryPoint!, options);

		if (result.Status != CompilationStatus.Success)
			throw new VulkanException($"Failed to compile shader '{info.File}'.\n{result.ErrorMessage}");

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
