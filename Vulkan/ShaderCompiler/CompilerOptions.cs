using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using static Vulkan.Constants;
using System.ComponentModel;

namespace Vulkan.ShaderCompiler;

public sealed class CompilerOptions : IDisposable
{
	private readonly CompilerOptionsHandle compilerOptions;

	internal CompilerOptionsHandle Handle => compilerOptions;

	public IEnumerable<(string, string)> MacroDefinitions
	{
		set
		{
			foreach (var x in value)
			{
				(string n, string v) = x;
				shaderc_compile_options_add_macro_definition(compilerOptions, n, (nuint)n.Length, v, (nuint)v.Length);
			}

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_add_macro_definition(CompilerOptionsHandle compilerOptions, string name, nuint nameLength, string value, nuint valueLength);
		}
	}

	public IEnumerable<(Limit, int)> Limits
	{
		set
		{
			foreach (var x in value)
			{
				(Limit n, int v) = x;
				shaderc_compile_options_set_target_spirv(compilerOptions, n, v);
			}

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_target_spirv(CompilerOptionsHandle compilerOptions, Limit n, int v);
		}
	}

	public ShaderLanguage ShaderLanguage
	{
		set
		{
			shaderc_compile_options_set_source_language(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_source_language(CompilerOptionsHandle compilerOptions, ShaderLanguage language);
		}
	}

	public OptimizationLevel OptimizationLevel
	{
		set
		{
			shaderc_compile_options_set_optimization_level(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_optimization_level(CompilerOptionsHandle compilerOptions, OptimizationLevel level);
		}
	}

	public (TargetEnvironment, EnvironmentVersion) TargetEnvironment
	{
		set
		{
			(TargetEnvironment target, EnvironmentVersion version) = value;

			shaderc_compile_options_set_target_env(compilerOptions, target, version);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_target_env(CompilerOptionsHandle compilerOptions, TargetEnvironment target, EnvironmentVersion version);
		}
	}

	// // Sets a descriptor set and binding for an HLSL register in the given stage.
	// // This method keeps a copy of the string data.
	// SHADERC_EXPORT void shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage(
	//     shaderc_compile_options_t options, shaderc_shader_kind shader_kind,
	//     const char* reg, const char* set, const char* binding);
	// // Like shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage,
	// // but affects all shader stages.
	// SHADERC_EXPORT void shaderc_compile_options_set_hlsl_register_set_and_binding(
	//     shaderc_compile_options_t options, const char* reg, const char* set,
	//     const char* binding);
	// SHADERC_EXPORT void shaderc_compile_options_set_binding_base(
	//     shaderc_compile_options_t options,
	//     shaderc_uniform_kind kind,
	//     uint32_t base);
	// // Like shaderc_compile_options_set_binding_base, but only takes effect when
	// // compiling a given shader stage.  The stage is assumed to be one of vertex,
	// // fragment, tessellation evaluation, tesselation control, geometry, or compute.
	// SHADERC_EXPORT void shaderc_compile_options_set_binding_base_for_stage(
	//     shaderc_compile_options_t options, shaderc_shader_kind shader_kind,
	//     shaderc_uniform_kind kind, uint32_t base);

	public SPIRVVersion SPIRVVersion
	{
		set
		{
			shaderc_compile_options_set_optimization_level(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_optimization_level(CompilerOptionsHandle compilerOptions, SPIRVVersion version);
		}
	}

	public bool GenerateDebugInfo
	{
		set
		{
			if (value)
				shaderc_compile_options_set_generate_debug_info(compilerOptions);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_generate_debug_info(CompilerOptionsHandle compilerOptions);
		}
	}

	public bool WarningsAsErrors
	{
		set
		{
			if (value)
				shaderc_compile_options_set_warnings_as_errors(compilerOptions);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_warnings_as_errors(CompilerOptionsHandle compilerOptions);
		}
	}

	public bool SuppressWarnings
	{
		set
		{
			if (value)
				shaderc_compile_options_set_suppress_warnings(compilerOptions);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_suppress_warnings(CompilerOptionsHandle compilerOptions);
		}
	}

	public bool AutoBindUniforms
	{
		set
		{
			shaderc_compile_options_set_auto_bind_uniforms(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_auto_bind_uniforms(CompilerOptionsHandle compilerOptions, bool autoBind);
		}
	}

	public bool AutoCombinedImageSampler
	{
		set
		{
			shaderc_compile_options_set_auto_combined_image_sampler(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_auto_combined_image_sampler(CompilerOptionsHandle compilerOptions, bool upgrade);
		}
	}

	public bool HLSLIOMapping
	{
		set
		{
			shaderc_compile_options_set_hlsl_io_mapping(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_io_mapping(CompilerOptionsHandle compilerOptions, bool hlslIOMap);
		}
	}

	public bool HLSLOffsets
	{
		set
		{
			shaderc_compile_options_set_hlsl_offsets(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_offsets(CompilerOptionsHandle compilerOptions, bool hlslOffsets);
		}
	}

	public bool PreserveBindings
	{
		set
		{
			shaderc_compile_options_set_preserve_bindings(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_preserve_bindings(CompilerOptionsHandle compilerOptions, bool preserveBindings);
		}
	}

	public bool AutoMapLocations
	{
		set
		{
			shaderc_compile_options_set_auto_map_locations(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_auto_map_locations(CompilerOptionsHandle compilerOptions, bool autoMap);
		}
	}

	public bool HLSLFunctionality1
	{
		set
		{
			shaderc_compile_options_set_hlsl_functionality1(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_functionality1(CompilerOptionsHandle compilerOptions, bool enable);
		}
	}

	public bool HLSL16BitTypes
	{
		set
		{
			shaderc_compile_options_set_hlsl_16bit_types(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_16bit_types(CompilerOptionsHandle compilerOptions, bool enable);
		}
	}

	public bool VulkanRulesRelaxed
	{
		set
		{
			shaderc_compile_options_set_vulkan_rules_relaxed(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_vulkan_rules_relaxed(CompilerOptionsHandle compilerOptions, bool enable);
		}
	}

	public bool InvertY
	{
		set
		{
			shaderc_compile_options_set_invert_y(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_invert_y(CompilerOptionsHandle compilerOptions, bool enable);
		}
	}

	public bool NanClamp
	{
		set
		{
			shaderc_compile_options_set_nan_clamp(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_nan_clamp(CompilerOptionsHandle compilerOptions, bool enable);
		}
	}

	public string[] IncludeDirectories { set; get; } = [];

	public void Dispose()
	{
		shaderc_compile_options_release(compilerOptions);

		[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_release(CompilerOptionsHandle compilerOptions);
	}

	public override string ToString() => compilerOptions.ToString();

	public CompilerOptions()
	{
		this.compilerOptions = shaderc_compile_options_initialize();

		IncludeResolverDelegate includeResolver = (options, requestedSource, type, requestingSource, includeDepth) =>
		{
			string filename, content;

			switch (type)
			{
				case IncludeType.Relative:
					filename = Path.GetFullPath(Path.Join(Path.GetDirectoryName(requestingSource), requestedSource), requestingSource);
					if (File.Exists(filename))
					{
						content = File.ReadAllText(filename);
						return new(new IncludeResult(filename: filename, content: content, userData: default));
					}
					break;
				case IncludeType.Standard:
					foreach (var dir in IncludeDirectories)
					{
						filename = Path.GetFullPath(Path.Join(dir, requestedSource));
						if (File.Exists(filename))
						{
							content = File.ReadAllText(filename);
							return new(new IncludeResult(filename: filename, content: content, userData: default));
						}
					}
					break;
				default:
					throw new InvalidOperationException($"Unsupported include type '{type}'.");
			}

			return new(new IncludeResult(filename: "", content: "", userData: default));
		};

		IncludeResultReleaserDelegate includeResultReleaser = (options, result) =>
		{
			((IncludeResult)result).Dispose();
			result.Dispose();
		};

		shaderc_compile_options_set_include_callbacks(compilerOptions, includeResolver, includeResultReleaser, default);

		[DllImport(SHADERC_LIB)] static extern CompilerOptionsHandle shaderc_compile_options_initialize();
		[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_include_callbacks(
			CompilerOptionsHandle options,
			IncludeResolverDelegate resolver,
			IncludeResultReleaserDelegate resultRelease,
			nint userData
		);
	}

	internal CompilerOptions(CompilerOptionsHandle compilerOptions) => this.compilerOptions = compilerOptions;
}
