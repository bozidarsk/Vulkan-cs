using System;

namespace Vulkan;

public readonly struct DescriptorSetLayoutBinding 
{
	public readonly uint Binding;
	public readonly DescriptorType DescriptorType;
	public readonly uint DescriptorCount;
	public readonly ShaderStage Stage;
	private readonly SamplerHandle immutableSamplers;

	public Sampler ImmutableSamplers => throw new NotImplementedException(); // cannot get allocator and device params

	public DescriptorSetLayoutBinding(
		uint binding,
		DescriptorType descriptorType,
		uint descriptorCount,
		ShaderStage stage,
		Sampler? immutableSamplers
	)
	{
		this.Binding = binding;
		this.DescriptorType = descriptorType;
		this.DescriptorCount = descriptorCount;
		this.Stage = stage;
		this.immutableSamplers = (immutableSamplers != null) ? immutableSamplers.Handle : default;
	}
}
