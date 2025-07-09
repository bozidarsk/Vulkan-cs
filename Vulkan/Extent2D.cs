namespace Vulkan;

public readonly struct Extent2D 
{
	public readonly uint Width;
	public readonly uint Height;

	public override string ToString() => $"({Width}, {Height})";

	public Extent2D(uint width, uint height) => (this.Width, this.Height) = (width, height);
}
