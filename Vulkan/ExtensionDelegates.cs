#pragma warning disable CS0649
#pragma warning disable CS8618

using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate Result CreateDebugUtilsMessengerDelegate(
	nint instance,
	in DebugUtilsMessengerCreateInfo createInfo,
	nint allocator,
	out nint messenger
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void DestroyDebugUtilsMessengerDelegate(
	nint instance,
	nint messenger,
	nint allocator
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void SetVertexInputDelegate(
	CommandBuffer commandBuffer,
	uint bindingDescriptionsCount,
	nint bindingDescriptions,
	uint attributeDescriptionsCount,
	nint attributeDescriptions
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void SetCullModeDelegate(CommandBuffer commandBuffer, CullMode mode);

internal static class ExtensionDelegates 
{
	public static CreateDebugUtilsMessengerDelegate vkCreateDebugUtilsMessengerEXT { private set; get; }
	public static DestroyDebugUtilsMessengerDelegate vkDestroyDebugUtilsMessengerEXT { private set; get; }
	public static SetVertexInputDelegate vkCmdSetVertexInputEXT { private set; get; }
	public static SetCullModeDelegate vkCmdSetCullModeEXT { private set; get; }

	public static void Initialize(Instance instance) 
	{
		vkCreateDebugUtilsMessengerEXT = Marshal.GetDelegateForFunctionPointer<CreateDebugUtilsMessengerDelegate>(vkGetInstanceProcAddr((nint)instance, "vkCreateDebugUtilsMessengerEXT"));
		vkDestroyDebugUtilsMessengerEXT = Marshal.GetDelegateForFunctionPointer<DestroyDebugUtilsMessengerDelegate>(vkGetInstanceProcAddr((nint)instance, "vkDestroyDebugUtilsMessengerEXT"));
		vkCmdSetVertexInputEXT = Marshal.GetDelegateForFunctionPointer<SetVertexInputDelegate>(vkGetInstanceProcAddr((nint)instance, "vkCmdSetVertexInputEXT"));
		vkCmdSetCullModeEXT = Marshal.GetDelegateForFunctionPointer<SetCullModeDelegate>(vkGetInstanceProcAddr((nint)instance, "vkCmdSetCullModeEXT"));

		[DllImport(VK_LIB)] static extern nint vkGetInstanceProcAddr(nint instance, string name);
	}
}
