namespace Vulkan;

public readonly struct ClearDepthStencilValue 
{
	public readonly float Depth;
	public readonly uint Stencil;

	public ClearDepthStencilValue(float depth, uint stencil) => (this.Depth, this.Stencil) = (depth, stencil);
}
