namespace Vulkan;

public readonly struct PipelineTessellationStateCreateInfo
{
	public readonly StructureType Type = StructureType.PipelineTessellationStateCreateInfo;
	public readonly nint Next;
	public readonly PipelineTessellationStateCreateFlags Flags;
	public readonly uint PatchControlPoints;

	public PipelineTessellationStateCreateInfo(
		nint next,
		PipelineTessellationStateCreateFlags flags,
		uint patchControlPoints
	)
	{
		this.Next = next;
		this.Flags = flags;
		this.PatchControlPoints = patchControlPoints;
	}
}
