using System;

namespace Vulkan;

public readonly struct PipelineMultisampleStateCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly PipelineMultisampleStateCreateFlags Flags;
	public readonly SampleCount RasterizationSamples;
	public readonly bool32 SampleShadingEnable;
	public readonly float MinSampleShading;
	private readonly Handle<SampleMask> sampleMask;
	public readonly bool32 AlphaToCoverageEnable;
	public readonly bool32 AlphaToOneEnable;

	public SampleMask SampleMask => sampleMask;

	public void Dispose() 
	{
		sampleMask.Dispose();
	}

	public PipelineMultisampleStateCreateInfo(
		StructureType type,
		nint next,
		PipelineMultisampleStateCreateFlags flags,
		SampleCount rasterizationSamples,
		bool sampleShadingEnable,
		float minSampleShading,
		SampleMask? sampleMask,
		bool alphaToCoverageEnable,
		bool alphaToOneEnable
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.RasterizationSamples = rasterizationSamples;
		this.SampleShadingEnable = sampleShadingEnable;
		this.MinSampleShading = minSampleShading;
		this.sampleMask = (sampleMask is SampleMask x) ? new(x) : default;
		this.AlphaToCoverageEnable = alphaToCoverageEnable;
		this.AlphaToOneEnable = alphaToOneEnable;
	}
}
