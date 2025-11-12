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
			switch (x.Key) 
			{
				case "stage":
					var tokens = x.Value.Split(' ');

					if ((tokens.Length != 1 && tokens.Length != 2) || !Enum.TryParse<ShaderStage>(tokens[0], true, out ShaderStage stage))
						goto case null;

					info.Stage = stage;
					info.EntryPoint = (tokens.Length == 2) ? tokens[1] : "main";
					break;
				case "language":
					if (!Enum.TryParse<ShaderLanguage>(x.Value, true, out language))
						goto case null;

					info.Language = language;
					break;
				case "cull":
					if (!Enum.TryParse<CullMode>(x.Value, true, out CullMode cullMode))
						goto case null;

					info.CullMode =  cullMode;
					break;
				case "frontface":
					if (!Enum.TryParse<FrontFace>(x.Value, true, out FrontFace frontFace))
						goto case null;

					info.FrontFace =  frontFace;
					break;
				case "blend":
					var factors = x.Value.Split(' ');
					if (factors.Length == 3) 
					{
						BlendFactor factor;

						if (Enum.TryParse<BlendFactor>(factors[0], true, out factor))
							info.SourceBlendFactor = factor;
						else goto case null;

						if (Enum.TryParse<BlendOp>(factors[1], true, out BlendOp op))
							info.BlendOp = op;
						else goto case null;

						if (Enum.TryParse<BlendFactor>(factors[2], true, out factor))
							info.DestinationBlendFactor = factor;
						else goto case null;
					}
					else if (factors.Length == 2)
					{
						BlendFactor factor;

						if (Enum.TryParse<BlendFactor>(factors[0], true, out factor))
							info.SourceBlendFactor = factor;
						else goto case null;

						if (Enum.TryParse<BlendFactor>(factors[1], true, out factor))
							info.DestinationBlendFactor = factor;
						else goto case null;
					}
					else if (factors.Length == 1) 
					{
						if (factors[0] == "disable" || factors[0] == "off" || factors[0] == "none")
							info.DisableBlending = true;
						else goto case null;
					}
					else goto case null;
					break;
				case null:
					throw new VulkanException($"Failed to process shader properties in '{info.File}'. (#pragma {x.Key} {x.Value})");
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
