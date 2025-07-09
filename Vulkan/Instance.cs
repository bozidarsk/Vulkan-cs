using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Instance : IDisposable
{
	private readonly nint instance;
	private readonly Handle<AllocationCallbacks> allocator;

	public Surface Surface { private set; get; }

	public PhysicalDevice[] PhysicalDevices 
	{
		get 
		{
			Result result;

			result = vkEnumeratePhysicalDevices(instance, out uint count, default);
			if (result != Result.Success) throw new VulkanException(result);

			PhysicalDevice[] devices = new PhysicalDevice[count];

			result = vkEnumeratePhysicalDevices(instance, out count, devices.AsPointer());
			if (result != Result.Success) throw new VulkanException(result);

			return devices;

			[DllImport(VK_LIB)] static extern Result vkEnumeratePhysicalDevices(nint instance, out uint count, nint pDevices);
		}
	}

	public void CreateSurface(GLFW.Window window) 
	{
		Result result = glfwCreateWindowSurface(instance, window, allocator, out Surface surface);
		if (result != Result.Success) throw new VulkanException(result);

		this.Surface = surface;

		[DllImport(GLFW_LIB)] static extern Result glfwCreateWindowSurface(nint instance, nint window, nint allocator, out Surface surface);
	}

	public static explicit operator nint (Instance x) => x.instance;

	public void Dispose() 
	{
		vkDestroySurfaceKHR(instance, Surface, allocator);
		vkDestroyInstance(instance, allocator);

		[DllImport(VK_LIB)] static extern void vkDestroySurfaceKHR(nint instance, Surface surface, nint allocator);
		[DllImport(VK_LIB)] static extern void vkDestroyInstance(nint instance, nint allocator);
	}

	private Instance(nint instance) => this.instance = instance;
	internal Instance(nint instance, Handle<AllocationCallbacks> allocator) : this(instance) => this.allocator = allocator;
}
