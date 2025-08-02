namespace Vulkan;

public readonly struct DescriptorPoolSize 
{
	public readonly DescriptorType Type;
	public readonly uint DescriptorCount;

	public DescriptorPoolSize(DescriptorType type, uint descriptorCount) => (this.Type, this.DescriptorCount) = (type, descriptorCount);
}
