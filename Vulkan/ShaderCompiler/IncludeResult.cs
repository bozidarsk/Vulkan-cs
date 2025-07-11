using System;

namespace Vulkan.ShaderCompiler;

public readonly struct IncludeResult : IDisposable
{
	// The name of the source file.  The name should be fully resolved
	// in the sense that it should be a unique name in the context of the
	// includer.  For example, if the includer maps source names to files in
	// a filesystem, then this name should be the absolute path of the file.
	// For a failed inclusion, this string is empty.
	private readonly cstring filename;
	private readonly nuint filenameLength;
	// The text contents of the source file in the normal case.
	// For a failed inclusion, this contains the error message.
	private readonly cstring content;
	private readonly nuint contentLength;
	// User data to be passed along with this request.
	public readonly nint UserData;

	public void Dispose() 
	{
		filename.Dispose();
		content.Dispose();
	}

	public IncludeResult(string filename, string content, nint userData) 
	{
		this.filename = filename;
		this.filenameLength = (nuint)filename.Length;

		this.content = content;
		this.contentLength = (nuint)content.Length;

		this.UserData = userData;
	}
}