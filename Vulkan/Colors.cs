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

	public static explicit operator Color (Vector2 a) => new(a.x, a.y, 0f, 0f);
	public static explicit operator Color (Vector2Int a) => new((float)a.x, (float)a.y, 0f, 0f);
	public static explicit operator Color (Vector3 a) => new(a.x, a.y, a.z, 0f);
	public static explicit operator Color (Vector3Int a) => new((float)a.x, (float)a.y, (float)a.z, 0f);
	public static explicit operator Color (Vector4 a) => new(a.x, a.y, a.z, a.w);
	public static explicit operator Color (Vector4Int a) => new((float)a.x, (float)a.y, (float)a.z, (float)a.w);

	public static explicit operator Vector2 (Color a) => new(a.r, a.g);
	public static explicit operator Vector2Int (Color a) => new((int)a.r, (int)a.g);
	public static explicit operator Vector3 (Color a) => new(a.r, a.g, a.b);
	public static explicit operator Vector3Int (Color a) => new((int)a.r, (int)a.g, (int)a.b);
	public static explicit operator Vector4 (Color a) => new(a.r, a.g, a.b, a.a);
	public static explicit operator Vector4Int (Color a) => new((int)a.r, (int)a.g, (int)a.b, (int)a.a);

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
