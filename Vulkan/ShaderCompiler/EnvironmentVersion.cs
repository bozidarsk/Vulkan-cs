namespace Vulkan.ShaderCompiler;

public enum EnvironmentVersion : int
{
	Vulkan10 = ((1 << 22)),
	Vulkan11 = ((1 << 22) | (1 << 12)),
	Vulkan12 = ((1 << 22) | (2 << 12)),
	Vulkan13 = ((1 << 22) | (3 << 12)),
	Vulkan14 = ((1 << 22) | (4 << 12)),
	// For OpenGL, use the number from #version in shaders.
	// TODO(dneto): Currently no difference between OpenGL 4.5 and 4.6.
	// See glslang/Standalone/Standalone.cpp
	// TODO(dneto): Glslang doesn't accept a OpenGL client version of 460.
	Opengl45 = 450,
	[System.Obsolete] WebGPU, // Deprecated, WebGPU env never defined versions
}
