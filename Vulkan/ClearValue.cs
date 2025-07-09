namespace Vulkan;

public readonly struct ClearValue 
{
	public readonly ClearColorValue Color;
	public readonly ClearDepthStencilValue DepthStencil;

	public ClearValue(ClearColorValue color, ClearDepthStencilValue depthStencil) => (this.Color, this.DepthStencil) = (color, depthStencil);
}
