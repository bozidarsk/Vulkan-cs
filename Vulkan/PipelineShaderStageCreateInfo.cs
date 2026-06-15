using System;

namespace Vulkan;

public readonly struct PipelineShaderStageCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.PipelineShaderStageCreateInfo;
	public readonly nint Next;
	public readonly PipelineShaderStageCreateFlags Flags;
	public readonly ShaderStage Stage;
	private readonly ShaderModuleHandle module;
	private readonly cstring name;
	private readonly Handle<SpecializationInfo> specializationInfo;

	public ShaderModule Module => throw new NotImplementedException(); // cannot get device and allocator params
	public string? Name => name;
	public SpecializationInfo SpecializationInfo => specializationInfo;

	public void Dispose()
	{
		name.Dispose();
		specializationInfo.Dispose();
	}

	public PipelineShaderStageCreateInfo(nint next, PipelineShaderStageCreateFlags flags, ShaderStage stage, ShaderModule module, string name, SpecializationInfo? specializationInfo)
	{
		this.Next = next;
		this.Flags = flags;
		this.Stage = stage;
		this.module = module.Handle;
		this.name = name;
		this.specializationInfo = (specializationInfo is SpecializationInfo x) ? new(x) : default;
	}
}
