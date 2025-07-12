namespace Vulkan;

public readonly struct MemoryRequirements 
{
	public readonly DeviceSize Size;
	public readonly DeviceSize Alignment;
	public readonly uint MemoryType;

	public MemoryRequirements(DeviceSize size, DeviceSize alignment, uint memoryType) => 
		(this.Size, this.Alignment, this.MemoryType) = (size, alignment, memoryType)
	;
}
