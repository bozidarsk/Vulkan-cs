namespace Vulkan;

public readonly struct VertexInputBindingDescription2 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly VertexInputBindingDescription Description;
	public readonly uint Divisor;

	public VertexInputBindingDescription2(
		StructureType type,
		nint next,
		VertexInputBindingDescription description,
		uint divisor
	) => (this.Type, this.Next, this.Description, this.Divisor) = (type, next, description, divisor);
}
