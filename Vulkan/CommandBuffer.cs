using System;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;
using static Vulkan.ExtensionDelegates;

namespace Vulkan;

public readonly struct CommandBuffer 
{
	private readonly nint handle;

	public void BindVertexBuffers(params Buffer[] buffers) 
	{
		vkCmdBindVertexBuffers(
			this,
			0,
			(uint)buffers.Length,
			buffers.Select(x => (nint)x).ToArray().AsPointer(),
			buffers.Skip(1).Select(x => x.MemoryRequirements.Size).Prepend(default).ToArray().AsPointer()
		);

		[DllImport(VK_LIB)] static extern void vkCmdBindVertexBuffers(CommandBuffer commandBuffer, uint firstBinding, uint bindingCount, nint pBuffers, nint pOffsets);
	}

	public void BindIndexBuffer(Buffer buffer, IndexType type) 
	{
		vkCmdBindIndexBuffer(
			this,
			(nint)buffer,
			0,
			type
		);

		[DllImport(VK_LIB)] static extern void vkCmdBindIndexBuffer(CommandBuffer commandBuffer, nint buffer, DeviceSize offset, IndexType type);
	}

	public void CopyBuffer(Buffer source, Buffer destination, DeviceSize size) 
	{
		var region = new BufferCopy(sourceOffset: default, destinationOffset: default, size: size);

		vkCmdCopyBuffer(this, (nint)source, (nint)destination, 1, in region);

		[DllImport(VK_LIB)] static extern void vkCmdCopyBuffer(CommandBuffer commandBuffer, nint source, nint destination, uint regionCount, in BufferCopy pRegions);
	}

	public void CopyBuffer(Buffer source, Buffer destination, DeviceSize size, BufferCopy[] regions) 
	{
		vkCmdCopyBuffer(this, (nint)source, (nint)destination, (uint)regions.Length, regions.AsPointer());

		[DllImport(VK_LIB)] static extern void vkCmdCopyBuffer(CommandBuffer commandBuffer, nint source, nint destination, uint regionCount, nint pRegions);
	}

	public void Reset(CommandBufferResetFlags flags) 
	{
		Result result = vkResetCommandBuffer(this, flags);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkResetCommandBuffer(CommandBuffer commandBuffer, CommandBufferResetFlags flags);
	}

	public void Begin(CommandBufferBeginInfo beginInfo) 
	{
		Result result = vkBeginCommandBuffer(this, in beginInfo);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkBeginCommandBuffer(CommandBuffer commandBuffer, in CommandBufferBeginInfo beginInfo);
	}

	public void End() 
	{
		Result result = vkEndCommandBuffer(this);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkEndCommandBuffer(CommandBuffer commandBuffer);
	}

	public void BeginRenderPass(RenderPassBeginInfo renderPassInfo, SubpassContents contents) 
	{
		Result result = vkCmdBeginRenderPass(this, in renderPassInfo, contents);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkCmdBeginRenderPass(CommandBuffer commandBuffer, in RenderPassBeginInfo renderPassInfo, SubpassContents contents);
	}

	public void EndRenderPass() 
	{
		vkCmdEndRenderPass(this);

		[DllImport(VK_LIB)] static extern void vkCmdEndRenderPass(CommandBuffer commandBuffer);
	}

	public void BindPipeline(Pipeline pipeline, PipelineBindPoint bindPoint) 
	{
		vkCmdBindPipeline(this, bindPoint, (nint)pipeline);

		[DllImport(VK_LIB)] static extern void vkCmdBindPipeline(CommandBuffer commandBuffer, PipelineBindPoint bindPoint, nint pipeline);
	}

	public void SetViewports(params Viewport[] viewports) 
	{
		if (viewports == null) throw new ArgumentNullException();

		vkCmdSetViewport(this, 0, (uint)viewports.Length, viewports.AsPointer());

		[DllImport(VK_LIB)] static extern void vkCmdSetViewport(CommandBuffer commandBuffer, uint first, uint count, nint pViewports);
	}

	public void SetScissors(params Rect2D[] scissors) 
	{
		if (scissors == null) throw new ArgumentNullException();

		vkCmdSetScissor(this, 0, (uint)scissors.Length, scissors.AsPointer());

		[DllImport(VK_LIB)] static extern void vkCmdSetScissor(CommandBuffer commandBuffer, uint first, uint count, nint pViewports);
	}

	public void SetVertexInput(Instance instance, VertexInputBindingDescription2[] bindingDescriptions, VertexInputAttributeDescription2[] attributeDescriptions) 
	{
		if (bindingDescriptions == null || attributeDescriptions == null)
			throw new ArgumentNullException();

		vkCmdSetVertexInputEXT(
			this,
			(uint)bindingDescriptions.Length,
			bindingDescriptions.AsPointer(),
			(uint)attributeDescriptions.Length,
			attributeDescriptions.AsPointer()
		);
	}

	public void SetCullMode(Instance instance, CullMode mode) 
	{
		vkCmdSetCullModeEXT(this, mode);
	}

	public void Draw(int vertextCount, int instanceCount = 1, int firstVertex = 0, int firstInstance = 0) 
	{
		vkCmdDraw(this, (uint)vertextCount, (uint)instanceCount, (uint)firstVertex, (uint)firstInstance);

		[DllImport(VK_LIB)] static extern void vkCmdDraw(CommandBuffer commandBuffer, uint vertextCount, uint instanceCount, uint firstVertex, uint firstInstance);
	}

	public void DrawIndexed(int indexCount, int instanceCount = 1, int firstIndex = 0, int vertexOffset = 0, int firstInstance = 0) 
	{
		vkCmdDrawIndexed(this, (uint)indexCount, (uint)instanceCount, (uint)firstIndex, vertexOffset, (uint)firstInstance);

		[DllImport(VK_LIB)] static extern void vkCmdDrawIndexed(CommandBuffer commandBuffer, uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance);
	}

	public static bool operator == (CommandBuffer a, CommandBuffer b) => a.handle == b.handle;
	public static bool operator != (CommandBuffer a, CommandBuffer b) => a.handle != b.handle;
	public override bool Equals(object? other) => (other is CommandBuffer x) ? x.handle == handle : false;

	public static implicit operator nint (CommandBuffer x) => x.handle;
	public static implicit operator CommandBuffer (nint x) => new(x);

	public override string ToString() => handle.ToString();
	public override int GetHashCode() => handle.GetHashCode();

	private CommandBuffer(nint handle) => this.handle = handle;
}
