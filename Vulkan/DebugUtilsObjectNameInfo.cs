#pragma warning disable CS0649

using System;

namespace Vulkan;

public readonly struct DebugUtilsObjectNameInfo : IDisposable
{
	public readonly StructureType Type = StructureType.DebugUtilsObjectNameInfoExt;
	public readonly nint Next;
	public readonly ObjectType ObjectType;
	public readonly ulong ObjectHandle;
	private readonly cstring objectName;

	public string? ObjectName => objectName;

	public override string? ToString() => ObjectName;

	public void Dispose()
	{
		objectName.Dispose();
	}

	public DebugUtilsObjectNameInfo(
		nint next,
		ObjectType objectType,
		ulong objectHandle,
		string? objectName
	)
	{
		this.Next = next;
		this.ObjectType = objectType;
		this.ObjectHandle = objectHandle;
		this.objectName = objectName;
	}
}
