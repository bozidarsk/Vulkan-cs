using System;

namespace Vulkan;

public readonly struct ApplicationInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	private readonly cstring applicationName;
	public readonly uint ApplicationVersion;
	private readonly cstring engineName;
	public readonly uint EngineVersion;
	public readonly uint ApiVersion;

	public string? ApplicationName => applicationName;
	public string? EngineName => engineName;

	public void Dispose() 
	{
		applicationName.Dispose();
		engineName.Dispose();
	}

	public ApplicationInfo(StructureType type, nint next, string applicationName, uint applicationVersion, string engineName, uint engineVersion, uint apiVersion) 
	{
		this.Type = type;
		this.Next = next;
		this.applicationName = applicationName;
		this.ApplicationVersion = applicationVersion;
		this.engineName = engineName;
		this.EngineVersion = engineVersion;
		this.ApiVersion = apiVersion;
	}
}
