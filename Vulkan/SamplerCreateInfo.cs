using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct SamplerCreateInfo 
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly SamplerCreateFlags Flags;
	public readonly Filter MagFilter;
	public readonly Filter MinFilter;
	public readonly SamplerMipmapMode MipmapMode;
	public readonly SamplerAddressMode AddressModeU;
	public readonly SamplerAddressMode AddressModeV;
	public readonly SamplerAddressMode AddressModeW;
	public readonly float MipLodBias;
	public readonly bool32 AnisotropyEnable;
	public readonly float MaxAnisotropy;
	public readonly bool32 CompareEnable;
	public readonly CompareOp CompareOp;
	public readonly float MinLod;
	public readonly float MaxLod;
	public readonly BorderColor BorderColor;
	public readonly bool32 UnnormalizedCoordinates;

	public Sampler CreateSampler(Device device, AllocationCallbacksHandle allocator) 
	{
		Result result = vkCreateSampler(device.Handle, in this, allocator, out SamplerHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetSampler(device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateSampler(DeviceHandle device, in SamplerCreateInfo createInfo, AllocationCallbacksHandle allocator, out SamplerHandle sampler);
	}

	public SamplerCreateInfo(
		StructureType type,
		nint next,
		SamplerCreateFlags flags,
		Filter magFilter,
		Filter minFilter,
		SamplerMipmapMode mipmapMode,
		SamplerAddressMode addressModeU,
		SamplerAddressMode addressModeV,
		SamplerAddressMode addressModeW,
		float mipLodBias,
		bool anisotropyEnable,
		float maxAnisotropy,
		bool compareEnable,
		CompareOp compareOp,
		float minLod,
		float maxLod,
		BorderColor borderColor,
		bool unnormalizedCoordinates
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;
		this.MagFilter = magFilter;
		this.MinFilter = minFilter;
		this.MipmapMode = mipmapMode;
		this.AddressModeU = addressModeU;
		this.AddressModeV = addressModeV;
		this.AddressModeW = addressModeW;
		this.MipLodBias = mipLodBias;
		this.AnisotropyEnable = anisotropyEnable;
		this.MaxAnisotropy = maxAnisotropy;
		this.CompareEnable = compareEnable;
		this.CompareOp = compareOp;
		this.MinLod = minLod;
		this.MaxLod = maxLod;
		this.BorderColor = borderColor;
		this.UnnormalizedCoordinates = unnormalizedCoordinates;
	}
}
