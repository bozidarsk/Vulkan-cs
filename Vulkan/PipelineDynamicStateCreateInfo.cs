using System;

namespace Vulkan;

public readonly struct PipelineDynamicStateCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly PipelineDynamicStateCreateFlags Flags;
	private readonly uint dynamicStateCount;
	private readonly Handle<DynamicState> dynamicStates;

	public DynamicState[]? DynamicStates => dynamicStates.ToArray(dynamicStateCount);

	public void Dispose() 
	{
		dynamicStates.Dispose();
	}

	public PipelineDynamicStateCreateInfo(
		StructureType type,
		nint next,
		PipelineDynamicStateCreateFlags flags,
		DynamicState[]? dynamicStates
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;

		this.dynamicStateCount = (uint)(dynamicStates?.Length ?? 0);
		this.dynamicStates = new(dynamicStates);
	}
}
