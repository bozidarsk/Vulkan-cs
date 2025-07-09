namespace Vulkan;

public readonly struct VertexInputAttributeDescription 
{
	public readonly uint Location;
	public readonly uint Binding;
	public readonly Format Format;
	public readonly uint Offset;

	public VertexInputAttributeDescription(uint location, uint binding, Format format, uint offset) => 
		(this.Location, this.Binding, this.Format, this.Offset) = (location, binding, format, offset)
	;
}
