namespace Vulkan;

public readonly struct PhysicalDeviceSparseProperties 
{
	public readonly bool32 ResidencyStandard2DBlockShape;
	public readonly bool32 ResidencyStandard2DMultisampleBlockShape;
	public readonly bool32 ResidencyStandard3DBlockShape;
	public readonly bool32 ResidencyAlignedMipSize;
	public readonly bool32 ResidencyNonResidentStrict;
}
