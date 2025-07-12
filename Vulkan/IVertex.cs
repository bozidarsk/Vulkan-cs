using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vulkan;

public interface IVertex 
{
}

public static class IVertexExtensions 
{
	private static Format GetFormat(string type) => type switch 
	{
		"Vulkan.Color" => Format.R32G32B32A32SFloat,
		"Vulkan.Vector2" => Format.R32G32SFloat,
		"Vulkan.Vector3" => Format.R32G32B32SFloat,
		"Vulkan.Vector4" => Format.R32G32B32A32SFloat,
		"Vulkan.Vector2Int" => Format.R32G32SInt,
		"Vulkan.Vector3Int" => Format.R32G32B32SInt,
		"Vulkan.Vector4Int" => Format.R32G32B32A32SInt,
		_ => throw new ArgumentOutOfRangeException()
	};

	public static int GetStride(this IVertex iVertex) => Marshal.SizeOf(iVertex.GetType());

	public static VertexInputBindingDescription GetBindingDescription(this IVertex iVertex) => new(
		binding: 0,
		stride: (uint)iVertex.GetStride(),
		inputRate: VertexInputRate.Vertex
	);

	public static VertexInputAttributeDescription[] GetAttributeDescriptions(this IVertex iVertex) => 
		iVertex
		.GetType()
		.GetFields()
		.Index()
		.Select(x => 
			new VertexInputAttributeDescription(
				location: (uint)x.Item1,
				binding: 0,
				format: GetFormat(x.Item2.FieldType.FullName!),
				offset: (uint)Marshal.OffsetOf(x.Item2.DeclaringType!, x.Item2.Name)
			)
		)
		.ToArray()
	;
}