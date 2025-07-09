using static Vulkan.Constants;

namespace Vulkan;

public struct LayerProperties 
{
	private unsafe fixed sbyte layerName[VK_MAX_EXTENSION_NAME_SIZE];
	public readonly uint SpecVersion;
	public readonly uint ImplementationVersion;
	private unsafe fixed sbyte description[VK_MAX_DESCRIPTION_SIZE];

	public unsafe string LayerName { get { fixed (sbyte* x = layerName) return new(x); } }
	public unsafe string Description { get { fixed (sbyte* x = description) return new(x); } }

	public override string ToString() => LayerName;
}
