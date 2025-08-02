using System;
using System.Linq;

namespace Vulkan;

public readonly struct WriteDescriptorSet : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	private readonly nint destinationSet;
	public readonly uint DestinationBinding;
	public readonly uint DestinationArrayElement;
	private readonly uint descriptorCount;
	public readonly DescriptorType DescriptorType;
	private readonly Handle<DescriptorImageInfo> imageInfos;
	private readonly Handle<DescriptorBufferInfo> bufferInfos;
	private readonly Handle<nint> texelBufferViews;

	public DescriptorSet DestinationSet => throw new NotImplementedException(); // cannot get allocator and device params
	public DescriptorImageInfo[]? ImageInfo => imageInfos.ToArray(descriptorCount);
	public DescriptorBufferInfo[]? BufferInfo => bufferInfos.ToArray(descriptorCount);
	public BufferView[]? TexelBufferView => throw new NotImplementedException(); // cannot get allocator and device params

	public void Dispose() 
	{
		imageInfos.Dispose();
		bufferInfos.Dispose();
		texelBufferViews.Dispose();
	}

	public WriteDescriptorSet(
		StructureType type,
		nint next,
		DescriptorSet destinationSet,
		uint destinationBinding,
		uint destinationArrayElement,
		DescriptorType descriptorType,
		DescriptorImageInfo[]? imageInfos,
		DescriptorBufferInfo[]? bufferInfos,
		BufferView[]? texelBufferViews
	)

	{
		this.Type = type;
		this.Next = next;
		this.destinationSet = (nint)destinationSet;
		this.DestinationBinding = destinationBinding;
		this.DestinationArrayElement = destinationArrayElement;
		this.DescriptorType = descriptorType;

		if (imageInfos != null) 
		{
			this.descriptorCount = (uint)(imageInfos?.Length ?? 0);
			this.imageInfos = new(imageInfos);
		}

		if (bufferInfos != null) 
		{
			this.descriptorCount = (uint)(bufferInfos?.Length ?? 0);
			this.bufferInfos = new(bufferInfos);
		}

		if (texelBufferViews != null) 
		{
			this.descriptorCount = (uint)(texelBufferViews?.Length ?? 0);
			this.texelBufferViews = new(texelBufferViews?.Select(x => (nint)x).ToArray());
		}
	}
}
