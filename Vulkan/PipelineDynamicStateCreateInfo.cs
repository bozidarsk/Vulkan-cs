using System;

namespace Vulkan;

public readonly struct PipelineDynamicStateCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.PipelineDynamicStateCreateInfo;
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
		nint next,
		PipelineDynamicStateCreateFlags flags,
		DynamicState[]? dynamicStates
	)
	{
		this.Next = next;
		this.Flags = flags;

		this.dynamicStateCount = (uint)(dynamicStates?.Length ?? 0);
		this.dynamicStates = new(dynamicStates);
	}
}
