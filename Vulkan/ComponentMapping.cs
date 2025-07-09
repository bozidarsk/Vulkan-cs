namespace Vulkan;

public readonly struct ComponentMapping 
{
	public readonly ComponentSwizzle r;
	public readonly ComponentSwizzle g;
	public readonly ComponentSwizzle b;
	public readonly ComponentSwizzle a;

	public ComponentMapping(ComponentSwizzle r, ComponentSwizzle g, ComponentSwizzle b, ComponentSwizzle a) => 
		(this.r, this.g, this.b, this.a) = (r, g, b, a)
	;
}
