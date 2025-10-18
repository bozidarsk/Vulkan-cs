using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct RenderPassCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly RenderPassCreateFlags Flags;
	public readonly uint attachmentCount;
	private readonly Handle<AttachmentDescription> attachments;
	public readonly uint subpassCount;
	private readonly Handle<SubpassDescription> subpasses;
	public readonly uint dependencyCount;
	private readonly Handle<SubpassDependency> dependencies;

	public AttachmentDescription[]? Attachments => attachments.ToArray(attachmentCount);
	public SubpassDescription[]? Subpasses => subpasses.ToArray(subpassCount);
	public SubpassDependency[]? Dependencies => dependencies.ToArray(dependencyCount);

	public RenderPass CreateRenderPass(Device device, AllocationCallbacksHandle allocator) 
	{
		Result result = vkCreateRenderPass(device.Handle, in this, allocator, out RenderPassHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetRenderPass(device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateRenderPass(DeviceHandle device, in RenderPassCreateInfo createInfo, AllocationCallbacksHandle allocator, out RenderPassHandle renderPass);
	}

	public void Dispose() 
	{
		attachments.Dispose();
		subpasses.Dispose();
		dependencies.Dispose();
	}

	public RenderPassCreateInfo(
		StructureType type,
		nint next,
		RenderPassCreateFlags flags,
		AttachmentDescription[]? attachments,
		SubpassDescription[]? subpasses,
		SubpassDependency[]? dependencies
	)
	{
		this.Type = type;
		this.Next = next;
		this.Flags = flags;

		this.attachmentCount = (uint)(attachments?.Length ?? 0);
		this.attachments = new(attachments);

		this.subpassCount = (uint)(subpasses?.Length ?? 0);
		this.subpasses = new(subpasses);

		this.dependencyCount = (uint)(dependencies?.Length ?? 0);
		this.dependencies = new(dependencies);
	}
}
