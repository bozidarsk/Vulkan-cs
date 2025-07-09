namespace Vulkan;

public readonly struct PipelineInputAssemblyStateCreateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly PipelineInputAssemblyStateCreateFlags Flags;
	public readonly PrimitiveTopology Topology;
	public readonly bool32 PrimitiveRestartEnable;

	public PipelineInputAssemblyStateCreateInfo(StructureType type, nint next, PipelineInputAssemblyStateCreateFlags flags, PrimitiveTopology topology, bool primitiveRestartEnable) 
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.Topology = topology;
		this.PrimitiveRestartEnable = primitiveRestartEnable;
	}
}
