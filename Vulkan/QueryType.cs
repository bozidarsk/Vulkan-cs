namespace Vulkan;

public enum QueryType : uint
{
	Occlusion = 0,
	PipelineStatistics = 1,
	Timestamp = 2,
	ResultStatusOnlyKhr = 1000023000,
	TransformFeedbackStreamExt = 1000028004,
	PerformanceQueryKhr = 1000116000,
	AccelerationStructureCompactedSizeKhr = 1000150000,
	AccelerationStructureSerializationSizeKhr = 1000150001,
	AccelerationStructureCompactedSizeNv = 1000165000,
	TimeElapsedQcom = 1000173000,
	PerformanceQueryIntel = 1000210000,
	VideoEncodeFeedbackKhr = 1000299000,
	MeshPrimitivesGeneratedExt = 1000328000,
	PrimitivesGeneratedExt = 1000382000,
	AccelerationStructureSerializationBottomLevelPointersKhr = 1000386000,
	AccelerationStructureSizeKhr = 1000386001,
	MicromapSerializationSizeExt = 1000396000,
	MicromapCompactedSizeExt = 1000396001,
}
