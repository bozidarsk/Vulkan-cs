using System.Runtime.InteropServices;

namespace Vulkan;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool DebugUtilsMessengerCallback(
	DebugUtilsMessageSeverity messageSeverity,
	DebugUtilsMessageType messageType,
	in DebugUtilsMessengerCallbackData callbackData,
	nint userData
);
