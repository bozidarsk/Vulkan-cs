namespace Vulkan;

public sealed class PipelineCache
{
	private readonly PipelineCacheHandle pipelineCache;

	internal PipelineCacheHandle Handle => pipelineCache;

	internal PipelineCache(PipelineCacheHandle pipelineCache) => this.pipelineCache = pipelineCache;
}
