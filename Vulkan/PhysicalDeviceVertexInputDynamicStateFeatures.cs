namespace Vulkan;

public readonly struct PhysicalDeviceVertexInputDynamicStateFeatures 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly bool32 VertexInputDynamicState;

	public PhysicalDeviceVertexInputDynamicStateFeatures(StructureType type, nint next, bool vertexInputDynamicState) => 
		(this.Type, this.Next, this.VertexInputDynamicState) = (type, next, vertexInputDynamicState)
	;
}
