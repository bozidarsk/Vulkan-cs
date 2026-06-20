#pragma warning disable CS0649

using System;

namespace Vulkan;

public unsafe struct DebugUtilsLabel : IDisposable
{
	public readonly StructureType Type = StructureType.DebugUtilsLabelExt;
	public readonly nint Next;
	private readonly cstring labelName;
	private fixed float color[4];

	public string? LabelName => labelName;
	public (float r, float g, float b, float a) Color => (color[0], color[1], color[2], color[3]);

	public override string? ToString() => LabelName;

	public void Dispose()
	{
		labelName.Dispose();
	}

	public DebugUtilsLabel(nint next, string? labelName, (float r, float g, float b, float a) color)
	{
		this.Next = next;
		this.labelName = labelName;

		(this.color[0], this.color[1], this.color[2], this.color[3]) = color;
	}
}
