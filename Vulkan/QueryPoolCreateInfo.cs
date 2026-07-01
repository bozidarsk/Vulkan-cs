using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct QueryPoolCreateInfo
{
	public readonly StructureType Type = StructureType.QueryPoolCreateInfo;
	public readonly nint Next;
	public readonly QueryPoolCreateFlags Flags;
	public readonly QueryType QueryType;
	public readonly uint QueryCount;
	public readonly QueryPipelineStatisticFlags PipelineStatistics;

	public QueryPool CreateQueryPool(Device device, AllocationCallbacks? allocator)
	{
		Result result = vkCreateQueryPool(device.Handle, in this, allocator?.Handle ?? default, out QueryPoolHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(handle, device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateQueryPool(DeviceHandle device, in QueryPoolCreateInfo createInfo, AllocationCallbacksHandle allocator, out QueryPoolHandle framebuffer);
	}

	public QueryPoolCreateInfo(
		nint next,
		QueryPoolCreateFlags flags,
		QueryType queryType,
		uint queryCount,
		QueryPipelineStatisticFlags pipelineStatistics
	)
	{
		this.Next = next;
		this.Flags = flags;
		this.QueryType = queryType;
		this.QueryCount = queryCount;
		this.PipelineStatistics = pipelineStatistics;
	}
}
