namespace Vulkan;

public readonly struct PhysicalDeviceVertexInputDynamicStateFeatures
{
	public readonly StructureType Type = StructureType.PhysicalDeviceVertexInputDynamicStateFeaturesExt;
	public readonly nint Next;
	public readonly bool32 VertexInputDynamicState;

	public PhysicalDeviceVertexInputDynamicStateFeatures(nint next, bool vertexInputDynamicState) =>
		(this.Next, this.VertexInputDynamicState) = (next, vertexInputDynamicState)
	;
}
