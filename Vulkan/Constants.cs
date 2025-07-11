namespace Vulkan;

internal static class Constants 
{
	public const string VK_LIB = 
	#if WINDOWS
	#error Not implemented. (WINDOWS)
	#elif LINUX
	"vulkan"
	#elif MAC
	#error Not implemented. (MAC)
	#else
	#error Unknown os.
	#endif
	;

	public const string GLFW_LIB = 
	#if WINDOWS
	"glfw3"
	#elif LINUX
	"glfw"
	#elif MAC
	#error Not implemented. (MAC)
	#else
	#error Unknown os.
	#endif
	;

	public const string SHADERC_LIB = 
	#if WINDOWS
	#error Not implemented. (WINDOWS)
	#elif LINUX
	"shaderc_shared"
	#elif MAC
	#error Not implemented. (MAC)
	#else
	#error Unknown os.
	#endif
	;

	public const int VK_MAX_EXTENSION_NAME_SIZE = 256;
	public const int VK_MAX_DESCRIPTION_SIZE = 256;
	public const int VK_MAX_PHYSICAL_DEVICE_NAME_SIZE = 256;
	public const int VK_UUID_SIZE = 16;
}
