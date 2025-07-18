using System;
using System.Linq;

namespace Vulkan;

public readonly struct SubmitInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	private readonly uint waitSemaphoreCount;
	private readonly Handle<nint> waitSemaphores;
	private readonly Handle<PipelineStage> waitDstStageMasks;
	private readonly uint commandBufferCount;
	private readonly Handle<CommandBuffer> commandBuffers;
	private readonly uint signalSemaphoreCount;
	private readonly Handle<nint> signalSemaphores;

	public Semaphore[]? WaitSemaphores => throw new NotImplementedException(); // cannot get allocator and device params
	public PipelineStage[]? WaitDstStage => waitDstStageMasks.ToArray(waitSemaphoreCount);
	public CommandBuffer[]? CommandBuffers => commandBuffers.ToArray(commandBufferCount);
	public Semaphore[]? SignalSemaphores => throw new NotImplementedException(); // cannot get allocator and device params

	public void Dispose() 
	{
		waitSemaphores.Dispose();
		waitDstStageMasks.Dispose();
		commandBuffers.Dispose();
		signalSemaphores.Dispose();
	}

	public SubmitInfo(
		StructureType type,
		nint next,
		Semaphore[]? waitSemaphores,
		PipelineStage[]? waitDstStageMasks,
		CommandBuffer[]? commandBuffers,
		Semaphore[]? signalSemaphores
	)
	{
		if (waitSemaphores?.Length != waitDstStageMasks?.Length)
			throw new ArgumentOutOfRangeException();

		this.Type = type;
		this.Next = next;

		this.waitSemaphoreCount = (uint)(waitSemaphores?.Length ?? 0);
		this.waitSemaphores = new(waitSemaphores?.Select(x => (nint)x).ToArray());
		this.waitDstStageMasks = new(waitDstStageMasks);

		this.commandBufferCount = (uint)(commandBuffers?.Length ?? 0);
		this.commandBuffers = new(commandBuffers);

		this.signalSemaphoreCount = (uint)(signalSemaphores?.Length ?? 0);
		this.signalSemaphores = new(signalSemaphores?.Select(x => (nint)x).ToArray());
	}
}
