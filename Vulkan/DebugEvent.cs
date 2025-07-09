using System;
using System.Runtime.InteropServices;

namespace Vulkan;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate Result CreateDebugUtilsMessengerDelegate(
	nint instance,
	in DebugUtilsMessengerCreateInfo createInfo,
	nint allocator,
	out nint messenger
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void DestroyDebugUtilsMessengerDelegate(
	nint instance,
	nint messenger,
	nint allocator
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool DebugUtilsMessengerCallback(
	DebugUtilsMessageSeverity messageSeverity,
	DebugUtilsMessageType messageType,
	in DebugUtilsMessengerCallbackData callbackData,
	nint userData
);

public delegate void DebugEventHandler(object? sender, DebugEventArgs args);

public sealed class DebugEventArgs : EventArgs
{
	public DebugUtilsMessageSeverity Severity { get; }
	public DebugUtilsMessageType Type { get; }
	public string? Message { get; }
	public string? MessageIdName { get; }
	public int MessageId { get; }
	public DebugUtilsLabel[]? QueueLabels { get; }
	public DebugUtilsLabel[]? CommandBufferLabels { get; }
	public DebugUtilsObjectNameInfo[]? Objects { get; }
	public nint UserData { get; }

	public DebugEventArgs(
		DebugUtilsMessageSeverity severity,
		DebugUtilsMessageType type,
		string? message,
		string? messageIdName,
		int messageId,
		DebugUtilsLabel[]? queueLabels,
		DebugUtilsLabel[]? commandBufferLabels,
		DebugUtilsObjectNameInfo[]? objects,
		nint userData
	) => (this.Severity, this.Type, this.Message, this.MessageIdName, this.MessageId, this.QueueLabels, this.CommandBufferLabels, this.Objects, this.UserData)
		=
		(severity, type, message, messageIdName, messageId, queueLabels, commandBufferLabels, objects, userData)
	;
}
