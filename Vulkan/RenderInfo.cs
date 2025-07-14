using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Vulkan;

public record RenderInfo(Buffer VertexBuffer, int VertexCount, Type VertexType, Buffer IndexBuffer, int IndexCount, IndexType IndexType, Matrix4x4 Transform) 
{
	public VertexInputBindingDescription2[] BindingDescriptions => [
		new(
			type: StructureType.VertexInputBindingDescription2Ext,
			next: default,
			description: new(
				binding: 0,
				stride: (uint)Marshal.SizeOf(this.VertexType),
				inputRate: VertexInputRate.Vertex
			),
			divisor: 1
		)
	];

	public VertexInputAttributeDescription2[] AttributeDescriptions => 
		this.VertexType
		.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
		.Index()
		.Select(x => 
			new VertexInputAttributeDescription2(
				type: StructureType.VertexInputAttributeDescription2Ext,
				next: default,
				description: new(
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
		)
		.ToArray()
	;
}
