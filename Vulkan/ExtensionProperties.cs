using static Vulkan.Constants;

namespace Vulkan;

public struct ExtensionProperties 
{
	private unsafe fixed sbyte extensionName[VK_MAX_EXTENSION_NAME_SIZE];
	public readonly uint SpecVersion;

	public unsafe string ExtensionName { get { fixed (sbyte* x = extensionName) return new(x); } }

	public override string ToString() => ExtensionName;
}
