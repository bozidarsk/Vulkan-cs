namespace Vulkan;

public struct Color 
{
	public float r, g, b, a;

	public static readonly Color Black = new(0f, 0f, 0f, 0f);
	public static readonly Color White = new(1f, 1f, 1f, 1f);

	public static Color operator * (Color a, float x) => new(a.r * x, a.g * x, a.b * x, a.a * x);
	public static Color operator / (Color a, float x) => new(a.r / x, a.g / x, a.b / x, a.a / x);
	public static Color operator * (float x, Color a) => new(a.r * x, a.g * x, a.b * x, a.a * x);
	public static Color operator / (float x, Color a) => new(a.r / x, a.g / x, a.b / x, a.a / x);
	public static Color operator + (Color a, Color b) => new(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
	public static Color operator - (Color a, Color b) => new(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);

	public override string ToString() => $"({r}, {g}, {b}, {a})";

	public Color(float r, float g, float b, float a = 1f) => (this.r, this.g, this.b, this.a) = (r, g, b, a);
}

public struct ColorInt 
{
	public int r, g, b, a;

	public static ColorInt operator * (ColorInt a, int x) => new(a.r * x, a.g * x, a.b * x, a.a * x);
	public static ColorInt operator / (ColorInt a, int x) => new(a.r / x, a.g / x, a.b / x, a.a / x);
	public static ColorInt operator * (int x, ColorInt a) => new(a.r * x, a.g * x, a.b * x, a.a * x);
	public static ColorInt operator / (int x, ColorInt a) => new(a.r / x, a.g / x, a.b / x, a.a / x);
	public static ColorInt operator + (ColorInt a, ColorInt b) => new(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
	public static ColorInt operator - (ColorInt a, ColorInt b) => new(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);

	public override string ToString() => $"({r}, {g}, {b}, {a})";

	public ColorInt(int r, int g, int b, int a) => (this.r, this.g, this.b, this.a) = (r, g, b, a);
}

public struct ColorUInt 
{
	public uint r, g, b, a;

	public static ColorUInt operator * (ColorUInt a, uint x) => new(a.r * x, a.g * x, a.b * x, a.a * x);
	public static ColorUInt operator / (ColorUInt a, uint x) => new(a.r / x, a.g / x, a.b / x, a.a / x);
	public static ColorUInt operator * (uint x, ColorUInt a) => new(a.r * x, a.g * x, a.b * x, a.a * x);
	public static ColorUInt operator / (uint x, ColorUInt a) => new(a.r / x, a.g / x, a.b / x, a.a / x);
	public static ColorUInt operator + (ColorUInt a, ColorUInt b) => new(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
	public static ColorUInt operator - (ColorUInt a, ColorUInt b) => new(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);

	public override string ToString() => $"({r}, {g}, {b}, {a})";

	public ColorUInt(uint r, uint g, uint b, uint a) => (this.r, this.g, this.b, this.a) = (r, g, b, a);
}
