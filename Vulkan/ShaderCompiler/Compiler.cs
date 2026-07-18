using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan.ShaderCompiler;

public sealed class Compiler : IDisposable
{
	private readonly CompilerHandle compiler;

	internal CompilerHandle Handle => compiler;

	private static void GetShaderMetadata(Shader shader, CompilerOptions options)
	{
		int lineIndex = 0;

		List<(Limit, int)> limits = new();

		if (Enum.TryParse<ShaderLanguage>(Path.GetExtension(shader.File).TrimStart('.').ToUpper(), out ShaderLanguage language))
			options.ShaderLanguage = language;

		foreach (var line in File.ReadAllLines(shader.File))
		{
			if (!line.TrimStart().StartsWith("#pragma"))
				continue;

			var tokens = line.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToArray();

			string name = tokens[1];
			var values = new Span<string>(tokens, 2, tokens.Length - 2);

			if (values.Length == 1 && Enum.TryParse<Limit>(name, true, out Limit limit) && int.TryParse(values[0], out int limitValue))
			{
				limits.Add((limit, limitValue));

				lineIndex++;
				continue;
			}

			switch (name.ToLower())
			{
				case "stage":
					if ((values.Length != 1 && values.Length != 2) || !Enum.TryParse<ShaderStage>(values[0], true, out ShaderStage stage))
						goto default;

					if (values.Length == 2)
						shader.EntryPoint = values[1];

					shader.Stage = stage;
					break;
				case "language":
					if (values.Length != 1 || !Enum.TryParse<ShaderLanguage>(values[0], true, out language))
						goto default;

					options.ShaderLanguage = language;
					break;
				case "cull":
					if (values.Length != 1 || !Enum.TryParse<CullMode>(values[0], true, out CullMode cull))
						goto default;

					shader.CullMode = cull;
					break;
				case "frontface":
					if (values.Length != 1 || !Enum.TryParse<FrontFace>(values[0], true, out FrontFace frontFace))
						goto default;

					shader.FrontFace = frontFace;
					break;
				case "blend":
					if (values.Length == 3)
					{
						BlendFactor factor;

						if (Enum.TryParse<BlendFactor>(values[0], true, out factor))
							shader.SourceBlendFactor = factor;
						else goto default;

						if (Enum.TryParse<BlendOp>(values[1], true, out BlendOp op))
							shader.BlendOp = op;
						else goto default;

						if (Enum.TryParse<BlendFactor>(values[2], true, out factor))
							shader.DestinationBlendFactor = factor;
						else goto default;
					}
					else if (values.Length == 2)
					{
						BlendFactor factor;

						if (Enum.TryParse<BlendFactor>(values[0], true, out factor))
							shader.SourceBlendFactor = factor;
						else goto default;

						if (Enum.TryParse<BlendFactor>(values[1], true, out factor))
							shader.DestinationBlendFactor = factor;
						else goto default;
					}
					else if (values.Length == 1)
					{
						if (values[0].ToLower() == "disabled" || values[0].ToLower() == "off")
							shader.DisableBlending = true;
						else goto default;
					}
					else goto default;
					break;
				case "environment":
					if (values.Length != 2 || !Enum.TryParse<TargetEnvironment>(values[0], true, out TargetEnvironment target) || !Enum.TryParse<EnvironmentVersion>(values[0], true, out EnvironmentVersion version))
						goto default;

					options.Environment = (target, version);
					break;
				case "spirv":
					if (values.Length != 1 || !Enum.TryParse<SPIRVVersion>(values[0], true, out SPIRVVersion spirv))
						goto default;

					options.SPIRVVersion = spirv;
					break;
				case "optimization":
					if (values.Length == 1 && (values[0].ToLower() == "disabled" || values[0].ToLower() == "off"))
					{
						options.OptimizationLevel = OptimizationLevel.Zero;
						break;
					}

					if (values.Length != 1 || !Enum.TryParse<OptimizationLevel>(values[0], true, out OptimizationLevel optimization))
						goto default;

					options.OptimizationLevel = optimization;
					break;
				case "generatedebuginfo":
					options.GenerateDebugInfo = true;
					break;
				case "warningsaserrors":
					options.WarningsAsErrors = true;
					break;
				case "suppresswarnings":
					options.SuppressWarnings = true;
					break;
				case "autobinduniforms":
					options.AutoBindUniforms = true;
					break;
				case "autocombinedimagesampler":
					options.AutoCombinedImageSampler = true;
					break;
				case "hlsliomapping":
					options.HLSLIOMapping = true;
					break;
				case "hlsloffsets":
					options.HLSLOffsets = true;
					break;
				case "preservebindings":
					options.PreserveBindings = true;
					break;
				case "automaplocations":
					options.AutoMapLocations = true;
					break;
				case "hlslfunctionality1":
					options.HLSLFunctionality1 = true;
					break;
				case "hlsl16bittypes":
					options.HLSL16BitTypes = true;
					break;
				case "vulkanrulesrelaxed":
					options.VulkanRulesRelaxed = true;
					break;
				case "inverty":
					options.InvertY = true;
					break;
				case "nanclamp":
					options.NanClamp = true;
					break;
				default:
					throw new VulkanException($"Failed to process shader properties in '{shader.File}'. (#pragma '{name}' at line {lineIndex})");
			}

			lineIndex++;
		}

		options.Limits = limits.ToArray();
	}

