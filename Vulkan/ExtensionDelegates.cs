using System.Runtime.InteropServices;

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
