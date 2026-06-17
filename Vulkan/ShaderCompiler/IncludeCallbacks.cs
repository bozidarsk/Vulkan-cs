using System.Runtime.InteropServices;

namespace Vulkan.ShaderCompiler;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate Box<IncludeResult> IncludeResolverDelegate(
	CompilerOptionsHandle options,
	[MarshalAs(UnmanagedType.LPStr)] string requestedSource,
	IncludeType type,
	[MarshalAs(UnmanagedType.LPStr)] string requestingSource,
	nuint includeDepth
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void IncludeResultReleaserDelegate(
	CompilerOptionsHandle options,
	Box<IncludeResult> result
);
