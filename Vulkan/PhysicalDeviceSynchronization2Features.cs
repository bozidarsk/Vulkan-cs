namespace Vulkan;

public readonly struct PhysicalDeviceSynchronization2Features
{
	public readonly StructureType Type = StructureType.PhysicalDeviceSynchronization2Features;
	public readonly nint Next;
	public readonly bool32 Synchronization2;

	public PhysicalDeviceSynchronization2Features(nint next, bool synchronization2)
	{
		this.Next = next;
		this.Synchronization2 = synchronization2;
	}
}
