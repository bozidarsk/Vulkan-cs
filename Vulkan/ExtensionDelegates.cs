#pragma warning disable CS0649
#pragma warning disable CS8618

using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate Result CreateDebugUtilsMessengerDelegate(
	InstanceHandle instance,
	in DebugUtilsMessengerCreateInfo createInfo,
	AllocationCallbacksHandle allocator,
	out DebugUtilsMessengerHandle messenger
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void DestroyDebugUtilsMessengerDelegate(
	InstanceHandle instance,
	DebugUtilsMessengerHandle messenger,
	AllocationCallbacksHandle allocator
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void SetVertexInputDelegate(
	CommandBufferHandle commandBuffer,
	uint bindingDescriptionsCount,
	ref VertexInputBindingDescription2 bindingDescriptions,
	uint attributeDescriptionsCount,
	ref VertexInputAttributeDescription2 attributeDescriptions
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void PushDescriptorSetDelegate(
	CommandBufferHandle commandBuffer,
	PipelineBindPoint bindPoint,
	PipelineLayoutHandle layout,
	uint set,
	uint descriptorWriteCount,
	ref WriteDescriptorSet pDescriptorWrites
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void SetCullModeDelegate(CommandBufferHandle commandBuffer, CullMode mode);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void SetFrontFaceDelegate(CommandBufferHandle commandBuffer, FrontFace frontFace);

internal static class ExtensionDelegates 
{
	public static CreateDebugUtilsMessengerDelegate vkCreateDebugUtilsMessengerEXT { private set; get; }
	public static DestroyDebugUtilsMessengerDelegate vkDestroyDebugUtilsMessengerEXT { private set; get; }
	public static SetVertexInputDelegate vkCmdSetVertexInputEXT { private set; get; }
	public static PushDescriptorSetDelegate vkCmdPushDescriptorSetKHR { private set; get; }
	public static SetCullModeDelegate vkCmdSetCullModeEXT { private set; get; }
	public static SetFrontFaceDelegate vkCmdSetFrontFaceEXT { private set; get; }

	public static void Initialize(Instance instance) 
	{
		vkCreateDebugUtilsMessengerEXT = Marshal.GetDelegateForFunctionPointer<CreateDebugUtilsMessengerDelegate>(vkGetInstanceProcAddr(instance.Handle, "vkCreateDebugUtilsMessengerEXT"));
		vkDestroyDebugUtilsMessengerEXT = Marshal.GetDelegateForFunctionPointer<DestroyDebugUtilsMessengerDelegate>(vkGetInstanceProcAddr(instance.Handle, "vkDestroyDebugUtilsMessengerEXT"));
		vkCmdSetVertexInputEXT = Marshal.GetDelegateForFunctionPointer<SetVertexInputDelegate>(vkGetInstanceProcAddr(instance.Handle, "vkCmdSetVertexInputEXT"));
		vkCmdPushDescriptorSetKHR = Marshal.GetDelegateForFunctionPointer<PushDescriptorSetDelegate>(vkGetInstanceProcAddr(instance.Handle, "vkCmdPushDescriptorSetKHR"));
		vkCmdSetCullModeEXT = Marshal.GetDelegateForFunctionPointer<SetCullModeDelegate>(vkGetInstanceProcAddr(instance.Handle, "vkCmdSetCullModeEXT"));
		vkCmdSetFrontFaceEXT = Marshal.GetDelegateForFunctionPointer<SetFrontFaceDelegate>(vkGetInstanceProcAddr(instance.Handle, "vkCmdSetFrontFaceEXT"));

		[DllImport(VK_LIB)] static extern nint vkGetInstanceProcAddr(InstanceHandle instance, string name);
	}
}
