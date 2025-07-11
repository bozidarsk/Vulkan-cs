namespace Vulkan.ShaderCompiler;

public enum IncludeType : int
{
	Relative = 0, // #include "source"
	Standard // #include <source>
}
