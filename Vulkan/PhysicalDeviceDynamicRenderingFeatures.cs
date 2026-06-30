namespace Vulkan;

public readonly struct PhysicalDeviceDynamicRenderingFeatures
{
	public readonly StructureType Type = StructureType.PhysicalDeviceDynamicRenderingFeatures;
	public readonly nint Next;
	public readonly bool32 DynamicRendering;

	public PhysicalDeviceDynamicRenderingFeatures(nint next, bool dynamicRendering)
	{
		this.Next = next;
		this.DynamicRendering = dynamicRendering;
	}
}
