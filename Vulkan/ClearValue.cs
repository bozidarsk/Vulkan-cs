using System.Runtime.InteropServices;

namespace Vulkan;

[StructLayout(LayoutKind.Explicit)]
public readonly struct ClearValue
{
	[FieldOffset(0)] public readonly ClearColorValue Color;
	[FieldOffset(0)] public readonly ClearDepthStencilValue DepthStencil;

	public ClearValue(ClearColorValue color) => this.Color = color;
	public ClearValue(ClearDepthStencilValue depthStencil) => this.DepthStencil = depthStencil;
}
