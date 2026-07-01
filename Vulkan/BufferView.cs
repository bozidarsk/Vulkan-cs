using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class BufferView : IDisposable
{
	private readonly BufferViewHandle bufferView;
	private readonly Device device;
	private readonly AllocationCallbacks? allocator;

	internal BufferViewHandle Handle => bufferView;

	public string Name
	{
		set
		{
			var vkSetDebugUtilsObjectNameEXT = Marshal.GetDelegateForFunctionPointer<SetDebugUtilsObjectNameDelegate>(vkGetDeviceProcAddr(device.Handle, "vkSetDebugUtilsObjectNameEXT"));

			using var nameInfo = new DebugUtilsObjectNameInfo(
				next: default,
				objectType: ObjectType.BufferView,
				objectHandle: (ulong)(nint)bufferView,
				objectName: value ?? throw new ArgumentNullException()
			);

			Result result = vkSetDebugUtilsObjectNameEXT(device.Handle, in nameInfo);
			if (result != Result.Success) throw new VulkanException(result);

			[DllImport(VK_LIB)] static extern nint vkGetDeviceProcAddr(DeviceHandle device, string name);
		}
	}

	public void Dispose()
	{
		vkDestroyBufferView(device.Handle, bufferView, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyBufferView(DeviceHandle device, BufferViewHandle bufferView, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => bufferView.ToString();

	internal BufferView(BufferViewHandle bufferView, Device device, AllocationCallbacks? allocator) =>
		(this.bufferView, this.device, this.allocator) = (bufferView, device, allocator)
	;
}
