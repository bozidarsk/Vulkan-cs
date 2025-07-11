using System.Runtime.InteropServices;

namespace Vulkan.ShaderCompiler;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate Handle<IncludeResult> IncludeResolverDelegate(
	CompilerOptions options,
	[MarshalAs(UnmanagedType.LPStr)] string pRequestedSource,
	IncludeType type,
	[MarshalAs(UnmanagedType.LPStr)] string pRequestingSource,
	nuint includeDepth
);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void IncludeResultReleaserDelegate(
	CompilerOptions options,
	Handle<IncludeResult> result
);
