using System;

namespace Vulkan;

public readonly struct PipelineVertexInputStateCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly PipelineVertexInputStateCreateFlags Flags;
	public readonly uint vertexBindingDescriptionCount;
	private readonly Handle<VertexInputBindingDescription> vertexBindingDescriptions;
	public readonly uint vertexAttributeDescriptionCount;
	private readonly Handle<VertexInputAttributeDescription> vertexAttributeDescriptions;

	public VertexInputBindingDescription[]? VertexBindingDescriptions => vertexBindingDescriptions.ToArray(vertexBindingDescriptionCount);
	public VertexInputAttributeDescription[]? VertexAttributeDescriptions => vertexAttributeDescriptions.ToArray(vertexAttributeDescriptionCount);

	public void Dispose() 
	{
		vertexBindingDescriptions.Dispose();
		vertexAttributeDescriptions.Dispose();
	}

	public PipelineVertexInputStateCreateInfo(StructureType type, nint next, PipelineVertexInputStateCreateFlags flags, VertexInputBindingDescription[]? vertexBindingDescriptions, VertexInputAttributeDescription[]? vertexAttributeDescriptions) 
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;

		this.vertexBindingDescriptionCount = (uint)(vertexBindingDescriptions?.Length ?? 0);
		this.vertexBindingDescriptions = new(vertexBindingDescriptions);

		this.vertexAttributeDescriptionCount = (uint)(vertexAttributeDescriptions?.Length ?? 0);
		this.vertexAttributeDescriptions = new(vertexAttributeDescriptions);
	}
}
