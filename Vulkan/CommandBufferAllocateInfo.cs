using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct CommandBufferAllocateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	private readonly CommandPoolHandle commandPool;
	public readonly CommandBufferLevel Level;
	public readonly uint CommandBufferCount;

	public CommandPool CommandPool => throw new NotImplementedException(); // cannot get allocator and device params

	public CommandBuffer[] CreateCommandBuffers(Device device, CommandPool commandPool) 
	{
		var commandBuffers = new CommandBufferHandle[this.CommandBufferCount];

		Result result = vkAllocateCommandBuffers(device.Handle, in this, ref MemoryMarshal.GetArrayDataReference(commandBuffers));
		if (result != Result.Success) throw new VulkanException(result);

		return commandBuffers.Select(x => x.GetCommandBuffer(device, commandPool)).ToArray();

		[DllImport(VK_LIB)] static extern Result vkAllocateCommandBuffers(DeviceHandle device, in CommandBufferAllocateInfo createInfo, ref CommandBufferHandle pCommandBuffers);
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
		this.commandPool = commandPool.Handle;
		this.Level = level;
		this.CommandBufferCount = commandBufferCount;
	}
}
