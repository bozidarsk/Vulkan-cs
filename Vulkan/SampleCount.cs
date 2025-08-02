namespace Vulkan;

[System.Flags]
public enum SampleCount : uint
{
	Bit1 = 0x00000001,
    Bit2 = 0x00000002,
    Bit4 = 0x00000004,
    Bit8 = 0x00000008,
    Bit16 = 0x00000010,
    Bit32 = 0x00000020,
    Bit64 = 0x00000040,
}
