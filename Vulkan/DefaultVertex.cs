using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vulkan;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct DefaultVertex : IVertex
{
	public readonly Vector3 Position;
	public readonly Color Color;

	public DefaultVertex(Vector3 position, Color color) => (this.Position, this.Color) = (position, color);
}
