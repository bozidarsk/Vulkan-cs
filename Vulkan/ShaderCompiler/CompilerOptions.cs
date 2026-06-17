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

	public (string name, string value)[] MacroDefinitions
	{
		set
		{
			if (value == null)
				throw new ArgumentNullException();

			field = value;

			foreach (var x in value)
				shaderc_compile_options_add_macro_definition(compilerOptions, x.name, (nuint)x.name.Length, x.value, (nuint)x.value.Length);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_add_macro_definition(CompilerOptionsHandle compilerOptions, string name, nuint nameLength, string value, nuint valueLength);
		}
		get;
	} = [];

	public (Limit limit, int value)[] Limits
	{
		set
		{
			if (value == null)
				throw new ArgumentNullException();

			field = value;

			foreach (var x in value)
				shaderc_compile_options_set_target_spirv(compilerOptions, x.limit, x.value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_target_spirv(CompilerOptionsHandle compilerOptions, Limit limit, int value);
		}
		get;
	} = [];

	public ShaderLanguage ShaderLanguage
	{
		set
		{
			field = value;

			shaderc_compile_options_set_source_language(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_source_language(CompilerOptionsHandle compilerOptions, ShaderLanguage language);
		}
		get;
	}

	public OptimizationLevel OptimizationLevel
	{
		set
		{
			field = value;

			shaderc_compile_options_set_optimization_level(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_optimization_level(CompilerOptionsHandle compilerOptions, OptimizationLevel level);
		}
		get;
	}

	public (TargetEnvironment target, EnvironmentVersion version) Environment
	{
		set
		{
			field = value;

			shaderc_compile_options_set_target_env(compilerOptions, value.target, value.version);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_target_env(CompilerOptionsHandle compilerOptions, TargetEnvironment target, EnvironmentVersion version);
		}
		get;
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
			field = value;

			shaderc_compile_options_set_target_spirv(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_target_spirv(CompilerOptionsHandle compilerOptions, SPIRVVersion version);
		}
		get;
	}

	public bool GenerateDebugInfo
	{
		set
		{
			field = value;

			if (value)
				shaderc_compile_options_set_generate_debug_info(compilerOptions);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_generate_debug_info(CompilerOptionsHandle compilerOptions);
		}
		get;
	}

	public bool WarningsAsErrors
	{
		set
		{
			field = value;

			if (value)
				shaderc_compile_options_set_warnings_as_errors(compilerOptions);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_warnings_as_errors(CompilerOptionsHandle compilerOptions);
		}
		get;
	}

	public bool SuppressWarnings
	{
		set
		{
			field = value;

			if (value)
				shaderc_compile_options_set_suppress_warnings(compilerOptions);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_suppress_warnings(CompilerOptionsHandle compilerOptions);
		}
		get;
	}

	public bool AutoBindUniforms
	{
		set
		{
			field = value;

			shaderc_compile_options_set_auto_bind_uniforms(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_auto_bind_uniforms(CompilerOptionsHandle compilerOptions, bool autoBind);
		}
		get;
	}

	public bool AutoCombinedImageSampler
	{
		set
		{
			field = value;

			shaderc_compile_options_set_auto_combined_image_sampler(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_auto_combined_image_sampler(CompilerOptionsHandle compilerOptions, bool upgrade);
		}
		get;
	}

	public bool HLSLIOMapping
	{
		set
		{
			field = value;

			shaderc_compile_options_set_hlsl_io_mapping(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_io_mapping(CompilerOptionsHandle compilerOptions, bool hlslIOMap);
		}
		get;
	}

	public bool HLSLOffsets
	{
		set
		{
			field = value;

			shaderc_compile_options_set_hlsl_offsets(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_offsets(CompilerOptionsHandle compilerOptions, bool hlslOffsets);
		}
		get;
	}

	public bool PreserveBindings
	{
		set
		{
			field = value;

			shaderc_compile_options_set_preserve_bindings(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_preserve_bindings(CompilerOptionsHandle compilerOptions, bool preserveBindings);
		}
		get;
	}

	public bool AutoMapLocations
	{
		set
		{
			field = value;

			shaderc_compile_options_set_auto_map_locations(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_auto_map_locations(CompilerOptionsHandle compilerOptions, bool autoMap);
		}
		get;
	}

	public bool HLSLFunctionality1
	{
		set
		{
			field = value;

			shaderc_compile_options_set_hlsl_functionality1(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_functionality1(CompilerOptionsHandle compilerOptions, bool enable);
		}
		get;
	}

	public bool HLSL16BitTypes
	{
		set
		{
			field = value;

			shaderc_compile_options_set_hlsl_16bit_types(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_16bit_types(CompilerOptionsHandle compilerOptions, bool enable);
		}
		get;
	}

	public bool VulkanRulesRelaxed
	{
		set
		{
			field = value;

			shaderc_compile_options_set_vulkan_rules_relaxed(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_vulkan_rules_relaxed(CompilerOptionsHandle compilerOptions, bool enable);
		}
		get;
	}

	public bool InvertY
	{
		set
		{
			field = value;

			shaderc_compile_options_set_invert_y(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_invert_y(CompilerOptionsHandle compilerOptions, bool enable);
		}
		get;
	}

	public bool NanClamp
	{
		set
		{
			field = value;

			shaderc_compile_options_set_nan_clamp(compilerOptions, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_nan_clamp(CompilerOptionsHandle compilerOptions, bool enable);
		}
		get;
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
