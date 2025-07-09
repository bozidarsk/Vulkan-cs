namespace Vulkan;

public readonly struct Rect2D 
{
	public readonly Offset2D Offset;
	public readonly Extent2D Extent;

	public Rect2D(Offset2D offset, Extent2D extent) => (this.Offset, this.Extent) = (offset, extent);
}
