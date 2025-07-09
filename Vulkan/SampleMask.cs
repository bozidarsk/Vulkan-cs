namespace Vulkan;

public readonly struct SampleMask 
{
	private readonly uint value;

	public static bool operator == (SampleMask a, SampleMask b) => a.value == b.value;
	public static bool operator != (SampleMask a, SampleMask b) => a.value != b.value;
	public override bool Equals(object? other) => (other is SampleMask x) ? x.value == value : false;

	public static implicit operator uint (SampleMask x) => x.value;
	public static implicit operator SampleMask (uint x) => new(x);

	public override string ToString() => value.ToString();
	public override int GetHashCode() => value.GetHashCode();

	private SampleMask(uint value) => this.value = value;
}


