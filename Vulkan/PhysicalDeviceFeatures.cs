#pragma warning disable CS0169

using System;

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

	public static implicit operator PhysicalDeviceFeatures(PhysicalDeviceFeaturesStruct x) => x.Value;
	public static implicit operator PhysicalDeviceFeaturesStruct(PhysicalDeviceFeatures x) => new(x);

	private unsafe PhysicalDeviceFeatures Value
	{
		get
		{
			fixed (PhysicalDeviceFeaturesStruct* pThis = &this)
			{
				ulong value = default;
				var span = new Span<uint>(pThis, sizeof(PhysicalDeviceFeaturesStruct) / sizeof(uint));

				for (int i = 0; i < span.Length; i++)
					value |= (ulong)span[i] << i;

				return (PhysicalDeviceFeatures)value;
			}
		}
	}

	private unsafe PhysicalDeviceFeaturesStruct(PhysicalDeviceFeatures value)
	{
		fixed (PhysicalDeviceFeaturesStruct* pThis = &this)
		{
			var span = new Span<uint>(pThis, sizeof(PhysicalDeviceFeaturesStruct) / sizeof(uint));

			for (int i = 0; i < span.Length; i++)
				span[i] = (uint)((ulong)value >> i) & 1;
		}
	}
}

[Flags]
public enum PhysicalDeviceFeatures : ulong
{
	RobustBufferAccess = 1ul << 0,
	FullDrawIndexUint32 = 1ul << 1,
	ImageCubeArray = 1ul << 2,
	IndependentBlend = 1ul << 3,
	GeometryShader = 1ul << 4,
	TessellationShader = 1ul << 5,
	SampleRateShading = 1ul << 6,
	DualSrcBlend = 1ul << 7,
	LogicOp = 1ul << 8,
	MultiDrawIndirect = 1ul << 9,
	DrawIndirectFirstInstance = 1ul << 10,
	DepthClamp = 1ul << 11,
	DepthBiasClamp = 1ul << 12,
	FillModeNonSolid = 1ul << 13,
	DepthBounds = 1ul << 14,
	WideLines = 1ul << 15,
	LargePoints = 1ul << 16,
	AlphaToOne = 1ul << 17,
	MultiViewport = 1ul << 18,
	SamplerAnisotropy = 1ul << 19,
	TextureCompressionETC2 = 1ul << 20,
	TextureCompressionASTC_LDR = 1ul << 21,
	TextureCompressionBC = 1ul << 22,
	OcclusionQueryPrecise = 1ul << 23,
	PipelineStatisticsQuery = 1ul << 24,
	VertexPipelineStoresAndAtomics = 1ul << 25,
	FragmentStoresAndAtomics = 1ul << 26,
	ShaderTessellationAndGeometryPointSize = 1ul << 27,
	ShaderImageGatherExtended = 1ul << 28,
	ShaderStorageImageExtendedFormats = 1ul << 29,
	ShaderStorageImageMultisample = 1ul << 30,
	ShaderStorageImageReadWithoutFormat = 1ul << 31,
	ShaderStorageImageWriteWithoutFormat = 1ul << 32,
	ShaderUniformBufferArrayDynamicIndexing = 1ul << 33,
	ShaderSampledImageArrayDynamicIndexing = 1ul << 34,
	ShaderStorageBufferArrayDynamicIndexing = 1ul << 35,
	ShaderStorageImageArrayDynamicIndexing = 1ul << 36,
	ShaderClipDistance = 1ul << 37,
	ShaderCullDistance = 1ul << 38,
	ShaderFloat64 = 1ul << 39,
	ShaderInt64 = 1ul << 40,
	ShaderInt16 = 1ul << 41,
	ShaderResourceResidency = 1ul << 42,
	ShaderResourceMinLod = 1ul << 43,
	SparseBinding = 1ul << 44,
	SparseResidencyBuffer = 1ul << 45,
	SparseResidencyImage2D = 1ul << 46,
	SparseResidencyImage3D = 1ul << 47,
	SparseResidency2Samples = 1ul << 48,
	SparseResidency4Samples = 1ul << 49,
	SparseResidency8Samples = 1ul << 50,
	SparseResidency16Samples = 1ul << 51,
	SparseResidencyAliased = 1ul << 52,
	VariableMultisampleRate = 1ul << 53,
	InheritedQueries = 1ul << 54,
}
