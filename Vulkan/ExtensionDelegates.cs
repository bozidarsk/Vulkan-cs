#pragma warning disable CS0649
#pragma warning disable CS8618

using System.Runtime.InteropServices;

namespace Vulkan;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void BeginDebugUtilsLabelDelegate(CommandBufferHandle commandBuffer, in DebugUtilsLabel label);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void EndDebugUtilsLabelDelegate(CommandBufferHandle commandBuffer);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate Result SetDebugUtilsObjectNameDelegate(
	DeviceHandle device,
	in DebugUtilsObjectNameInfo nameInfo
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate Result CreateDebugUtilsMessengerExtensionDelegate(
	InstanceHandle instance,
	in DebugUtilsMessengerCreateInfo createInfo,
	AllocationCallbacksHandle allocator,
	out DebugUtilsMessengerHandle messenger
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void DestroyDebugUtilsMessengerExtensionDelegate(
	InstanceHandle instance,
	DebugUtilsMessengerHandle messenger,
	AllocationCallbacksHandle allocator
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void SetVertexInputExtensionDelegate(
	CommandBufferHandle commandBuffer,
	uint bindingDescriptionsCount,
	ref VertexInputBindingDescription2 bindingDescriptions,
	uint attributeDescriptionsCount,
	ref VertexInputAttributeDescription2 attributeDescriptions
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void SetColorBlendEnableDelegate(
	CommandBufferHandle commandBuffer,
	uint firstAttachment,
	uint attachmentCount,
	ref bool32 colorBlendEnables
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void SetColorBlendEquationDelegate(
	CommandBufferHandle commandBuffer,
	uint firstAttachment,
	uint attachmentCount,
	ref ColorBlendEquation colorBlendEquations
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void PushDescriptorSetExtensionDelegate(
	CommandBufferHandle commandBuffer,
	PipelineBindPoint bindPoint,
	PipelineLayoutHandle layout,
	uint set,
	uint descriptorWriteCount,
	ref WriteDescriptorSet pDescriptorWrites
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void SetCullModeExtensionDelegate(CommandBufferHandle commandBuffer, CullMode mode);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void SetFrontFaceExtensionDelegate(CommandBufferHandle commandBuffer, FrontFace frontFace);
