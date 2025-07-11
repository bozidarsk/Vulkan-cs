namespace Vulkan.ShaderCompiler;

public enum TargetEnvironment : int
{
	Vulkan = 0, // SPIR-V under Vulkan semantics
	Opengl, // SPIR-V under OpenGL semantics
	// NOTE: SPIR-V code generation is not supported for shaders under OpenGL
	// compatibility profile.
	OpenglCompat, // SPIR-V under OpenGL semantics,
	// including compatibility profile
	// functions
	[System.Obsolete] WebGPU, // Deprecated, SPIR-V under WebGPU
	// semantics
	Default = Vulkan
}
