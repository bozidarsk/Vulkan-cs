namespace Vulkan;

public readonly struct VertexInputAttributeDescription2 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly VertexInputAttributeDescription Description;

	public VertexInputAttributeDescription2(
		StructureType type,
		nint next,
		VertexInputAttributeDescription description
	) => (this.Type, this.Next, this.Description) = (type, next, description);
}
