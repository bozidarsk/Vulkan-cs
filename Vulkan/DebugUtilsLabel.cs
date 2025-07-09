#pragma warning disable CS0649

using System;

namespace Vulkan;

public readonly struct DebugUtilsLabel : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	private readonly cstring labelName;
	public readonly Color Color;

	public string? LabelName => labelName;

	public override string? ToString() => LabelName;

	public void Dispose() 
	{
		labelName.Dispose();
	}
}
