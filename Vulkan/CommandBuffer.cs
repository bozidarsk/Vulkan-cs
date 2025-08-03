using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static Vulkan.Constants;
using static Vulkan.ExtensionDelegates;

namespace Vulkan;

public sealed class CommandBuffer 
{
	private readonly CommandBufferHandle commandBuffer;

	internal CommandBufferHandle Handle => commandBuffer;

	public void PushDescriptorSet(PipelineBindPoint bindPoint, PipelineLayout layout, params WriteDescriptorSet[] writes) 
	{
		if (writes == null)
			throw new ArgumentNullException();

		vkCmdPushDescriptorSetKHR(commandBuffer, bindPoint, layout.Handle, 0, (uint)writes.Length, ref MemoryMarshal.GetArrayDataReference(writes));
	}

	public void PushConstants(PipelineLayout layout, ShaderStage stage, uint offset, uint size, ref byte data) 
	{
		vkCmdPushConstants(commandBuffer, layout.Handle, stage, offset, size, ref data);

		[DllImport(VK_LIB)] static extern void vkCmdPushConstants(
			CommandBufferHandle commandBuffer,
			PipelineLayoutHandle layout,
			ShaderStage stage,
			uint offset,
			uint size,
			ref byte data
		);
	}

	public unsafe void BindDescriptorSets(PipelineBindPoint bindPoint, PipelineLayout layout, DescriptorSet[] sets, uint[]? dynamicOffsets = null) 
	{
		if (sets == null)
			throw new ArgumentNullException();

		vkCmdBindDescriptorSets(
			commandBuffer,
			bindPoint,
			layout.Handle,
			0,
			(uint)sets.Length,
			ref MemoryMarshal.GetArrayDataReference(sets.Select(x => x.Handle).ToArray()),
			(uint)(dynamicOffsets?.Length ?? 0),
			ref (dynamicOffsets != null) ? ref MemoryMarshal.GetArrayDataReference(dynamicOffsets) : ref Unsafe.AsRef<uint>(default)
		);

		[DllImport(VK_LIB)] static extern void vkCmdBindDescriptorSets(
			CommandBufferHandle commandBuffer,
			PipelineBindPoint pipelineBindPoint,
			PipelineLayoutHandle layout,
			uint firstSet,
			uint descriptorSetCount,
			ref DescriptorSetHandle pDescriptorSets,
			uint dynamicOffsetCount,
			ref uint pDynamicOffsets
		);
	}

	public void BindVertexBuffers(params Buffer[] buffers) 
	{
		if (buffers == null)
			throw new ArgumentNullException();

		vkCmdBindVertexBuffers(
			commandBuffer,
			0,
			(uint)buffers.Length,
			ref MemoryMarshal.GetArrayDataReference(buffers.Select(x => x.Handle).ToArray()),
			ref MemoryMarshal.GetArrayDataReference(buffers.Skip(1).Select(x => x.MemoryRequirements.Size).Prepend(default).ToArray())
		);

		[DllImport(VK_LIB)] static extern void vkCmdBindVertexBuffers(CommandBufferHandle commandBuffer, uint firstBinding, uint bindingCount, ref BufferHandle pBuffers, ref DeviceSize pOffsets);
	}

	public void BindIndexBuffer(Buffer buffer, IndexType type) 
	{
		vkCmdBindIndexBuffer(
			commandBuffer,
			buffer.Handle,
			0,
			type
		);

		[DllImport(VK_LIB)] static extern void vkCmdBindIndexBuffer(CommandBufferHandle commandBuffer, BufferHandle buffer, DeviceSize offset, IndexType type);
	}

	public void CopyBuffer(Buffer source, Buffer destination, DeviceSize size) 
	{
		var region = new BufferCopy(sourceOffset: default, destinationOffset: default, size: size);

		vkCmdCopyBuffer(commandBuffer, source.Handle, destination.Handle, 1, ref region);

		[DllImport(VK_LIB)] static extern void vkCmdCopyBuffer(CommandBufferHandle commandBuffer, BufferHandle source, BufferHandle destination, uint regionCount, ref BufferCopy pRegions);
	}

	public void CopyBuffer(Buffer source, Buffer destination, DeviceSize size, BufferCopy[] regions) 
	{
		vkCmdCopyBuffer(commandBuffer, source.Handle, destination.Handle, (uint)regions.Length, ref MemoryMarshal.GetArrayDataReference(regions));

		[DllImport(VK_LIB)] static extern void vkCmdCopyBuffer(CommandBufferHandle commandBuffer, BufferHandle source, BufferHandle destination, uint regionCount, ref BufferCopy pRegions);
	}

