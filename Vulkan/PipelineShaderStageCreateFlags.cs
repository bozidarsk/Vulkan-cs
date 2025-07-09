namespace Vulkan;

[System.Flags]
public enum PipelineShaderStageCreateFlags : uint
{
	AllowVaryingSubgroupSize = 0x00000001,
	RequireFullSubgroups = 0x00000002,
	AllowVaryingSubgroupSizeExt = AllowVaryingSubgroupSize,
	RequireFullSubgroupsExt = RequireFullSubgroups,
}
