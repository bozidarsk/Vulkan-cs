using System;

namespace Vulkan;

public sealed class DebugUtilsMessengerEventArgs : EventArgs
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

	public DebugUtilsMessengerEventArgs(
		DebugUtilsMessageSeverity severity,
		DebugUtilsMessageType type,
		string? message,
		string? messageIdName,
		int messageId,
		DebugUtilsLabel[]? queueLabels,
		DebugUtilsLabel[]? commandBufferLabels,
		DebugUtilsObjectNameInfo[]? objects,
		nint userData
	)
	{
		this.Severity = severity;
		this.Type = type;
		this.Message = message;
		this.MessageIdName = messageIdName;
		this.MessageId = messageId;
		this.QueueLabels = queueLabels;
		this.CommandBufferLabels = commandBufferLabels;
		this.Objects = objects;
		this.UserData = userData;
	}
}
