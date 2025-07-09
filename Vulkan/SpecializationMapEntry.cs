namespace Vulkan;

public readonly struct SpecializationMapEntry 
{
	public readonly uint ConstantID;
	public readonly uint Offset;
	public readonly nuint Size;

	public SpecializationMapEntry(uint constantID, uint offset, nuint size) => (this.ConstantID, this.Offset, this.Size) = (constantID, offset, size);
}
