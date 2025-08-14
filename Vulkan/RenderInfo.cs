using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using Vulkan.ShaderCompiler;

namespace Vulkan;

public sealed record RenderInfo(Buffer VertexBuffer, int VertexCount, Type VertexType, Buffer IndexBuffer, int IndexCount, IndexType IndexType, ShaderInfo[] Shaders) 
{
	public CullMode? CullMode => this.Shaders.Select(x => x.CullMode).Where(x => x != null).FirstOrDefault();
	public FrontFace? FrontFace => this.Shaders.Select(x => x.FrontFace).Where(x => x != null).FirstOrDefault();
	public BlendFactor? SourceBlendFactor => this.Shaders.Select(x => x.SourceBlendFactor).Where(x => x != null).FirstOrDefault();
	public BlendFactor? DestinationBlendFactor => this.Shaders.Select(x => x.DestinationBlendFactor).Where(x => x != null).FirstOrDefault();

	public VertexInputBindingDescription[] BindingDescriptions => [
		new(
			binding: 0,
			stride: (uint)Marshal.SizeOf(this.VertexType),
			inputRate: VertexInputRate.Vertex
		)
	];

	public VertexInputAttributeDescription[] AttributeDescriptions => 
		this.VertexType
		.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
		.Index()
		.Select(x => 
			new VertexInputAttributeDescription(
				location: (uint)x.Item1,
				binding: 0,
				format: x.Item2.FieldType.FullName! switch 
				{
					"System.Single" => Format.R32SFloat,
					"System.Double" => Format.R64SFloat,
					"Vulkan.Color" => Format.R32G32B32A32SFloat,
					"Vulkan.Vector2" => Format.R32G32SFloat,
					"Vulkan.Vector3" => Format.R32G32B32SFloat,
					"Vulkan.Vector4" => Format.R32G32B32A32SFloat,
					"Vulkan.Vector2Int" => Format.R32G32SInt,
					"Vulkan.Vector3Int" => Format.R32G32B32SInt,
					"Vulkan.Vector4Int" => Format.R32G32B32A32SInt,
					_ => throw new ArgumentOutOfRangeException(nameof(Type), $"Cannot map field type '{x.Item2.FieldType.FullName!}' to a format.")
				},
				offset: (uint)Marshal.OffsetOf(x.Item2.DeclaringType!, x.Item2.Name)
			)
		)
		.ToArray()
	;

	public VertexInputBindingDescription2[] BindingDescriptions2 => this.BindingDescriptions.Select(x => 
		new VertexInputBindingDescription2(
			type: StructureType.VertexInputBindingDescription2Ext,
			next: default,
			description: x,
			divisor: 1
		)
	).ToArray();

	public VertexInputAttributeDescription2[] AttributeDescriptions2 => this.AttributeDescriptions.Select(x => 
		new VertexInputAttributeDescription2(
			type: StructureType.VertexInputAttributeDescription2Ext,
			next: default,
			description: x
		)
	).ToArray();
}
