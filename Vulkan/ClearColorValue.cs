namespace Vulkan;

public readonly struct ClearColorValue 
{
	public readonly Color Float32;
	public readonly ColorInt Int32;
	public readonly ColorUInt UInt32;

	public ClearColorValue(Color float32, ColorInt int32, ColorUInt uint32) 
	{
		this.Float32 = float32;
		this.Int32 = int32;
		this.UInt32 = uint32;
	}
}
