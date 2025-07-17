using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Vulkan.ShaderCompiler;
using static Vulkan.Constants;

namespace Vulkan;

public partial class Program 
{
	protected Dictionary<string, (ShaderInfo, byte[])> shaderCode = new();
	protected Dictionary<RenderInfo, Pipeline> graphicsPipelines = new();

	protected ShaderModule CreateShaderModule(ref ShaderInfo info) 
	{
		if (info == null) throw new ArgumentNullException();

		info.File = Path.GetFullPath(info.File);
		byte[]? code;

		if (!shaderCode.ContainsKey(info.File)) 
		{
			code = Compiler.Compile(ref info);
			shaderCode.Add(info.File, (info, code!));
		}
		else (info, code) = shaderCode[info.File];

		using ShaderModuleCreateInfo shaderModuleCreateInfo = new(
			type: StructureType.ShaderModuleCreateInfo,
			next: default,
			flags: default,
			code: code!
		);

		return shaderModuleCreateInfo.CreateShaderModule(device, allocator);
	}

	protected virtual Pipeline CreateGraphicsPipeline(RenderInfo obj) 
	{
		var modules = new ShaderModule[obj.Shaders.Length];

		var stages = obj.Shaders.Index().Select(x => 
			{
				(int i, ShaderInfo info) = x;

				modules[i] = CreateShaderModule(ref info);
				obj.Shaders[i] = info;

				return new PipelineShaderStageCreateInfo(
					type: StructureType.PipelineShaderStageCreateInfo,
					next: default,
					flags: default,
					stage: (ShaderStage)info.Stage!,
					module: modules[i],
					name: info.EntryPoint!,
					specializationInfo: null
				);
			}
		).ToArray();

		using var vertexInput = new PipelineVertexInputStateCreateInfo(
			type: StructureType.PipelineVertexInputStateCreateInfo,
			next: default,
			flags: default,
			vertexBindingDescriptions: obj.BindingDescriptions,
			vertexAttributeDescriptions: obj.AttributeDescriptions
		);

		var inputAssembly = new PipelineInputAssemblyStateCreateInfo(
			type: StructureType.PipelineInputAssemblyStateCreateInfo,
			next: default,
			flags: default,
			topology: PrimitiveTopology.TriangleList,
			primitiveRestartEnable: false
		);

		using var viewport = new PipelineViewportStateCreateInfo(
			type: StructureType.PipelineViewportStateCreateInfo,
			next: default,
			flags: default,
			viewports: 
			[
				new(
					x: 0f,
					y: 0f,
					width: (float)extent.Width,
					height: (float)extent.Height,
					minDepth: 0f,
					maxDepth: 1f
				)
			],
			scissors: 
			[
				new(
					offset: new(x: 0, y: 0),
					extent: extent
				)
			]
		);

		var rasterization = new PipelineRasterizationStateCreateInfo(
			type: StructureType.PipelineRasterizationStateCreateInfo,
			next: default,
			flags: default,
			depthClampEnable: false,
			rasterizerDiscardEnable: false,
			polygonMode: PolygonMode.Fill,
			cullMode: obj.CullMode ?? CullMode.Back,
			frontFace: obj.FrontFace ?? FrontFace.CounterClockwise,
			depthBiasEnable: false,
			depthBiasConstantFactor: 0f,
			depthBiasClamp: 0f,
			depthBiasSlopeFactor: 0f,
			lineWidth: 1f
		);

		using var multisample = new PipelineMultisampleStateCreateInfo(
			type: StructureType.PipelineMultisampleStateCreateInfo,
			next: default,
			flags: default,
			rasterizationSamples: SampleCount.Bit1,
			sampleShadingEnable: false,
			minSampleShading: 1f,
			sampleMask: null,
			alphaToCoverageEnable: false,
			alphaToOneEnable: false
		);

		using var colorBlend = new PipelineColorBlendStateCreateInfo(
			type: StructureType.PipelineColorBlendStateCreateInfo,
			next: default,
			flags: default,
			logicOpEnable: false,
			logicOp: LogicOp.Copy,
			attachments: 
			[
				new(
					blendEnable: false,
					srcColorBlendFactor: BlendFactor.One,
					dstColorBlendFactor: BlendFactor.Zero,
					colorBlendOp: BlendOp.Add,
					srcAlphaBlendFactor: BlendFactor.One,
					dstAlphaBlendFactor: BlendFactor.Zero,
					alphaBlendOp: BlendOp.Add,
					colorWriteMask: ColorComponent.R | ColorComponent.G | ColorComponent.B | ColorComponent.A
				)
			],
			blendConstants: default
		);

		using var dynamicState = new PipelineDynamicStateCreateInfo(
			type: StructureType.PipelineDynamicStateCreateInfo,
			next: default,
			flags: default,
			dynamicStates: null
		);

		using var graphicsPipelineCreateInfo = new GraphicsPipelineCreateInfo(
			type: StructureType.GraphicsPipelineCreateInfo,
			next: default,
			flags: default,
			stages: stages,
			vertexInputState: vertexInput,
			inputAssemblyState: inputAssembly,
			tessellationState: null,
			viewportState: viewport,
			rasterizationState: rasterization,
			multisampleState: multisample,
			depthStencilState: null,
			colorBlendState: colorBlend,
			dynamicState: dynamicState,
			layout: pipelineLayout,
			renderPass: renderPass,
			subpass: 0,
			basePipeline: null,
			basePipelineIndex: -1
		);

		var pipeline = graphicsPipelineCreateInfo.CreateGraphicsPipeline(device, allocator);

		foreach (var x in stages)
			x.Dispose();

		foreach (var x in modules)
			x.Dispose();

		return pipeline;
	}

