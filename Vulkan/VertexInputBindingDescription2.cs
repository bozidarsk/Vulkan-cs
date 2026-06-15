namespace Vulkan;

public readonly struct VertexInputBindingDescription2
{
	public readonly StructureType Type = StructureType.VertexInputBindingDescription2Ext;
	public readonly nint Next;
	public readonly VertexInputBindingDescription Description;
	public readonly uint Divisor;

	public VertexInputBindingDescription2(
		nint next,
		VertexInputBindingDescription description,
		uint divisor
	) => (this.Next, this.Description, this.Divisor) = (next, description, divisor);
}
