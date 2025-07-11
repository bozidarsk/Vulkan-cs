namespace Vulkan.ShaderCompiler;

public enum SPIRVVersion : int
{
	// Use the values used for word 1 of a SPIR-V binary:
	// - bits 24 to 31: zero
	// - bits 16 to 23: major version number
	// - bits 8 to 15: minor version number
	// - bits 0 to 7: zero
	Version10 = 0x010000,
	Version11 = 0x010100,
	Version12 = 0x010200,
	Version13 = 0x010300,
	Version14 = 0x010400,
	Version15 = 0x010500,
	Version16 = 0x010600,
}
