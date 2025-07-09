using System;
using System.Linq;

namespace Vulkan;

public readonly struct PresentInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	private readonly uint waitSemaphoreCount;
	private readonly Handle<nint> waitSemaphores;
	private readonly uint swapchainCount;
	private readonly Handle<nint> swapchains;
	private readonly Handle<uint> imageIndices;
	private readonly Handle<Result> results;

	public Semaphore[]? WaitSemaphores => throw new NotImplementedException(); // cannot get device and allocator params
	public Swapchain[]? Swapchains => throw new NotImplementedException(); // cannot get device and allocator params
	public uint[]? ImageIndices => imageIndices.ToArray(swapchainCount);
	public Result[]? Results => results.ToArray(swapchainCount);

	public void Dispose() 
	{
		waitSemaphores.Dispose();
		swapchains.Dispose();
		imageIndices.Dispose();
		results.Dispose();
	}

	public PresentInfo(
		StructureType type,
		nint next,
		Semaphore[]? waitSemaphores,
		Swapchain[]? swapchains,
		uint[]? imageIndices,
		Result[]? results
	)
	{
		if (swapchains?.Length != imageIndices?.Length)
			throw new ArgumentOutOfRangeException();

		this.Type = type;
		this.Next = next;

		this.waitSemaphoreCount = (uint)(waitSemaphores?.Length ?? 0);
		this.waitSemaphores = new(waitSemaphores?.Select(x => (nint)x).ToArray());

		this.swapchainCount = (uint)(swapchains?.Length ?? 0);
		this.swapchains = new(swapchains?.Select(x => (nint)x).ToArray());
		this.imageIndices = new(imageIndices);
		this.results = new(results);
	}
}
