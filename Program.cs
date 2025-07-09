using System;
using System.Linq;

using GLFW;
using Vulkan;

public static class Program 
{
	private static int Main(string[] args) 
	{
		if (!GLFW.Program.Initialize())
			return 1;

		Window.SetHint(Hint.ClientApi, 0);

		AllocationCallbacks? allocator = null;

		Window window = new(1280, 720);
		Vulkan.Program vk = new(window, in allocator);

		window.OnKey += (s, e) => Console.WriteLine(e.Key);
		// window.OnFramebufferSize += (s, e) => vk.RecreateSwapchain();

		#if DEBUG
		vk.OnDebugMessage += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Type}: {e.MessageIdName}: {e.Message}");
		#endif

		vk.Initialize();

		while (!window.ShouldClose) 
		{
			Input.PollEvents();

			vk.DrawFrame();
		}

		vk.DeviceWaitIdle();

		vk.Dispose();
		window.Dispose();
		GLFW.Program.Terminate();

		return 0;
	}
}
