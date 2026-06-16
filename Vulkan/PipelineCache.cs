namespace Vulkan;

public sealed class PipelineCache
{
	private readonly PipelineCacheHandle pipelineCache;

	internal PipelineCacheHandle Handle => pipelineCache;

	public override string ToString() => pipelineCache.ToString();

	internal PipelineCache(PipelineCacheHandle pipelineCache) => this.pipelineCache = pipelineCache;
}
