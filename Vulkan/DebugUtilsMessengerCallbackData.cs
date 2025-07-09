#pragma warning disable CS0649

using System;

namespace Vulkan;

public readonly struct DebugUtilsMessengerCallbackData : IDisposable
{
	public readonly StructureType Type;
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
		messageIdName.Dispose();
		message.Dispose();
		queueLabels.Dispose();
		cmdBufLabels.Dispose();
		objects.Dispose();
	}
}
