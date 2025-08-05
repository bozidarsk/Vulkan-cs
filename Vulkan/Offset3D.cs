namespace Vulkan;

public readonly struct Offset3D 
{
	public readonly int x;
	public readonly int y;
	public readonly int z;

	public override string ToString() => $"({x}, {y}, {z})";

	public Offset3D(int x, int y, int z) => (this.x, this.y, this.z) = (x, y, z);
}
