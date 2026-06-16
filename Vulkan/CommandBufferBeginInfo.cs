using System;

namespace Vulkan;

public readonly struct CommandBufferBeginInfo : IDisposable
{
	public readonly StructureType Type = StructureType.CommandBufferBeginInfo;
	public readonly nint Next;
	public readonly CommandBufferUsage Usage;
	private readonly Box<CommandBufferInheritanceInfo> inheritanceInfo;

	public CommandBufferInheritanceInfo InheritanceInfo => inheritanceInfo;

	public void Dispose()
	{
		inheritanceInfo.Dispose();
	}

	public CommandBufferBeginInfo(nint next, CommandBufferUsage usage, CommandBufferInheritanceInfo? inheritanceInfo)
	{
		this.Next = next;
		this.Usage = usage;
		this.inheritanceInfo = (inheritanceInfo is CommandBufferInheritanceInfo x) ? new(x) : default;
	}
}
