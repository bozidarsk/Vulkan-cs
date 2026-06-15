namespace Vulkan;

public readonly struct VertexInputAttributeDescription2
{
	public readonly StructureType Type = StructureType.VertexInputAttributeDescription2Ext;
	public readonly nint Next;
	public readonly VertexInputAttributeDescription Description;

	public VertexInputAttributeDescription2(
		nint next,
		VertexInputAttributeDescription description
	) => (this.Next, this.Description) = (next, description);
}
