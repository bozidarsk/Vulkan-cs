using System;

using static Vulkan.Constants;
using static Vulkan.ExtensionDelegates;

namespace Vulkan;

public readonly struct DebugUtilsMessengerCreateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly DebugUtilsMessengerCreateFlags Flags;
	public readonly DebugUtilsMessageSeverity MessageSeverity;
	public readonly DebugUtilsMessageType MessageType;
	public readonly DebugUtilsMessengerCallback UserCallback;
	public readonly nint UserData;

	public DebugUtilsMessenger CreateDebugUtilsMessanger(Instance instance, AllocationCallbacksHandle allocator) 
	{
		Result result = vkCreateDebugUtilsMessengerEXT(instance.Handle, in this, allocator, out DebugUtilsMessengerHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetDebugUtilsMessenger(instance, allocator);
	}

	public DebugUtilsMessengerCreateInfo(
		StructureType type,
		nint next,
		DebugUtilsMessengerCreateFlags flags,
		DebugUtilsMessageSeverity messageSeverity,
		DebugUtilsMessageType messageType,
		DebugUtilsMessengerCallback userCallback,
		nint userData
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.MessageSeverity = messageSeverity;
		this.MessageType = messageType;
		this.UserCallback = userCallback;
		this.UserData = userData;
	}
}
