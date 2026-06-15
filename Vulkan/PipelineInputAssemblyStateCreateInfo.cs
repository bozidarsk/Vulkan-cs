namespace Vulkan;

public readonly struct PipelineInputAssemblyStateCreateInfo
{
	public readonly StructureType Type = StructureType.PipelineInputAssemblyStateCreateInfo;
	public readonly nint Next;
	public readonly PipelineInputAssemblyStateCreateFlags Flags;
	public readonly PrimitiveTopology Topology;
	public readonly bool32 PrimitiveRestartEnable;

	public PipelineInputAssemblyStateCreateInfo(nint next, PipelineInputAssemblyStateCreateFlags flags, PrimitiveTopology topology, bool primitiveRestartEnable)
	{
		this.Next = next;
		this.Flags = flags;
		this.Topology = topology;
		this.PrimitiveRestartEnable = primitiveRestartEnable;
	}
}
