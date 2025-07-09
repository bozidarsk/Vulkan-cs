namespace Vulkan;

public readonly struct Extent3D 
{
	public readonly uint Width;
	public readonly uint Height;
	public readonly uint Depth;

	public override string ToString() => $"({Width}, {Height}, {Depth})";

	public Extent3D(uint width, uint height, uint depth) => (this.Width, this.Height, this.Depth) = (width, height, depth);
}
