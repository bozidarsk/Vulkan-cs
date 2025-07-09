namespace Vulkan;

public readonly struct Viewport 
{
	public readonly float x;
	public readonly float y;
	public readonly float Width;
	public readonly float Height;
	public readonly float MinDepth;
	public readonly float MaxDepth;

	public Viewport(float x, float y, float width, float height, float minDepth, float maxDepth) => 
		(this.x, this.y, this.Width, this.Height, this.MinDepth, this.MaxDepth) = (x, y, width, height, minDepth, maxDepth)
	; 
}
