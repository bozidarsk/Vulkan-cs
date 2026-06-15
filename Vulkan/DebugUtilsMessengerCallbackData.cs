#pragma warning disable CS0649

using System;
using System.Linq;

namespace Vulkan;

public readonly struct DebugUtilsMessengerCallbackData : IDisposable
{
	public readonly StructureType Type = StructureType.DebugUtilsMessengerCallbackDataExt;
	public readonly nint Next;
	public readonly DebugUtilsMessengerCallbackDataFlags Flags;
	private readonly cstring messageIdName;
	public readonly int MessageIdNumber;
	private readonly cstring message;
	private readonly uint queueLabelCount;
	private readonly Handle<DebugUtilsLabel> queueLabels;
	private readonly uint cmdBufLabelCount;
	private readonly Handle<DebugUtilsLabel> cmdBufLabels;
	private readonly uint objectCount;
	private readonly Handle<DebugUtilsObjectNameInfo> objects;

	public string? MessageIdName => messageIdName;
	public string? Message => message;
	public DebugUtilsLabel[]? QueueLabels => queueLabels.ToArray(queueLabelCount);
	public DebugUtilsLabel[]? CommandBufferLabels => cmdBufLabels.ToArray(cmdBufLabelCount);
	public DebugUtilsObjectNameInfo[]? Objects => objects.ToArray(objectCount);

	public void Dispose()
	{
		foreach (var x in queueLabels.ToArray(queueLabelCount) ?? [])
			x.Dispose();

		foreach (var x in cmdBufLabels.ToArray(cmdBufLabelCount) ?? [])
			x.Dispose();

		foreach (var x in objects.ToArray(objectCount) ?? [])
			x.Dispose();

		messageIdName.Dispose();
		message.Dispose();
		queueLabels.Dispose();
		cmdBufLabels.Dispose();
		objects.Dispose();
	}

	public DebugUtilsMessengerCallbackData(
		nint next,
		DebugUtilsMessengerCallbackDataFlags flags,
		string? messageIdName,
		int messageIdNumber,
		string? message,
		DebugUtilsLabel[]? queueLabels,
		DebugUtilsLabel[]? cmdBufLabels,
		DebugUtilsObjectNameInfo[]? objects
	)
	{
		this.Next = next;
		this.Flags = flags;
		this.messageIdName = messageIdName;
		this.MessageIdNumber = messageIdNumber;
		this.message = message;

		this.queueLabelCount = (uint)(queueLabels?.Length ?? 0);
		this.queueLabels = new(queueLabels);

		this.cmdBufLabelCount = (uint)(cmdBufLabels?.Length ?? 0);
		this.cmdBufLabels = new(cmdBufLabels);

		this.objectCount = (uint)(objects?.Length ?? 0);
		this.objects = new(objects);
	}
}
