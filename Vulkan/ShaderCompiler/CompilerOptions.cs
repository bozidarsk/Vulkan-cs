using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan.ShaderCompiler;

public readonly struct CompilerOptions : IDisposable
{
	private readonly nint handle;

	public IEnumerable<(string, string)> MacroDefinitions 
	{
		set 
		{
			foreach (var x in value) 
			{
				(string n, string v) = x;
				shaderc_compile_options_add_macro_definition(this, n, (nuint)n.Length, v, (nuint)v.Length);
			}

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_add_macro_definition(CompilerOptions options, string name, nuint nameLength, string value, nuint valueLength);
		}
	}

	public IEnumerable<(Limit, int)> Limits 
	{
		set 
		{
			foreach (var x in value) 
			{
				(Limit n, int v) = x;
				shaderc_compile_options_set_target_spirv(this, n, v);
			}

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_target_spirv(CompilerOptions options, Limit n, int v);
		}
	}

	public ShaderLanguage ShaderLanguage 
	{
		set 
		{
			shaderc_compile_options_set_source_language(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_source_language(CompilerOptions options, ShaderLanguage language);
		}
	}

	public OptimizationLevel OptimizationLevel 
	{
		set 
		{
			shaderc_compile_options_set_optimization_level(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_optimization_level(CompilerOptions options, OptimizationLevel level);
		}
	}

	public (TargetEnvironment, EnvironmentVersion) TargetEnvironment 
	{
		set 
		{
			(TargetEnvironment target, EnvironmentVersion version) = value;

			shaderc_compile_options_set_target_env(this, target, version);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_target_env(CompilerOptions options, TargetEnvironment target, EnvironmentVersion version);
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
			shaderc_compile_options_set_optimization_level(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_optimization_level(CompilerOptions options, SPIRVVersion version);
		}
	}

	public bool GenerateDebugInfo 
	{
		set 
		{
			if (value)
				shaderc_compile_options_set_generate_debug_info(this);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_generate_debug_info(CompilerOptions options);
		}
	}

	public bool WarningsAsErrors 
	{
		set 
		{
			if (value)
				shaderc_compile_options_set_warnings_as_errors(this);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_warnings_as_errors(CompilerOptions options);
		}
	}

	public bool SuppressWarnings 
	{
		set 
		{
			if (value)
				shaderc_compile_options_set_suppress_warnings(this);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_suppress_warnings(CompilerOptions options);
		}
	}

	public bool AutoBindUniforms 
	{
		set 
		{
			shaderc_compile_options_set_auto_bind_uniforms(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_auto_bind_uniforms(CompilerOptions options, bool autoBind);
		}
	}

	public bool AutoCombinedImageSampler 
	{
		set 
		{
			shaderc_compile_options_set_auto_combined_image_sampler(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_auto_combined_image_sampler(CompilerOptions options, bool upgrade);
		}
	}

	public bool HLSLIOMapping 
	{
		set 
		{
			shaderc_compile_options_set_hlsl_io_mapping(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_io_mapping(CompilerOptions options, bool hlslIOMap);
		}
	}

	public bool HLSLOffsets 
	{
		set 
		{
			shaderc_compile_options_set_hlsl_offsets(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_offsets(CompilerOptions options, bool hlslOffsets);
		}
	}

	public bool PreserveBindings 
	{
		set 
		{
			shaderc_compile_options_set_preserve_bindings(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_preserve_bindings(CompilerOptions options, bool preserveBindings);
		}
	}

	public bool AutoMapLocations 
	{
		set 
		{
			shaderc_compile_options_set_auto_map_locations(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_auto_map_locations(CompilerOptions options, bool autoMap);
		}
	}

	public bool HLSLFunctionality1 
	{
		set 
		{
			shaderc_compile_options_set_hlsl_functionality1(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_functionality1(CompilerOptions options, bool enable);
		}
	}

	public bool HLSL16BitTypes 
	{
		set 
		{
			shaderc_compile_options_set_hlsl_16bit_types(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_hlsl_16bit_types(CompilerOptions options, bool enable);
		}
	}

	public bool VulkanRulesRelaxed 
	{
		set 
		{
			shaderc_compile_options_set_vulkan_rules_relaxed(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_vulkan_rules_relaxed(CompilerOptions options, bool enable);
		}
	}

	public bool InvertY 
	{
		set 
		{
			shaderc_compile_options_set_invert_y(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_invert_y(CompilerOptions options, bool enable);
		}
	}

	public bool NanClamp 
	{
		set 
		{
			shaderc_compile_options_set_nan_clamp(this, value);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_nan_clamp(CompilerOptions options, bool enable);
		}
	}

	public string[] IncludeDirectories 
	{
		set 
		{
			if (value == null)
				throw new ArgumentNullException();

			includeDirectoriesMap[this] = value;
			shaderc_compile_options_set_include_callbacks(this, includeResolver, includeResultReleaser, this);

			[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_set_include_callbacks(
				CompilerOptions options,
				IncludeResolverDelegate resolver,
				IncludeResultReleaserDelegate resultRelease,
				CompilerOptions userData
			);
		}
	}

	private static Dictionary<CompilerOptions, string[]> includeDirectoriesMap = new();

	private static readonly IncludeResolverDelegate includeResolver = (options, requestedSource, type, requestingSource, includeDepth) => 
	{
		string filename, content;

		switch (type) 
		{
			case IncludeType.Relative:
				filename = Path.GetFullPath(Path.Join(Path.GetDirectoryName(requestingSource), requestedSource), requestingSource);
				content = File.ReadAllText(filename);
				break;
			case IncludeType.Standard:
				foreach (var dir in includeDirectoriesMap[options]) 
				{
					filename = Path.GetFullPath(Path.Join(dir, requestedSource));
					if (File.Exists(filename)) 
					{
						content = File.ReadAllText(filename);
						goto ret;
					}
				}

				filename = "";
				content = "Cannot find source";
				break;
			default:
				throw new InvalidOperationException($"Unsupported IncludeType '{type}'.");
		}

		ret:
		return new(
			new IncludeResult(
				filename: filename,
				content: content,
				userData: default
			)
		);
	};

	private static readonly IncludeResultReleaserDelegate includeResultReleaser = (options, result) => 
	{
		((IncludeResult)result).Dispose();
		result.Dispose();
	};

	public void Dispose() 
	{
		shaderc_compile_options_release(this);
		includeDirectoriesMap.Remove(this);

		[DllImport(SHADERC_LIB)] static extern void shaderc_compile_options_release(CompilerOptions options);
	}

	public static bool operator == (CompilerOptions a, CompilerOptions b) => a.handle == b.handle;
	public static bool operator != (CompilerOptions a, CompilerOptions b) => a.handle != b.handle;
	public override bool Equals(object? other) => (other is CompilerOptions x) ? x.handle == handle : false;

	public static implicit operator nint (CompilerOptions x) => x.handle;

	public override string ToString() => handle.ToString();
	public override int GetHashCode() => handle.GetHashCode();

	public CompilerOptions() 
	{
		this.handle = shaderc_compile_options_initialize();

		[DllImport(SHADERC_LIB)] static extern CompilerOptions shaderc_compile_options_initialize();
	}
}
