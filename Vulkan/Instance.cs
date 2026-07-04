using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Linq;

using static Vulkan.Constants;

namespace Vulkan;

public sealed class Instance : IDisposable
{
	private readonly InstanceHandle instance;
	private readonly AllocationCallbacks? allocator;

	internal InstanceHandle Handle => instance;

	public Surface? Surface { private set; get; }

	public unsafe PhysicalDevice[] PhysicalDevices
	{
		get
		{
			Result result;

			result = vkEnumeratePhysicalDevices(instance, out uint count, ref Unsafe.AsRef<PhysicalDeviceHandle>(default));
			if (result != Result.Success) throw new VulkanException(result);

			PhysicalDeviceHandle[] devices = new PhysicalDeviceHandle[count];

			result = vkEnumeratePhysicalDevices(instance, out count, ref MemoryMarshal.GetArrayDataReference(devices));
			if (result != Result.Success) throw new VulkanException(result);

			return devices.Select(x => new PhysicalDevice(x)).ToArray();

			[DllImport(VK_LIB)] static extern Result vkEnumeratePhysicalDevices(InstanceHandle instance, out uint count, ref PhysicalDeviceHandle pDevices);
		}
	}

	public void CreateSurface(GLFW.Window window)
	{
		Result result = glfwCreateWindowSurface(instance, window.NativeHandle, allocator?.Handle ?? default, out SurfaceHandle surface);
		if (result != Result.Success) throw new VulkanException(result);

		this.Surface = new(surface, this, allocator);

		[DllImport(GLFW_LIB)] static extern Result glfwCreateWindowSurface(InstanceHandle instance, nint window, AllocationCallbacksHandle allocator, out SurfaceHandle surface);
	}

	public void Dispose()
	{
		this.Surface?.Dispose();

		vkDestroyInstance(instance, allocator?.Handle ?? default);

		[DllImport(VK_LIB)] static extern void vkDestroyInstance(InstanceHandle instance, AllocationCallbacksHandle allocator);
	}

	public override string ToString() => instance.ToString();

	internal Instance(InstanceHandle instance, AllocationCallbacks? allocator) =>
		(this.instance, this.allocator) = (instance, allocator)
	;
}
