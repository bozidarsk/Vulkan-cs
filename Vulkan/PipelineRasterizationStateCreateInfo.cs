namespace Vulkan;

public readonly struct PipelineRasterizationStateCreateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly PipelineRasterizationStateCreateFlags Flags;
	public readonly bool32 DepthClampEnable;
	public readonly bool32 RasterizerDiscardEnable;
	public readonly PolygonMode PolygonMode;
	public readonly CullMode CullMode;
	public readonly FrontFace FrontFace;
	public readonly bool32 DepthBiasEnable;
	public readonly float DepthBiasConstantFactor;
	public readonly float DepthBiasClamp;
	public readonly float DepthBiasSlopeFactor;
	public readonly float LineWidth;

	public PipelineRasterizationStateCreateInfo(
		StructureType type,
		nint next,
		PipelineRasterizationStateCreateFlags flags,
		bool depthClampEnable,
		bool rasterizerDiscardEnable,
		PolygonMode polygonMode,
		CullMode cullMode,
		FrontFace frontFace,
		bool depthBiasEnable,
		float depthBiasConstantFactor,
		float depthBiasClamp,
		float depthBiasSlopeFactor,
		float lineWidth
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.DepthClampEnable = depthClampEnable;
		this.RasterizerDiscardEnable = rasterizerDiscardEnable;
		this.PolygonMode = polygonMode;
		this.CullMode = cullMode;
		this.FrontFace = frontFace;
		this.DepthBiasEnable = depthBiasEnable;
		this.DepthBiasConstantFactor = depthBiasConstantFactor;
		this.DepthBiasClamp = depthBiasClamp;
		this.DepthBiasSlopeFactor = depthBiasSlopeFactor;
		this.LineWidth = lineWidth;
	}
}
