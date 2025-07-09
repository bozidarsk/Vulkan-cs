#pragma warning disable CS0169

using System;
using System.Reflection;

namespace Vulkan;

internal readonly struct PhysicalDeviceFeaturesStruct 
{
	private readonly uint RobustBufferAccess;
	private readonly uint FullDrawIndexUint32;
	private readonly uint ImageCubeArray;
	private readonly uint IndependentBlend;
	private readonly uint GeometryShader;
	private readonly uint TessellationShader;
	private readonly uint SampleRateShading;
	private readonly uint DualSrcBlend;
	private readonly uint LogicOp;
	private readonly uint MultiDrawIndirect;
	private readonly uint DrawIndirectFirstInstance;
	private readonly uint DepthClamp;
	private readonly uint DepthBiasClamp;
	private readonly uint FillModeNonSolid;
	private readonly uint DepthBounds;
	private readonly uint WideLines;
	private readonly uint LargePoints;
	private readonly uint AlphaToOne;
	private readonly uint MultiViewport;
	private readonly uint SamplerAnisotropy;
	private readonly uint TextureCompressionETC2;
	private readonly uint TextureCompressionASTC_LDR;
	private readonly uint TextureCompressionBC;
	private readonly uint OcclusionQueryPrecise;
	private readonly uint PipelineStatisticsQuery;
	private readonly uint VertexPipelineStoresAndAtomics;
	private readonly uint FragmentStoresAndAtomics;
	private readonly uint ShaderTessellationAndGeometryPointSize;
	private readonly uint ShaderImageGatherExtended;
	private readonly uint ShaderStorageImageExtendedFormats;
	private readonly uint ShaderStorageImageMultisample;
	private readonly uint ShaderStorageImageReadWithoutFormat;
	private readonly uint ShaderStorageImageWriteWithoutFormat;
	private readonly uint ShaderUniformBufferArrayDynamicIndexing;
	private readonly uint ShaderSampledImageArrayDynamicIndexing;
	private readonly uint ShaderStorageBufferArrayDynamicIndexing;
	private readonly uint ShaderStorageImageArrayDynamicIndexing;
	private readonly uint ShaderClipDistance;
	private readonly uint ShaderCullDistance;
	private readonly uint ShaderFloat64;
	private readonly uint ShaderInt64;
	private readonly uint ShaderInt16;
	private readonly uint ShaderResourceResidency;
	private readonly uint ShaderResourceMinLod;
	private readonly uint SparseBinding;
	private readonly uint SparseResidencyBuffer;
	private readonly uint SparseResidencyImage2D;
	private readonly uint SparseResidencyImage3D;
	private readonly uint SparseResidency2Samples;
	private readonly uint SparseResidency4Samples;
	private readonly uint SparseResidency8Samples;
	private readonly uint SparseResidency16Samples;
	private readonly uint SparseResidencyAliased;
	private readonly uint VariableMultisampleRate;
	private readonly uint InheritedQueries;

	public static implicit operator PhysicalDeviceFeatures (PhysicalDeviceFeaturesStruct x) => x.Value;
	public static implicit operator PhysicalDeviceFeaturesStruct (PhysicalDeviceFeatures x) => new(x);

	public PhysicalDeviceFeatures Value 
	{
		get 
		{
			PhysicalDeviceFeatures value = default;

			foreach (FieldInfo x in this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField))
				if ((uint)(x.GetValue(this)!) == 1)
					value |= Enum.Parse<PhysicalDeviceFeatures>(x.Name);

			return value;
		}
	}

	public PhysicalDeviceFeaturesStruct(PhysicalDeviceFeatures value) 
	{
		foreach (FieldInfo x in this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField))
			if (value.HasFlag(Enum.Parse<PhysicalDeviceFeatures>(x.Name)))
				x.SetValue(this, (uint)1);
	}
}

[Flags]
public enum PhysicalDeviceFeatures 
{
	RobustBufferAccess = 1 << 1,
	FullDrawIndexUint32 = 1 << 2,
	ImageCubeArray = 1 << 3,
	IndependentBlend = 1 << 4,
	GeometryShader = 1 << 5,
	TessellationShader = 1 << 6,
	SampleRateShading = 1 << 7,
	DualSrcBlend = 1 << 8,
	LogicOp = 1 << 9,
	MultiDrawIndirect = 1 << 10,
	DrawIndirectFirstInstance = 1 << 11,
	DepthClamp = 1 << 12,
	DepthBiasClamp = 1 << 13,
	FillModeNonSolid = 1 << 14,
	DepthBounds = 1 << 15,
	WideLines = 1 << 16,
	LargePoints = 1 << 17,
	AlphaToOne = 1 << 18,
	MultiViewport = 1 << 19,
	SamplerAnisotropy = 1 << 20,
	TextureCompressionETC2 = 1 << 21,
	TextureCompressionASTC_LDR = 1 << 22,
	TextureCompressionBC = 1 << 23,
	OcclusionQueryPrecise = 1 << 24,
	PipelineStatisticsQuery = 1 << 25,
	VertexPipelineStoresAndAtomics = 1 << 26,
	FragmentStoresAndAtomics = 1 << 27,
	ShaderTessellationAndGeometryPointSize = 1 << 28,
	ShaderImageGatherExtended = 1 << 29,
	ShaderStorageImageExtendedFormats = 1 << 30,
	ShaderStorageImageMultisample = 1 << 31,
	ShaderStorageImageReadWithoutFormat = 1 << 32,
	ShaderStorageImageWriteWithoutFormat = 1 << 33,
	ShaderUniformBufferArrayDynamicIndexing = 1 << 34,
	ShaderSampledImageArrayDynamicIndexing = 1 << 35,
	ShaderStorageBufferArrayDynamicIndexing = 1 << 36,
	ShaderStorageImageArrayDynamicIndexing = 1 << 37,
	ShaderClipDistance = 1 << 38,
	ShaderCullDistance = 1 << 39,
	ShaderFloat64 = 1 << 40,
	ShaderInt64 = 1 << 41,
	ShaderInt16 = 1 << 42,
	ShaderResourceResidency = 1 << 43,
	ShaderResourceMinLod = 1 << 44,
	SparseBinding = 1 << 45,
	SparseResidencyBuffer = 1 << 46,
	SparseResidencyImage2D = 1 << 47,
	SparseResidencyImage3D = 1 << 48,
	SparseResidency2Samples = 1 << 49,
	SparseResidency4Samples = 1 << 50,
	SparseResidency8Samples = 1 << 51,
	SparseResidency16Samples = 1 << 52,
	SparseResidencyAliased = 1 << 53,
	VariableMultisampleRate = 1 << 54,
	InheritedQueries = 1 << 56,
}
