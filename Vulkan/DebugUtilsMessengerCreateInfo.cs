using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

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

	public DebugUtilsMessenger CreateDebugUtilsMessanger(Instance instance, Handle<AllocationCallbacks> allocator) 
	{
		var func = Marshal.GetDelegateForFunctionPointer<CreateDebugUtilsMessengerDelegate>(vkGetInstanceProcAddr((nint)instance, "vkCreateDebugUtilsMessengerEXT"));

		Result result = func((nint)instance, in this, allocator, out nint debugUtilsMessengerHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(instance, debugUtilsMessengerHandle, allocator);

		[DllImport(VK_LIB)] static extern nint vkGetInstanceProcAddr(nint instance, string name);
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
