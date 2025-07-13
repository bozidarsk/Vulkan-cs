using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vulkan;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct DefaultVertex : IVertex
{
	public readonly Vector3 Position;
	public readonly Color Color;

	public int Stride => Marshal.SizeOf<DefaultVertex>();

	public VertexInputBindingDescription BindingDescription => new(
		binding: 0,
		stride: (uint)Stride,
		inputRate: VertexInputRate.Vertex
	);

	public VertexInputAttributeDescription[] AttributeDescriptions => 
		typeof(DefaultVertex)
		.GetFields()
		.Index()
		.Select(x => 
			new VertexInputAttributeDescription(
				location: (uint)x.Item1,
				binding: 0,
				format: x.Item2.FieldType.FullName! switch 
				{
					"Vulkan.Color" => Format.R32G32B32A32SFloat,
					"Vulkan.Vector2" => Format.R32G32SFloat,
					"Vulkan.Vector3" => Format.R32G32B32SFloat,
					"Vulkan.Vector4" => Format.R32G32B32A32SFloat,
					"Vulkan.Vector2Int" => Format.R32G32SInt,
					"Vulkan.Vector3Int" => Format.R32G32B32SInt,
					"Vulkan.Vector4Int" => Format.R32G32B32A32SInt,
					_ => throw new ArgumentOutOfRangeException($"Cannot map field type {x.Item2.FieldType.FullName!} to a format.")
				},
				offset: (uint)Marshal.OffsetOf(x.Item2.DeclaringType!, x.Item2.Name)
			)
		)
		.ToArray()
	;

	public DefaultVertex(Vector3 position, Color color) => (this.Position, this.Color) = (position, color);
}
