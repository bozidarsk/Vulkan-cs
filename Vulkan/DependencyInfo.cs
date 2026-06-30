using System;

namespace Vulkan;

public readonly struct DependencyInfo : IDisposable
{
	public readonly StructureType Type = StructureType.DependencyInfo;
	public readonly nint Next;
	public readonly DependencyFlags DependencyFlags;
	private readonly uint memoryBarrierCount;
	private readonly Box<MemoryBarrier2> memoryBarriers;
	private readonly uint bufferMemoryBarrierCount;
	private readonly Box<BufferMemoryBarrier2> bufferMemoryBarriers;
	private readonly uint imageMemoryBarrierCount;
	private readonly Box<ImageMemoryBarrier2> imageMemoryBarriers;

	public MemoryBarrier2[]? MemoryBarriers => memoryBarriers.ToArray(memoryBarrierCount);
	public BufferMemoryBarrier2[]? BufferMemoryBarriers => bufferMemoryBarriers.ToArray(bufferMemoryBarrierCount);
	public ImageMemoryBarrier2[]? ImageMemoryBarriers => imageMemoryBarriers.ToArray(imageMemoryBarrierCount);

	public void Dispose()
	{
		memoryBarriers.Dispose();
		bufferMemoryBarriers.Dispose();
		imageMemoryBarriers.Dispose();
	}

	public DependencyInfo(
		nint next,
		DependencyFlags dependencyFlags,
		MemoryBarrier2[]? memoryBarriers,
		BufferMemoryBarrier2[]? bufferMemoryBarriers,
		ImageMemoryBarrier2[]? imageMemoryBarriers
	)
	{
		this.Next = next;
		this.DependencyFlags = dependencyFlags;

		this.memoryBarrierCount = (uint)(memoryBarriers?.Length ?? 0);
		this.memoryBarriers = new(memoryBarriers);

		this.bufferMemoryBarrierCount = (uint)(bufferMemoryBarriers?.Length ?? 0);
		this.bufferMemoryBarriers = new(bufferMemoryBarriers);

		this.imageMemoryBarrierCount = (uint)(imageMemoryBarriers?.Length ?? 0);
		this.imageMemoryBarriers = new(imageMemoryBarriers);
	}
}
