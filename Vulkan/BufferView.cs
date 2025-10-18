using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class BufferView : IDisposable
{
	private readonly BufferViewHandle bufferView;
	private readonly Device device;
	private readonly AllocationCallbacksHandle allocator;

	internal BufferViewHandle Handle => bufferView;

	public void Dispose() 
	{
		vkDestroyBufferView(device.Handle, bufferView, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyBufferView(DeviceHandle device, BufferViewHandle bufferView, AllocationCallbacksHandle allocator);
	}

	internal BufferView(BufferViewHandle bufferView, Device device, AllocationCallbacksHandle allocator) => 
		(this.bufferView, this.device, this.allocator) = (bufferView, device, allocator)
	;
}
