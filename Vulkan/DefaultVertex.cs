using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vulkan;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct DefaultVertex : IVertex
{
	public Vector3 Position { set; get; }
	public Color Color { set; get; }

	public Vector2 UV { set {} }
	public Vector3 Normal { set {} }

	public DefaultVertex(Vector3 position, Color color) => (this.Position, this.Color) = (position, color);
}
