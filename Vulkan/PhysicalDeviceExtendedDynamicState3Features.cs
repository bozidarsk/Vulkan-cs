namespace Vulkan;

public readonly struct PhysicalDeviceExtendedDynamicState3Features
{
	public readonly StructureType Type = StructureType.PhysicalDeviceExtendedDynamicState3FeaturesExt;
	public readonly nint Next;
	public readonly bool32 TessellationDomainOrigin;
	public readonly bool32 DepthClampEnable;
	public readonly bool32 PolygonMode;
	public readonly bool32 RasterizationSamples;
	public readonly bool32 SampleMask;
	public readonly bool32 AlphaToCoverageEnable;
	public readonly bool32 AlphaToOneEnable;
	public readonly bool32 LogicOpEnable;
	public readonly bool32 ColorBlendEnable;
	public readonly bool32 ColorBlendEquation;
	public readonly bool32 ColorWriteMask;
	public readonly bool32 RasterizationStream;
	public readonly bool32 ConservativeRasterizationMode;
	public readonly bool32 ExtraPrimitiveOverestimationSize;
	public readonly bool32 DepthClipEnable;
	public readonly bool32 SampleLocationsEnable;
	public readonly bool32 ColorBlendAdvanced;
	public readonly bool32 ProvokingVertexMode;
	public readonly bool32 LineRasterizationMode;
	public readonly bool32 LineStippleEnable;
	public readonly bool32 DepthClipNegativeOneToOne;
	public readonly bool32 ViewportWScalingEnable;
	public readonly bool32 ViewportSwizzle;
	public readonly bool32 CoverageToColorEnable;
	public readonly bool32 CoverageToColorLocation;
	public readonly bool32 CoverageModulationMode;
	public readonly bool32 CoverageModulationTableEnable;
	public readonly bool32 CoverageModulationTable;
	public readonly bool32 CoverageReductionMode;
	public readonly bool32 RepresentativeFragmentTestEnable;
	public readonly bool32 ShadingRateImageEnable;

	public PhysicalDeviceExtendedDynamicState3Features(
		nint next,
		bool tessellationDomainOrigin,
		bool depthClampEnable,
		bool polygonMode,
		bool rasterizationSamples,
		bool sampleMask,
		bool alphaToCoverageEnable,
		bool alphaToOneEnable,
		bool logicOpEnable,
		bool colorBlendEnable,
		bool colorBlendEquation,
		bool colorWriteMask,
		bool rasterizationStream,
		bool conservativeRasterizationMode,
		bool extraPrimitiveOverestimationSize,
		bool depthClipEnable,
		bool sampleLocationsEnable,
		bool colorBlendAdvanced,
		bool provokingVertexMode,
		bool lineRasterizationMode,
		bool lineStippleEnable,
		bool depthClipNegativeOneToOne,
		bool viewportWScalingEnable,
		bool viewportSwizzle,
		bool coverageToColorEnable,
		bool coverageToColorLocation,
		bool coverageModulationMode,
		bool coverageModulationTableEnable,
		bool coverageModulationTable,
		bool coverageReductionMode,
		bool representativeFragmentTestEnable,
		bool shadingRateImageEnable
	)
	{
		this.Next = next;
		this.TessellationDomainOrigin = tessellationDomainOrigin;
		this.DepthClampEnable = depthClampEnable;
		this.PolygonMode = polygonMode;
		this.RasterizationSamples = rasterizationSamples;
		this.SampleMask = sampleMask;
		this.AlphaToCoverageEnable = alphaToCoverageEnable;
		this.AlphaToOneEnable = alphaToOneEnable;
		this.LogicOpEnable = logicOpEnable;
		this.ColorBlendEnable = colorBlendEnable;
		this.ColorBlendEquation = colorBlendEquation;
		this.ColorWriteMask = colorWriteMask;
		this.RasterizationStream = rasterizationStream;
		this.ConservativeRasterizationMode = conservativeRasterizationMode;
		this.ExtraPrimitiveOverestimationSize = extraPrimitiveOverestimationSize;
		this.DepthClipEnable = depthClipEnable;
		this.SampleLocationsEnable = sampleLocationsEnable;
		this.ColorBlendAdvanced = colorBlendAdvanced;
		this.ProvokingVertexMode = provokingVertexMode;
		this.LineRasterizationMode = lineRasterizationMode;
		this.LineStippleEnable = lineStippleEnable;
		this.DepthClipNegativeOneToOne = depthClipNegativeOneToOne;
		this.ViewportWScalingEnable = viewportWScalingEnable;
		this.ViewportSwizzle = viewportSwizzle;
		this.CoverageToColorEnable = coverageToColorEnable;
		this.CoverageToColorLocation = coverageToColorLocation;
		this.CoverageModulationMode = coverageModulationMode;
		this.CoverageModulationTableEnable = coverageModulationTableEnable;
		this.CoverageModulationTable = coverageModulationTable;
		this.CoverageReductionMode = coverageReductionMode;
		this.RepresentativeFragmentTestEnable = representativeFragmentTestEnable;
		this.ShadingRateImageEnable = shadingRateImageEnable;
	}
}
