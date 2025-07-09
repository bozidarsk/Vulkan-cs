namespace Vulkan;

public readonly struct PushConstantRange 
{
	public readonly ShaderStage Stage;
	public readonly uint Offset;
	public readonly uint Size;

	public PushConstantRange(ShaderStage stage, uint offset, uint size) => (this.Stage, this.Offset, this.Size) = (stage, offset, size);
}