	protected virtual void StartRenderPass(IEnumerable<(Matrix4x4, RenderInfo)> objects, uint imageIndex) 
	{
		using var beginInfo = new CommandBufferBeginInfo(
			type: StructureType.CommandBufferBeginInfo,
			next: default,
			usage: default,
			inheritanceInfo: null
		);

		using var renderPassInfo = new RenderPassBeginInfo(
			type: StructureType.RenderPassBeginInfo,
			next: default,
			renderPass: renderPass,
			framebuffer: framebuffers[imageIndex],
			renderArea: new(offset: new(0, 0), extent: extent),
			clearValues: 
			[
				new(
					color: new(float32: Color.White * 0.02f, int32: default, uint32: default),
					depthStencil: new(depth: 0f, stencil: 0)
				)
			]
		);

		var cmd = commandBuffers[currentFrame];

		cmd.Begin(beginInfo);
		cmd.BeginRenderPass(renderPassInfo, SubpassContents.Inline);

		foreach ((Matrix4x4 transform, RenderInfo obj) in objects) 
		{
			Pipeline graphicsPipeline;

			if (!graphicsPipelines.TryGetValue(obj, out graphicsPipeline!)) 
			{
				graphicsPipeline = CreateGraphicsPipeline(obj);
				graphicsPipelines[obj] = graphicsPipeline;
			}

			cmd.BindPipeline(graphicsPipeline, PipelineBindPoint.Graphics);
			cmd.BindVertexBuffers(obj.VertexBuffer);
			cmd.BindIndexBuffer(obj.IndexBuffer, obj.IndexType);
			cmd.DrawIndexed(obj.IndexCount);
		}
		cmd.EndRenderPass();
		cmd.End();
	}

	// if throws ErrorOutOfDateKhr or SuboptimalKhr it needs swapchain recreation (see https://vulkan-tutorial.com/en/Drawing_a_triangle/Swap_chain_recreation)
	public virtual void DrawFrame(IEnumerable<(Matrix4x4, RenderInfo)> objects) 
	{
		if (objects == null)
			throw new ArgumentNullException();

		inFlightFence[currentFrame].Wait();
		inFlightFence[currentFrame].Reset();

		uint imageIndex = swapchain.GetNextImage(imageAvailableSemaphore[currentFrame]);
		var cmd = commandBuffers[currentFrame];

		cmd.Reset(default);

		StartRenderPass(objects, imageIndex);

		using var submitInfo = new SubmitInfo(
			type: StructureType.SubmitInfo,
			next: default,
			waitSemaphores: [ imageAvailableSemaphore[currentFrame] ],
			waitDstStageMasks: [ PipelineStage.ColorAttachmentOutput ],
			commandBuffers: [ cmd ],
			signalSemaphores: [ renderFinishedSemaphore[imageIndex] ]
		);

		graphicsQueue.Submit(inFlightFence[currentFrame], submitInfo);

		using var presentInfo = new PresentInfo(
			type: StructureType.PresentInfo,
			next: default,
			waitSemaphores: [ renderFinishedSemaphore[imageIndex] ],
			swapchains: [ swapchain ],
			imageIndices: [ imageIndex ],
			results: null
		);

		presentationQueue.Present(presentInfo);

		if (++currentFrame >= maxFrames)
			currentFrame = 0;
	}

	public bool TryDisposeGraphicsPipeline(RenderInfo info) 
	{
		bool success = graphicsPipelines.Remove(info, out Pipeline? pipeline);

		if (success) pipeline!.Dispose();
		return success;
	}
}
