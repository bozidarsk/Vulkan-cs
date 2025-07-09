namespace Vulkan;

public readonly struct Image 
{
	private readonly nint handle;

	public static bool operator == (Image a, Image b) => a.handle == b.handle;
	public static bool operator != (Image a, Image b) => a.handle != b.handle;
	public override bool Equals(object? other) => (other is Image x) ? x.handle == handle : false;

	public static implicit operator nint (Image x) => x.handle;

	public override string ToString() => handle.ToString();
	public override int GetHashCode() => handle.GetHashCode();
}
