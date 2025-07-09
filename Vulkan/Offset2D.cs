namespace Vulkan;

public readonly struct Offset2D 
{
	public readonly int x;
	public readonly int y;

	public override string ToString() => $"({x}, {y})";

	public Offset2D(int x, int y) => (this.x, this.y) = (x, y);
}
