namespace Vulkan;

public readonly struct DeviceSize 
{
	private readonly ulong value;

	public static bool operator == (DeviceSize a, DeviceSize b) => a.value == b.value;
	public static bool operator != (DeviceSize a, DeviceSize b) => a.value != b.value;
	public override bool Equals(object? other) => (other is DeviceSize x) ? x.value == value : false;

	public static implicit operator ulong (DeviceSize x) => x.value;
	public static implicit operator DeviceSize (ulong x) => new(x);

	public override string ToString() => value.ToString();
	public override int GetHashCode() => value.GetHashCode();

	private DeviceSize(ulong value) => this.value = value;
}

