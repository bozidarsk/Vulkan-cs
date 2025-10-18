global using bool32 = global::Vulkan.Helpers.Bool32;

namespace Vulkan.Helpers;

public readonly struct Bool32 
{
	private readonly uint value;

	public bool Value => value != 0;

	public static implicit operator bool (Bool32 x) => x.value == 1;
	public static implicit operator uint (Bool32 x) => x.value;
	public static implicit operator Bool32 (bool x) => new((uint)(x ? 1 : 0));
	public static implicit operator Bool32 (uint x) => new(x);

	public override string ToString() => (value == 1).ToString();

	private Bool32(uint value) => this.value = value;
}
