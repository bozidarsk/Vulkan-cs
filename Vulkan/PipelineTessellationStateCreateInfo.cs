namespace Vulkan;

public readonly struct PipelineTessellationStateCreateInfo
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly PipelineTessellationStateCreateFlags Flags;
	public readonly uint PatchControlPoints;

	public PipelineTessellationStateCreateInfo(
		StructureType type,
		nint next,
		PipelineTessellationStateCreateFlags flags,
		uint patchControlPoints
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.PatchControlPoints = patchControlPoints;
	}
}
