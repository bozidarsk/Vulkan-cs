using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Instance : IDisposable
{
	private readonly InstanceHandle instance;
	private readonly AllocationCallbacksHandle allocator;

	internal InstanceHandle Handle => instance;

	public Surface Surface { private set; get; }

	public unsafe PhysicalDevice[] PhysicalDevices 
	{
		get 
		{
			Result result;

			result = vkEnumeratePhysicalDevices(instance, out uint count, ref Unsafe.AsRef<PhysicalDevice>(default));
			if (result != Result.Success) throw new VulkanException(result);

			PhysicalDevice[] devices = new PhysicalDevice[count];

			result = vkEnumeratePhysicalDevices(instance, out count, ref MemoryMarshal.GetArrayDataReference(devices));
			if (result != Result.Success) throw new VulkanException(result);

			return devices;

			[DllImport(VK_LIB)] static extern Result vkEnumeratePhysicalDevices(InstanceHandle instance, out uint count, ref PhysicalDevice pDevices);
		}
	}

	public void CreateSurface(GLFW.Window window) 
	{
		Result result = glfwCreateWindowSurface(instance, window, allocator, out Surface surface);
		if (result != Result.Success) throw new VulkanException(result);

		this.Surface = surface;

		[DllImport(GLFW_LIB)] static extern Result glfwCreateWindowSurface(InstanceHandle instance, nint window, AllocationCallbacksHandle allocator, out Surface surface);
	}

	public void Dispose() 
	{
		vkDestroySurfaceKHR(instance, Surface, allocator);
		vkDestroyInstance(instance, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroySurfaceKHR(InstanceHandle instance, Surface surface, AllocationCallbacksHandle allocator);
		[DllImport(VK_LIB)] static extern void vkDestroyInstance(InstanceHandle instance, AllocationCallbacksHandle allocator);
	}

	internal Instance(InstanceHandle instance, AllocationCallbacksHandle allocator) => 
		(this.instance, this.allocator) = (instance, allocator)
	;
}
