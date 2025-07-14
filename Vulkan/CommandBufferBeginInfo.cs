using System;

namespace Vulkan;

public readonly struct CommandBufferBeginInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly CommandBufferUsage Usage;
	private readonly Handle<CommandBufferInheritanceInfo> inheritanceInfo;

	public CommandBufferInheritanceInfo InheritanceInfo => inheritanceInfo;

	public void Dispose() 
	{
		inheritanceInfo.Dispose();
	}

	public CommandBufferBeginInfo(StructureType type, nint next, CommandBufferUsage usage, CommandBufferInheritanceInfo? inheritanceInfo) 
	{
		this.Type = type;
		this.Next = next;
		this.Usage = usage;
		this.inheritanceInfo = (inheritanceInfo is CommandBufferInheritanceInfo x) ? new(x) : default;
	}
}
