using System;

namespace Vulkan;

public readonly struct DeviceQueueCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly DeviceQueueCreateFlags Flags;
	public readonly uint QueueFamilyIndex;
	private readonly uint queueCount;
	private readonly Handle<float> queuePriorities;

	public float[]? QueuePriorities => queuePriorities.ToArray(queueCount);

	public void Dispose() 
	{
		queuePriorities.Dispose();
	}

	public DeviceQueueCreateInfo(StructureType type, nint next, DeviceQueueCreateFlags flags, uint queueFamilyIndex, float[]? queuePriorities) 
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.QueueFamilyIndex = queueFamilyIndex;

		this.queueCount = (uint)(queuePriorities?.Length ?? 0);
		this.queuePriorities = new(queuePriorities);
	}
}
