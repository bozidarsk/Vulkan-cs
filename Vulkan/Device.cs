using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Device : IDisposable
{
	private readonly DeviceHandle device;
	private readonly AllocationCallbacksHandle allocator;

	internal DeviceHandle Handle => device;

	public unsafe void UpdateDescriptorSets(WriteDescriptorSet[]? writes, WriteDescriptorSet[]? copies) 
	{
		vkUpdateDescriptorSets(
			device,
			(uint)(writes?.Length ?? 0),
			ref (writes != null) ? ref MemoryMarshal.GetArrayDataReference(writes) : ref Unsafe.AsRef<WriteDescriptorSet>(default),
			(uint)(copies?.Length ?? 0),
			ref (copies != null) ? ref MemoryMarshal.GetArrayDataReference(copies) : ref Unsafe.AsRef<WriteDescriptorSet>(default)
		);

		[DllImport(VK_LIB)] static extern void vkUpdateDescriptorSets(
			DeviceHandle device,
			uint descriptorWriteCount,
			ref WriteDescriptorSet pDescriptorWrites,
			uint descriptorCopyCount,
			ref WriteDescriptorSet pDescriptorCopies
		);
	}

	public Queue GetQueue(uint queueFamilyIndex, uint queueIndex) 
	{
		vkGetDeviceQueue(device, queueFamilyIndex, queueIndex, out Queue queue);
		return queue;

		[DllImport(VK_LIB)] static extern void vkGetDeviceQueue(DeviceHandle device, uint queueFamilyIndex, uint queueIndex, out Queue queue);
	}

	public void WaitIdle() 
	{
		vkDeviceWaitIdle(device);

		[DllImport(VK_LIB)] static extern void vkDeviceWaitIdle(DeviceHandle device);
	}

	public void Dispose() 
	{
		vkDestroyDevice(device, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroyDevice(DeviceHandle device, AllocationCallbacksHandle allocator);
	}

	internal Device(DeviceHandle device, AllocationCallbacksHandle allocator) => 
		(this.device, this.allocator) = (device, allocator)
	;
}
