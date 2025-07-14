using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct Queue 
{
	private readonly nint handle;

	public void Submit(Fence? fence, params SubmitInfo[] infos) 
	{
		Result result = vkQueueSubmit(this, (uint)infos.Length, infos.AsPointer(), (fence != null) ? (nint)fence : default);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkQueueSubmit(Queue queue, uint count, nint pInfos, nint fence);
	}

	public void Present(PresentInfo info) 
	{
		Result result = vkQueuePresentKHR(this, in info);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkQueuePresentKHR(Queue queue, in PresentInfo info);
	}

	public void WaitIdle() 
	{
		Result result = vkQueueWaitIdle(this);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkQueueWaitIdle(Queue queue);
	}

	public static bool operator == (Queue a, Queue b) => a.handle == b.handle;
	public static bool operator != (Queue a, Queue b) => a.handle != b.handle;
	public override bool Equals(object? other) => (other is Queue x) ? x.handle == handle : false;

	public static implicit operator nint (Queue x) => x.handle;
	public static implicit operator Queue (nint x) => new(x);

	public override string ToString() => handle.ToString();
	public override int GetHashCode() => handle.GetHashCode();

	private Queue(nint handle) => this.handle = handle;
}