	public Shader Compile(string filename, CompilerOptions? options = null)
	{
		if (filename == null)
			throw new ArgumentNullException();

		bool disposeOptions = options == null;
		options ??= new CompilerOptions()
		{
			Environment = (TargetEnvironment.Vulkan, EnvironmentVersion.Vulkan13),
			SPIRVVersion = SPIRVVersion.Version16,
			InvertY = true,
		};

		byte[] source;

		var shader = new Shader() { File = Path.GetFullPath(filename) };

		GetShaderMetadata(shader, options);

		source = File.ReadAllBytes(shader.File);
		using CompilationResult preprocessingResult = new(shaderc_compile_into_preprocessed_text(compiler, ref MemoryMarshal.GetArrayDataReference(source), (nuint)source.Length, shader.Stage.GetShaderKind(), shader.File, shader.EntryPoint ?? "main", options.Handle));

		if (preprocessingResult.Status != CompilationStatus.Success)
		{
			if (disposeOptions)
				options.Dispose();

			throw new VulkanException($"Failed to preprocess shader '{shader.File}' - {preprocessingResult.Status}.\n{preprocessingResult.ErrorMessage}");
		}

		source = preprocessingResult.Data;
		using CompilationResult compilationResult = new(shaderc_compile_into_spv(compiler, ref MemoryMarshal.GetArrayDataReference(source), (nuint)source.Length, shader.Stage.GetShaderKind(), shader.File, shader.EntryPoint ?? "main", options.Handle));

		if (compilationResult.Status != CompilationStatus.Success)
		{
			if (disposeOptions)
				options.Dispose();

			throw new VulkanException($"Failed to compile shader '{shader.File}'.\n{compilationResult.ErrorMessage}");
		}

		if (disposeOptions)
			options.Dispose();

		shader.Code = compilationResult.Data;
		return shader;

		[DllImport(SHADERC_LIB)]
		static extern CompilationResultHandle shaderc_compile_into_preprocessed_text(
			CompilerHandle compiler,
			ref byte source,
			nuint sourceLength,
			ShaderKind kind,
			string filename,
			string entryPoint,
			CompilerOptionsHandle options
		);

		[DllImport(SHADERC_LIB)]
		static extern CompilationResultHandle shaderc_compile_into_spv(
			CompilerHandle compiler,
			ref byte source,
			nuint length,
			ShaderKind kind,
			string filename,
			string entryPoint,
			CompilerOptionsHandle options
		);
	}

	public void Dispose()
	{
		shaderc_compiler_release(compiler);

		[DllImport(SHADERC_LIB)] static extern void shaderc_compiler_release(CompilerHandle compiler);
	}

	public override string ToString() => compiler.ToString();

	public Compiler()
	{
		this.compiler = shaderc_compiler_initialize();

		[DllImport(SHADERC_LIB)] static extern CompilerHandle shaderc_compiler_initialize();
	}

	internal Compiler(CompilerHandle compiler) => this.compiler = compiler;
}
