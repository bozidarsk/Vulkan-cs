using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Queue
{
	private readonly QueueHandle queue;

	internal QueueHandle Handle => queue;

	public void Submit(Fence? fence, params SubmitInfo[] infos)
	{
		Result result = vkQueueSubmit(queue, (uint)infos.Length, ref MemoryMarshal.GetArrayDataReference(infos), (fence != null) ? fence.Handle : default);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkQueueSubmit(QueueHandle queue, uint count, ref SubmitInfo pInfos, FenceHandle fence);
	}

	public void Present(PresentInfo info)
	{
		Result result = vkQueuePresentKHR(queue, in info);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkQueuePresentKHR(QueueHandle queue, in PresentInfo info);
	}

	public void WaitIdle()
	{
		Result result = vkQueueWaitIdle(queue);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkQueueWaitIdle(QueueHandle queue);
	}

	public override string ToString() => queue.ToString();

	internal Queue(QueueHandle queue) => this.queue = queue;
}
