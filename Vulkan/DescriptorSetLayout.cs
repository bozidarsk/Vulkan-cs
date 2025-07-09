namespace Vulkan;

public readonly struct DescriptorSetLayout 
{
	private readonly nint handle;

	public static bool operator == (DescriptorSetLayout a, DescriptorSetLayout b) => a.handle == b.handle;
	public static bool operator != (DescriptorSetLayout a, DescriptorSetLayout b) => a.handle != b.handle;
	public override bool Equals(object? other) => (other is DescriptorSetLayout x) ? x.handle == handle : false;

	public static implicit operator nint (DescriptorSetLayout x) => x.handle;
	public static implicit operator DescriptorSetLayout (nint x) => new(x);

	public override string ToString() => handle.ToString();
	public override int GetHashCode() => handle.GetHashCode();

	private DescriptorSetLayout(nint handle) => this.handle = handle;
}
