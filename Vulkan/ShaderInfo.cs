using Vulkan.ShaderCompiler;

namespace Vulkan;

public sealed class ShaderInfo 
{
	public string File;
	public string? EntryPoint;
	public ShaderStage? Stage;
	public CullMode? CullMode;
	public FrontFace? FrontFace;
	public BlendFactor? SourceBlendFactor;
	public BlendFactor? DestinationBlendFactor;
	public ShaderLanguage? Language;

	public ShaderInfo(
		string file,
		string? entryPoint = null,
		ShaderStage? stage = null,
		CullMode? cullMode = null,
		FrontFace? frontFace = null,
		BlendFactor? sourceBlendFactor = null,
		BlendFactor? destinationBlendFactor = null,
		ShaderLanguage? language = null
	)
	{
		this.File = file;
		this.EntryPoint = entryPoint;
		this.Stage = stage;
		this.CullMode = cullMode;
		this.FrontFace = frontFace;
		this.SourceBlendFactor = sourceBlendFactor;
		this.DestinationBlendFactor = destinationBlendFactor;
		this.Language = language;
	}
}
