using System;

namespace Vulkan;

public readonly struct CommandBufferBeginInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly CommandBufferUsageFlags Flags;
	private readonly Handle<CommandBufferInheritanceInfo> inheritanceInfo;

	public CommandBufferInheritanceInfo InheritanceInfo => inheritanceInfo;

	public void Dispose() 
	{
		inheritanceInfo.Dispose();
	}

	public CommandBufferBeginInfo(StructureType type, nint next, CommandBufferUsageFlags flags, CommandBufferInheritanceInfo? inheritanceInfo) 
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.inheritanceInfo = (inheritanceInfo is CommandBufferInheritanceInfo x) ? new(x) : default;
	}
}
