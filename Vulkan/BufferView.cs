using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class BufferView : IDisposable
{
	private readonly Device device;
	private readonly nint bufferView;
	private readonly Handle<AllocationCallbacks> allocator;

	public static explicit operator nint (BufferView x) => x.bufferView;

	public void Dispose() 
	{
		vkDestroyBufferView((nint)device, bufferView, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyBufferView(nint device, nint bufferView, nint allocator);
	}

	private BufferView(Device device, nint bufferView) => (this.device, this.bufferView) = (device, bufferView);
	internal BufferView(Device device, nint bufferView, Handle<AllocationCallbacks> allocator) : this(device, bufferView) => this.allocator = allocator;
}