	public void Reset(CommandBufferResetFlags flags) 
	{
		Result result = vkResetCommandBuffer(commandBuffer, flags);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkResetCommandBuffer(CommandBufferHandle commandBuffer, CommandBufferResetFlags flags);
	}

	public void Begin(CommandBufferBeginInfo beginInfo) 
	{
		Result result = vkBeginCommandBuffer(commandBuffer, in beginInfo);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkBeginCommandBuffer(CommandBufferHandle commandBuffer, in CommandBufferBeginInfo beginInfo);
	}

	public void End() 
	{
		Result result = vkEndCommandBuffer(commandBuffer);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkEndCommandBuffer(CommandBufferHandle commandBuffer);
	}

	public void BeginRenderPass(RenderPassBeginInfo renderPassInfo, SubpassContents contents) 
	{
		Result result = vkCmdBeginRenderPass(commandBuffer, in renderPassInfo, contents);
		if (result != Result.Success) throw new VulkanException(result);

		[DllImport(VK_LIB)] static extern Result vkCmdBeginRenderPass(CommandBufferHandle commandBuffer, in RenderPassBeginInfo renderPassInfo, SubpassContents contents);
	}

	public void EndRenderPass() 
	{
		vkCmdEndRenderPass(commandBuffer);

		[DllImport(VK_LIB)] static extern void vkCmdEndRenderPass(CommandBufferHandle commandBuffer);
	}

	public void BindPipeline(Pipeline pipeline, PipelineBindPoint bindPoint) 
	{
		vkCmdBindPipeline(commandBuffer, bindPoint, pipeline.Handle);

		[DllImport(VK_LIB)] static extern void vkCmdBindPipeline(CommandBufferHandle commandBuffer, PipelineBindPoint bindPoint, PipelineHandle pipeline);
	}

	public void SetViewports(params Viewport[] viewports) 
	{
		if (viewports == null)
			throw new ArgumentNullException();

		vkCmdSetViewport(commandBuffer, 0, (uint)viewports.Length, ref MemoryMarshal.GetArrayDataReference(viewports));

		[DllImport(VK_LIB)] static extern void vkCmdSetViewport(CommandBufferHandle commandBuffer, uint first, uint count, ref Viewport pViewports);
	}

	public void SetScissors(params Rect2D[] scissors) 
	{
		if (scissors == null)
			throw new ArgumentNullException();

		vkCmdSetScissor(commandBuffer, 0, (uint)scissors.Length, ref MemoryMarshal.GetArrayDataReference(scissors));

		[DllImport(VK_LIB)] static extern void vkCmdSetScissor(CommandBufferHandle commandBuffer, uint first, uint count, ref Rect2D pScissors);
	}

	public void SetVertexInput(VertexInputBindingDescription2[] bindingDescriptions, VertexInputAttributeDescription2[] attributeDescriptions) 
	{
		if (bindingDescriptions == null || attributeDescriptions == null)
			throw new ArgumentNullException();

		vkCmdSetVertexInputEXT(
			commandBuffer,
			(uint)bindingDescriptions.Length,
			ref MemoryMarshal.GetArrayDataReference(bindingDescriptions),
			(uint)attributeDescriptions.Length,
			ref MemoryMarshal.GetArrayDataReference(attributeDescriptions)
		);
	}

	public void SetCullMode(CullMode mode) 
	{
		vkCmdSetCullModeEXT(commandBuffer, mode);
	}

	public void SetFrontFace(FrontFace frontFace) 
	{
		vkCmdSetFrontFaceEXT(commandBuffer, frontFace);
	}

	public void Draw(int vertextCount, int instanceCount = 1, int firstVertex = 0, int firstInstance = 0) 
	{
		vkCmdDraw(commandBuffer, (uint)vertextCount, (uint)instanceCount, (uint)firstVertex, (uint)firstInstance);

		[DllImport(VK_LIB)] static extern void vkCmdDraw(CommandBufferHandle commandBuffer, uint vertextCount, uint instanceCount, uint firstVertex, uint firstInstance);
	}

	public void DrawIndexed(int indexCount, int instanceCount = 1, int firstIndex = 0, int vertexOffset = 0, int firstInstance = 0) 
	{
		vkCmdDrawIndexed(commandBuffer, (uint)indexCount, (uint)instanceCount, (uint)firstIndex, vertexOffset, (uint)firstInstance);

		[DllImport(VK_LIB)] static extern void vkCmdDrawIndexed(CommandBufferHandle commandBuffer, uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance);
	}

	internal CommandBuffer(CommandBufferHandle commandBuffer) => 
		this.commandBuffer = commandBuffer
	;
}
