namespace Vulkan;

public unsafe struct ClearColorValue
{
	private fixed float float32[4];
	private fixed int int32[4];
	private fixed uint uint32[4];

	public (float r, float g, float b, float a) Float32 => (float32[0], float32[1], float32[2], float32[3]);
	public (int r, int g, int b, int a) Int32 => (int32[0], int32[1], int32[2], int32[3]);
	public (uint r, uint g, uint b, uint a) UInt32 => (uint32[0], uint32[1], uint32[2], uint32[3]);

	public ClearColorValue((float r, float g, float b, float a) float32, (int r, int g, int b, int a) int32, (uint r, uint g, uint b, uint a) uint32)
	{
		(this.float32[0], this.float32[1], this.float32[2], this.float32[3]) = float32;
		(this.int32[0], this.int32[1], this.int32[2], this.int32[3]) = int32;
		(this.uint32[0], this.uint32[1], this.uint32[2], this.uint32[3]) = uint32;
	}
}
