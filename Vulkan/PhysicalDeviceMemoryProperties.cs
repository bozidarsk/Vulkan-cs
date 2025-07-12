using System;

using static Vulkan.Constants;

namespace Vulkan;

public unsafe struct PhysicalDeviceMemoryProperties 
{
	private uint memoryTypeCount;
	private fixed byte memoryTypes[VK_MAX_MEMORY_TYPES * 8];
	private uint memoryHeapCount;
	private fixed byte memoryHeaps[VK_MAX_MEMORY_HEAPS * 16];

	public MemoryType[] MemoryTypes { get { fixed (byte* x = memoryTypes) return new ReadOnlySpan<MemoryType>(x, (int)memoryTypeCount).ToArray(); } }
	public MemoryHeap[] MemoryHeaps { get { fixed (byte* x = memoryHeaps) return new ReadOnlySpan<MemoryHeap>(x, (int)memoryHeapCount).ToArray(); } }
}
