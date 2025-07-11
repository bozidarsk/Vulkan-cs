using System;

namespace Vulkan.ShaderCompiler;

public static class ShaderCompilerExtensions 
{
	public static ShaderKind GetShaderKind(this ShaderStage stage) => stage switch 
	{
		ShaderStage.Vertex => ShaderKind.VertexShader,
		ShaderStage.Fragment => ShaderKind.FragmentShader,
		ShaderStage.Compute => ShaderKind.ComputeShader,
		ShaderStage.Geometry => ShaderKind.GeometryShader,
		ShaderStage.TessellationControl => ShaderKind.TessControlShader,
		ShaderStage.TessellationEvaluation => ShaderKind.TessEvaluationShader,
		_ => throw new ArgumentOutOfRangeException()
	};
}
