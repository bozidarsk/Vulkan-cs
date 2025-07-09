namespace Vulkan;

public readonly struct PipelineCache 
{
	private readonly nint handle;

	public static bool operator == (PipelineCache a, PipelineCache b) => a.handle == b.handle;
	public static bool operator != (PipelineCache a, PipelineCache b) => a.handle != b.handle;
	public override bool Equals(object? other) => (other is PipelineCache x) ? x.handle == handle : false;

	public static implicit operator nint (PipelineCache x) => x.handle;
	public static implicit operator PipelineCache (nint x) => new(x);

	public override string ToString() => handle.ToString();
	public override int GetHashCode() => handle.GetHashCode();

	private PipelineCache(nint handle) => this.handle = handle;
}
