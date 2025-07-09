using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct GraphicsPipelineCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly PipelineCreateFlags Flags;
	private readonly uint stageCount;
	private readonly Handle<PipelineShaderStageCreateInfo> stages;
	private readonly Handle<PipelineVertexInputStateCreateInfo> vertexInputState;
	private readonly Handle<PipelineInputAssemblyStateCreateInfo> inputAssemblyState;
	private readonly Handle<PipelineTessellationStateCreateInfo> tessellationState;
	private readonly Handle<PipelineViewportStateCreateInfo> viewportState;
	private readonly Handle<PipelineRasterizationStateCreateInfo> rasterizationState;
	private readonly Handle<PipelineMultisampleStateCreateInfo> multisampleState;
	private readonly Handle<PipelineDepthStencilStateCreateInfo> depthStencilState;
	private readonly Handle<PipelineColorBlendStateCreateInfo> colorBlendState;
	private readonly Handle<PipelineDynamicStateCreateInfo> dynamicState;
	private readonly nint layout;
	private readonly nint renderPass;
	public readonly uint Subpass;
	private readonly nint basePipeline;
	public readonly int BasePipelineIndex;

	public PipelineShaderStageCreateInfo[]? Stages => stages.ToArray(stageCount);
	public PipelineVertexInputStateCreateInfo VertexInputState => vertexInputState;
	public PipelineInputAssemblyStateCreateInfo InputAssemblyState => inputAssemblyState;
	public PipelineTessellationStateCreateInfo TessellationState => tessellationState;
	public PipelineViewportStateCreateInfo ViewportState => viewportState;
	public PipelineRasterizationStateCreateInfo RasterizationState => rasterizationState;
	public PipelineMultisampleStateCreateInfo MultisampleState => multisampleState;
	public PipelineDepthStencilStateCreateInfo DepthStencilState => depthStencilState;
	public PipelineColorBlendStateCreateInfo ColorBlendState => colorBlendState;
	public PipelineDynamicStateCreateInfo DynamicState => dynamicState;
	public PipelineLayout Layout => throw new NotImplementedException(); // cannot get allocator and device params
	public RenderPass RenderPass => throw new NotImplementedException(); // cannot get allocator and device params
	public Pipeline BasePipeline => throw new NotImplementedException(); // cannot get allocator and device params

	public Pipeline CreateGraphicsPipeline(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateGraphicsPipelines((nint)device, default, 1, in this, allocator, out nint graphicsPipelineHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, graphicsPipelineHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateGraphicsPipelines(nint device, PipelineCache cache, uint count, in GraphicsPipelineCreateInfo createInfos, nint allocator, out nint graphicsPipeline);
	}

	public void Dispose() 
	{
		stages.Dispose();
		vertexInputState.Dispose();
		inputAssemblyState.Dispose();
		tessellationState.Dispose();
		viewportState.Dispose();
		rasterizationState.Dispose();
		multisampleState.Dispose();
		depthStencilState.Dispose();
		colorBlendState.Dispose();
		dynamicState.Dispose();
	}

	public GraphicsPipelineCreateInfo(
		StructureType type,
		nint next,
		PipelineCreateFlags flags,
		PipelineShaderStageCreateInfo[]? stages,
		PipelineVertexInputStateCreateInfo? vertexInputState,
		PipelineInputAssemblyStateCreateInfo? inputAssemblyState,
		PipelineTessellationStateCreateInfo? tessellationState,
		PipelineViewportStateCreateInfo? viewportState,
		PipelineRasterizationStateCreateInfo? rasterizationState,
		PipelineMultisampleStateCreateInfo? multisampleState,
		PipelineDepthStencilStateCreateInfo? depthStencilState,
		PipelineColorBlendStateCreateInfo? colorBlendState,
		PipelineDynamicStateCreateInfo? dynamicState,
		PipelineLayout layout,
		RenderPass renderPass,
		uint subpass,
		Pipeline? basePipeline,
		int basePipelineIndex
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;

		this.stageCount = (uint)(stages?.Length ?? 0);
		this.stages = new(stages);

		this.vertexInputState = (vertexInputState is PipelineVertexInputStateCreateInfo a) ? new(a) : default;
		this.inputAssemblyState = (inputAssemblyState is PipelineInputAssemblyStateCreateInfo b) ? new(b) : default;
		this.tessellationState = (tessellationState is PipelineTessellationStateCreateInfo c) ? new(c) : default;
		this.viewportState = (viewportState is PipelineViewportStateCreateInfo d) ? new(d) : default;
		this.rasterizationState = (rasterizationState is PipelineRasterizationStateCreateInfo e) ? new(e) : default;
		this.multisampleState = (multisampleState is PipelineMultisampleStateCreateInfo f) ? new(f) : default;
		this.depthStencilState = (depthStencilState is PipelineDepthStencilStateCreateInfo g) ? new(g) : default;
		this.colorBlendState = (colorBlendState is PipelineColorBlendStateCreateInfo h) ? new(h) : default;
		this.dynamicState = (dynamicState is PipelineDynamicStateCreateInfo i) ? new(i) : default;

		this.layout = (nint)layout;
		this.renderPass = (nint)renderPass;
		this.Subpass = subpass;

		this.basePipeline = (basePipeline != null) ? (nint)basePipeline : default;
		this.BasePipelineIndex = basePipelineIndex;
	}
}
