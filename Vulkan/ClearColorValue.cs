using System.Runtime.InteropServices;

namespace Vulkan;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct ClearColorValue
{
	[FieldOffset(0)] private fixed float float32[4];
	[FieldOffset(0)] private fixed int int32[4];
	[FieldOffset(0)] private fixed uint uint32[4];

	public (float r, float g, float b, float a) Float32 => (float32[0], float32[1], float32[2], float32[3]);
	public (int r, int g, int b, int a) Int32 => (int32[0], int32[1], int32[2], int32[3]);
	public (uint r, uint g, uint b, uint a) UInt32 => (uint32[0], uint32[1], uint32[2], uint32[3]);

	public ClearColorValue(float r, float g, float b, float a)
	{
		this.float32[0] = r;
		this.float32[1] = g;
		this.float32[2] = b;
		this.float32[3] = a;
	}

	public ClearColorValue(int r, int g, int b, int a)
	{
		this.int32[0] = r;
		this.int32[1] = g;
		this.int32[2] = b;
		this.int32[3] = a;
	}

	public ClearColorValue(uint r, uint g, uint b, uint a)
	{
		this.uint32[0] = r;
		this.uint32[1] = g;
		this.uint32[2] = b;
		this.uint32[3] = a;
	}
}
