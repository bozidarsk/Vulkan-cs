using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct DebugUtilsMessengerCreateInfo
{
	public readonly StructureType Type = StructureType.DebugUtilsMessengerCreateInfoExt;
	public readonly nint Next;
	public readonly DebugUtilsMessengerCreateFlags Flags;
	public readonly DebugUtilsMessageSeverity MessageSeverity;
	public readonly DebugUtilsMessageType MessageType;
	public readonly DebugUtilsMessengerCallback UserCallback;
	public readonly nint UserData;

	public DebugUtilsMessenger CreateDebugUtilsMessanger(Instance instance, AllocationCallbacks? allocator)
	{
		var vkCreateDebugUtilsMessengerEXT = Marshal.GetDelegateForFunctionPointer<CreateDebugUtilsMessengerExtensionDelegate>(vkGetInstanceProcAddr(instance.Handle, "vkCreateDebugUtilsMessengerEXT"));

		Result result = vkCreateDebugUtilsMessengerEXT(instance.Handle, in this, allocator?.Handle ?? default, out DebugUtilsMessengerHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(handle, instance, allocator);

		[DllImport(VK_LIB)] static extern nint vkGetInstanceProcAddr(InstanceHandle instance, string name);
	}

	public DebugUtilsMessengerCreateInfo(
		nint next,
		DebugUtilsMessengerCreateFlags flags,
		DebugUtilsMessageSeverity messageSeverity,
		DebugUtilsMessageType messageType,
		DebugUtilsMessengerCallback userCallback,
		nint userData
	)
	{
		this.Next = next;
		this.Flags = flags;
		this.MessageSeverity = messageSeverity;
		this.MessageType = messageType;
		this.UserCallback = userCallback;
		this.UserData = userData;
	}
}
