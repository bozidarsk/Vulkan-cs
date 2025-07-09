using System;

namespace Vulkan;

public readonly struct SpecializationInfo : IDisposable
{
	private readonly uint mapEntryCount;
	private readonly Handle<SpecializationMapEntry> mapEntries;
	private readonly nuint dataSize;
	private readonly Handle<byte> data;

	public SpecializationMapEntry[]? MapEntries => mapEntries.ToArray(mapEntryCount);
	public byte[]? Data => data.ToArray((uint)dataSize);

	public void Dispose() 
	{
		mapEntries.Dispose();
		data.Dispose();
	}

	public SpecializationInfo(SpecializationMapEntry[]? mapEntries, byte[]? data) 
	{
		this.mapEntryCount = (uint)(mapEntries?.Length ?? 0);
		this.mapEntries = new(mapEntries);

		this.dataSize = (nuint)(data?.Length ?? 0);
		this.data = new(data);
	}
}
