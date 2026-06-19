using Vulkan;

namespace Vulkan.ShaderCompiler;

public sealed class Shader
{
	public required string File { init; get; }
	public byte[] Code { internal set; get; } = [];
	public ShaderStage Stage { internal set; get; } = ShaderStage.All;
	public string EntryPoint { internal set; get; } = "main";
	public CullMode? CullMode { internal set; get; }
	public FrontFace? FrontFace { internal set; get; }
	public BlendFactor? SourceBlendFactor { internal set; get; }
	public BlendFactor? DestinationBlendFactor { internal set; get; }
	public BlendOp? BlendOp { internal set; get; }
	public bool? DisableBlending { internal set; get; }

	public override string ToString() =>
	$$$"""
	{
		File: {{{File}}}
		Code: {{{Code.Length}}} bytes
		Stage: {{{Stage}}}
		EntryPoint: {{{EntryPoint}}}
		CullMode: {{{CullMode?.ToString() ?? "default"}}}
		FrontFace: {{{FrontFace?.ToString() ?? "default"}}}
		SourceBlendFactor: {{{SourceBlendFactor?.ToString() ?? "default"}}}
		DestinationBlendFactor: {{{DestinationBlendFactor?.ToString() ?? "default"}}}
		BlendOp: {{{BlendOp?.ToString() ?? "default"}}}
		DisableBlending: {{{DisableBlending?.ToString() ?? "default"}}}
	}
	""";
}
