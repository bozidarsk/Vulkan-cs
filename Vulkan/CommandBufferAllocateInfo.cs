using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct CommandBufferAllocateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	private readonly nint commandPool;
	public readonly CommandBufferLevel Level;
	public readonly uint CommandBufferCount;

	public CommandPool CommandPool => throw new NotImplementedException(); // cannot get allocator and device params

	public CommandBuffer[] CreateCommandBuffers(Device device) 
	{
		CommandBuffer[] commandBuffers = new CommandBuffer[this.CommandBufferCount];

		Result result = vkAllocateCommandBuffers((nint)device, in this, commandBuffers.AsPointer());
		if (result != Result.Success) throw new VulkanException(result);

		return commandBuffers;

		[DllImport(VK_LIB)] static extern Result vkAllocateCommandBuffers(nint device, in CommandBufferAllocateInfo createInfo, nint pCommandBuffers);
	}

	public CommandBufferAllocateInfo(
		StructureType type,
		nint next,
		CommandPool commandPool,
		CommandBufferLevel level,
		uint commandBufferCount
	)
	{
		this.Type = type;
		this.Next = next;
		this.commandPool = (nint)commandPool;
		this.Level = level;
		this.CommandBufferCount = commandBufferCount;
	}
}
