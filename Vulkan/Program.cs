using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public class Program : IDisposable
{
	protected readonly GLFW.Window window;
	protected readonly Handle<AllocationCallbacks> allocator;

	protected uint graphicsQueueFamilyIndex, presentationQueueFamilyIndex;
	protected Format swapchainImageFormat;
	protected Extent2D extent;

	protected uint currentFrame = 0;
	protected uint maxFrames => (uint)imageViews.Length;

	protected Instance instance;
	protected DebugUtilsMessenger debugUtilsMessenger;
	protected PhysicalDevice physicalDevice;
	protected Device device;
	protected Swapchain swapchain;
	protected ImageView[] imageViews;
	protected PipelineLayout pipelineLayout;
	protected RenderPass renderPass;
	protected Pipeline graphicsPipeline;
	protected Framebuffer[] framebuffers;
	protected CommandPool commandPool;
	protected CommandBuffer[] commandBuffers;
	protected Semaphore[] imageAvailableSemaphore, renderFinishedSemaphore;
	protected Fence[] inFlightFence;

	protected Dictionary<string, byte[]> shaderCode = new();

	public static uint MakeVersion(int major, int minor, int patch) => ((((uint)major) << 22) | (((uint)minor) << 12) | ((uint)patch));
	public static uint MakeApiVersion(int variant, int major, int minor, int patch) => ((((uint)variant) << 29) | (((uint)major) << 22) | (((uint)minor) << 12) | ((uint)patch));

	private readonly DebugUtilsMessengerCallback debugMessageCallback;
	public event DebugEventHandler? OnDebugMessage;

	protected virtual void InitializeDebugMessages() 
	{
		var debugCreateInfo = new DebugUtilsMessengerCreateInfo(
			type: StructureType.DebugUtilsMessengerCreateInfoExt,
			next: default,
			flags: default,
			messageSeverity: DebugUtilsMessageSeverity.Info | DebugUtilsMessageSeverity.Verbose | DebugUtilsMessageSeverity.Warning | DebugUtilsMessageSeverity.Error,
			messageType: DebugUtilsMessageType.General | DebugUtilsMessageType.Validation | DebugUtilsMessageType.Performance,
			userCallback: debugMessageCallback,
			userData: default
		);

		debugUtilsMessenger = debugCreateInfo.CreateDebugUtilsMessanger(instance, allocator);
	}

	protected virtual void InitializeInstance() 
	{
		using var appInfo = new ApplicationInfo(
			type: StructureType.ApplicationInfo,
			next: default,
			applicationName: "Vulkan Test",
			applicationVersion: MakeVersion(1, 0, 0),
			engineName: "No Engine",
			engineVersion: MakeVersion(1, 0, 0),
			apiVersion: MakeApiVersion(0, 1, 0, 0)
		);

		var extensions = new List<string>(GLFW.Program.RequiredInstanceExtensions!);
		extensions.Add("VK_KHR_portability_enumeration");
		extensions.Add("VK_EXT_debug_utils");

		using var instanceCreateInfo = new InstanceCreateInfo(
			type: StructureType.InstanceCreateInfo,
			next: default,
			flags: InstanceCreateFlags.NumeratePortability,
			applicationInfo: appInfo,
			enabledLayerNames: [ "VK_LAYER_KHRONOS_validation" ],
			enabledExtensionNames: extensions.ToArray()
		);

		instance = instanceCreateInfo.CreateInstance(allocator);
	}

	protected virtual void InitializePhysicalDevice() => 
		physicalDevice = instance.PhysicalDevices.Where(x => x.Properties.DeviceType == PhysicalDeviceType.IntegratedGpu).First()
	;

	protected virtual void InitializeDevice() 
	{
		graphicsQueueFamilyIndex = find(physicalDevice.QueueFamilyProperties, static (i, x) => x.QueueFlags.HasFlag(QueueFlags.Graphics));
		presentationQueueFamilyIndex = find(physicalDevice.QueueFamilyProperties, (i, x) => instance.Surface.IsSupported(physicalDevice, (uint)i));

		using var graphicsDeviceQueueCreateInfo = new DeviceQueueCreateInfo(
			type: StructureType.DeviceQueueCreateInfo,
			next: default,
			flags: default,
			queueFamilyIndex: graphicsQueueFamilyIndex,
			queuePriorities: [ 1f ]
		);

		using var presentationDeviceQueueCreateInfo = new DeviceQueueCreateInfo(
			type: StructureType.DeviceQueueCreateInfo,
			next: default,
			flags: default,
			queueFamilyIndex: presentationQueueFamilyIndex,
			queuePriorities: [ 1f ]
		);

		using var deviceCreateInfo = new DeviceCreateInfo(
			type: StructureType.DeviceCreateInfo,
			next: default,
			flags: default,
			queueCreateInfos: (graphicsQueueFamilyIndex != presentationQueueFamilyIndex) ? [ graphicsDeviceQueueCreateInfo, presentationDeviceQueueCreateInfo ] : [ graphicsDeviceQueueCreateInfo ],
			enabledLayerNames: [ "VK_LAYER_KHRONOS_validation" ],
			enabledExtensionNames: [ "VK_KHR_swapchain" ],
			enabledFeatures: physicalDevice.Features
		);

		device = deviceCreateInfo.CreateDevice(physicalDevice, allocator);

		static uint find(QueueFamilyProperties[] properties, Func<int, QueueFamilyProperties, bool> predicate) => 
			(uint)properties
			.Index()
			.Where(x => 
			{
				(int i, QueueFamilyProperties q) = x;
				return predicate(i, q);
			})
			.Select(x => 
			{
				(int i, QueueFamilyProperties q) = x;
				return i;
			})
			.First()
		;
	}

	protected virtual void InitializeSwapchain() 
	{
		(int framebufferWidth, int framebufferHeight) = window.FramebufferSize;

		SwapchainProperties swapchainProperties = new(physicalDevice, instance.Surface);
		SurfaceFormat surfaceFormat = swapchainProperties.GetSurfaceFormat(Format.R8G8B8A8SRGB, ColorSpace.SRGBNonlinear);
		PresentMode presentMode = swapchainProperties.GetPresentMode(PresentMode.Mailbox);
		uint imageCount = swapchainProperties.Capabilities.MinImageCount + 1;

		/*Extent2D*/ extent = swapchainProperties.GetExtent(framebufferWidth, framebufferHeight);
		swapchainImageFormat = surfaceFormat.Format;

		if (swapchainProperties.Capabilities.MaxImageCount > 0 && imageCount > swapchainProperties.Capabilities.MaxImageCount)
			imageCount = swapchainProperties.Capabilities.MaxImageCount;

		using var swapchainCreateInfo = new SwapchainCreateInfo(
			type: StructureType.SwapchainCreateInfo,
			next: default,
			flags: default,
			surface: instance.Surface,
			minImageCount: imageCount,
			imageFormat: swapchainImageFormat,
			imageColorSpace: surfaceFormat.ColorSpace,
			imageExtent: extent,
			imageArrayLayers: 1,
			imageUsage: ImageUsageFlags.ColorAttachment,
			imageSharingMode: (graphicsQueueFamilyIndex != presentationQueueFamilyIndex) ? SharingMode.Concurrent : SharingMode.Exclusive,
			queueFamilyIndices: (graphicsQueueFamilyIndex != presentationQueueFamilyIndex) ? [ graphicsQueueFamilyIndex, presentationQueueFamilyIndex ] : [ graphicsQueueFamilyIndex ],
			preTransform: swapchainProperties.Capabilities.CurrentTransform,
			compositeAlpha: CompositeAlphaFlags.Opaque,
			presentMode: presentMode,
			clipped: true,
			oldSwapchain: default
		);

		swapchain = swapchainCreateInfo.CreateSwapchain(device, allocator);
	}

	protected virtual void InitializeImageViews() 
	{
		Image[] swapchainImages = swapchain.GetImages();

		imageViews = new ImageView[swapchainImages.Length];

		for (int i = 0; i < imageViews.Length; i++) 
		{
			var imageViewCreateInfo = new ImageViewCreateInfo(
				type: StructureType.ImageViewCreateInfo,
				next: default,
				flags: default,
				image: swapchainImages[i],
				viewType: ImageViewType.Generic2D,
				format: swapchainImageFormat,
				components: new(ComponentSwizzle.Identity, ComponentSwizzle.Identity, ComponentSwizzle.Identity, ComponentSwizzle.Identity),
				subresourceRange: new(
					aspectMask: ImageAspectFlags.Color,
					baseMipLevel: 0,
					levelCount: 1,
					baseArrayLayer: 0,
					layerCount: 1
				)
			);

			imageViews[i] = imageViewCreateInfo.CreateImageView(device, allocator);
		}
	}

	protected virtual void InitializePipelineLayout() 
	{
		using var pipelineLayoutCreateInfo = new PipelineLayoutCreateInfo(
			type: StructureType.PipelineLayoutCreateInfo,
			next: default,
			flags: default,
			setLayouts: null,
			pushConstantRanges: null
		);

		pipelineLayout = pipelineLayoutCreateInfo.CreatePipelineLayout(device, allocator);
	}

	protected virtual void InitializeRenderPass() 
	{
		var colorAttachment = new AttachmentDescription(
			flags: default,
			format: swapchainImageFormat,
			samples: SampleCount.Bit1,
			loadOp: AttachmentLoadOp.Clear,
			storeOp: AttachmentStoreOp.Store,
			stencilLoadOp: AttachmentLoadOp.DontCare,
			stencilStoreOp: AttachmentStoreOp.DontCare,
			initialLayout: ImageLayout.Undefined,
			finalLayout: ImageLayout.PresentSrc
		);

		var colorAttachmentRef = new AttachmentReference(
			attachment: 0,
			layout: ImageLayout.ColorAttachmentOptimal
		);

		using var subpass = new SubpassDescription(
			flags: default,
			pipelineBindPoint: PipelineBindPoint.Graphics,
			inputAttachments: null,
			colorAttachments: [ colorAttachmentRef ],
			resolveAttachments: null,
			depthStencilAttachment: null,
			preserveAttachments: null
		);

		var dependency = new SubpassDependency(
			srcSubpass: unchecked((uint)-1),
			dstSubpass: 0,
			srcStageMask: PipelineStage.ColorAttachmentOutput,
			dstStageMask: PipelineStage.ColorAttachmentOutput,
			srcAccessMask: 0,
			dstAccessMask: Access.ColorAttachmentWrite,
			dependencyFlags: default
		);

		using var renderPassCreateInfo = new RenderPassCreateInfo(
			type: StructureType.RenderPassCreateInfo,
			next: default,
			flags: default,
			attachments: [ colorAttachment ],
			subpasses: [ subpass ],
			dependencies: [ dependency ]
		);

		renderPass = renderPassCreateInfo.CreateRenderPass(device, allocator);
	}

	protected virtual void InitializePipeline(params (string, ShaderStage)[] shaderDefinitions) 
	{
		var shaders = shaderDefinitions.Select(x => 
			{
				(string filename, ShaderStage stage) = x;

				return (filename, stage, CreateShaderModule(filename, stage));
			}
		).ToArray();

		var stages = shaders.Select(x => 
			{
				(string filename, ShaderStage stage, ShaderModule module) = x;

				return new PipelineShaderStageCreateInfo(
					type: StructureType.PipelineShaderStageCreateInfo,
					next: default,
					flags: default,
					stage: stage,
					module: module,
					name: "main",
					specializationInfo: null
				);
			}
		).ToArray();

		using var vertexInput = new PipelineVertexInputStateCreateInfo(
			type: StructureType.PipelineVertexInputStateCreateInfo,
			next: default,
			flags: default,
			vertexBindingDescriptions: null,
			vertexAttributeDescriptions: null
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
			cullMode: CullMode.Back,
			frontFace: FrontFace.Clockwise,
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
			dynamicStates: [ DynamicState.Viewport, DynamicState.Scissor ]
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

		graphicsPipeline = graphicsPipelineCreateInfo.CreateGraphicsPipeline(device, allocator);

		foreach (var x in stages)
			x.Dispose();

		foreach ((string filename, ShaderStage stage, ShaderModule module) in shaders)
			module.Dispose();
	}

	protected virtual void InitializeFramebuffers() 
	{
		framebuffers = new Framebuffer[imageViews.Length];

		for (int i = 0; i < framebuffers.Length; i++) 
		{
			using var framebufferCreateInfo = new FramebufferCreateInfo(
				type: StructureType.FramebufferCreateInfo,
				next: default,
				flags: default,
				renderPass: renderPass,
				attachments: [ imageViews[i] ],
				width: extent.Width,
				height: extent.Height,
				layers: 1
			);

			framebuffers[i] = framebufferCreateInfo.CreateFramebuffer(device, allocator);
		}
	}

	protected virtual void InitializeCommandPool() 
	{
		var commandPoolCreateInfo = new CommandPoolCreateInfo(
			type: StructureType.CommandPoolCreateInfo,
			next: default,
			flags: CommandPoolCreateFlags.ResetCommandBuffer,
			queueFamilyIndex: graphicsQueueFamilyIndex
		);

		commandPool = commandPoolCreateInfo.CreateCommandPool(device, allocator);
	}

	protected virtual void InitializeCommandBuffers() 
	{
		var commandBufferAllocateInfo = new CommandBufferAllocateInfo(
			type: StructureType.CommandBufferAllocateInfo,
			next: default,
			commandPool: commandPool,
			level: CommandBufferLevel.Primary,
			commandBufferCount: maxFrames
		);

		commandBuffers = commandBufferAllocateInfo.CreateCommandBuffers(device);
	}

	protected virtual void InitializeSyncObjects() 
	{
		var semaphoreCreateInfo = new SemaphoreCreateInfo(
			type: StructureType.SemaphoreCreateInfo,
			next: default,
			flags: default
		);

		var fenceCreateInfo = new FenceCreateInfo(
			type: StructureType.FenceCreateInfo,
			next: default,
			flags: FenceCreateFlags.Signaled
		);

		imageAvailableSemaphore = new Semaphore[maxFrames];
		renderFinishedSemaphore = new Semaphore[maxFrames];
		inFlightFence = new Fence[maxFrames];

		for (int i = 0; i < maxFrames; i++) 
		{
			imageAvailableSemaphore[i] = semaphoreCreateInfo.CreateSemaphore(device, allocator);
			renderFinishedSemaphore[i] = semaphoreCreateInfo.CreateSemaphore(device, allocator);
			inFlightFence[i] = fenceCreateInfo.CreateFence(device, allocator);
		}
	}

	protected virtual void StartRenderPass(uint imageIndex) 
	{
		using var beginInfo = new CommandBufferBeginInfo(
			type: StructureType.CommandBufferBeginInfo,
			next: default,
			flags: default,
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
					color: new(float32: Color.Black, int32: default, uint32: default),
					depthStencil: new(depth: 0f, stencil: 0)
				)
			]
		);

		var viewport = new Viewport(
			x: 0f,
			y: 0f,
			width: (float)extent.Width,
			height: (float)extent.Height,
			minDepth: 0f,
			maxDepth: 1f
		);

		var scissor = new Rect2D(
			offset: new(0, 0),
			extent: extent
		);

		commandBuffers[currentFrame].Begin(beginInfo);
		commandBuffers[currentFrame].BeginRenderPass(renderPassInfo, SubpassContents.Inline);
		commandBuffers[currentFrame].BindPipeline(graphicsPipeline, PipelineBindPoint.Graphics);
		commandBuffers[currentFrame].SetViewports(viewport);
		commandBuffers[currentFrame].SetScissors(scissor);
		commandBuffers[currentFrame].Draw(3, 1, 0, 0);
		commandBuffers[currentFrame].EndRenderPass();
		commandBuffers[currentFrame].End();
	}

	protected ShaderModule CreateShaderModule(string filename, ShaderStage stage) 
	{
		if (filename == null) throw new ArgumentNullException();

		byte[]? code;

		if (!shaderCode.TryGetValue(filename, out code)) 
		{
			code = ShaderCompiler.Compile(filename, stage);
			shaderCode.Add(filename, code!);
		}

		using ShaderModuleCreateInfo shaderModuleCreateInfo = new(
			type: StructureType.ShaderModuleCreateInfo,
			next: default,
			flags: default,
			code: code!
		);

		return shaderModuleCreateInfo.CreateShaderModule(device, allocator);
	}

	public void DeviceWaitIdle() 
	{
		vkDeviceWaitIdle((nint)device);

		[DllImport(VK_LIB)] static extern void vkDeviceWaitIdle(nint device);
	}

	public virtual void DrawFrame() 
	{
		inFlightFence[currentFrame].Wait();
		inFlightFence[currentFrame].Reset();

		uint imageIndex = swapchain.GetNextImage(imageAvailableSemaphore[currentFrame]);

		commandBuffers[currentFrame].Reset(default);

		StartRenderPass(imageIndex);

		using var submitInfo = new SubmitInfo(
			type: StructureType.SubmitInfo,
			next: default,
			waitSemaphores: [ imageAvailableSemaphore[currentFrame] ],
			waitDstStageMasks: [ PipelineStage.ColorAttachmentOutput ],
			commandBuffers: [ commandBuffers[currentFrame] ],
			signalSemaphores: [ renderFinishedSemaphore[imageIndex] ]
		);

		var graphicsQueue = device.GetQueue(graphicsQueueFamilyIndex, 0);
		graphicsQueue.Submit(inFlightFence[currentFrame], submitInfo);

		using var presentInfo = new PresentInfo(
			type: StructureType.PresentInfo,
			next: default,
			waitSemaphores: [ renderFinishedSemaphore[imageIndex] ],
			swapchains: [ swapchain ],
			imageIndices: [ imageIndex ],
			results: null
		);

		var presentationQueue = device.GetQueue(presentationQueueFamilyIndex, 0);
		presentationQueue.Present(presentInfo);

		if (++currentFrame >= maxFrames)
			currentFrame = 0;
	}

	public virtual void Initialize() 
	{
		InitializeInstance();
		InitializeDebugMessages();

		instance.CreateSurface(window);

		InitializePhysicalDevice();
		InitializeDevice();
		InitializeSwapchain();
		InitializeImageViews();
		InitializePipelineLayout();
		InitializeRenderPass();
		InitializePipeline(
			("Shaders/default.vert", ShaderStage.Vertex),
			("Shaders/default.frag", ShaderStage.Fragment)
		);
		InitializeFramebuffers();
		InitializeCommandPool();
		InitializeCommandBuffers();
		InitializeSyncObjects();

		Console.WriteLine("Vulkan Initialized!");
	}

	public void Dispose() 
	{
		foreach (var x in imageAvailableSemaphore)
			x.Dispose();

		foreach (var x in renderFinishedSemaphore)
			x.Dispose();

		foreach (var x in inFlightFence)
			x.Dispose();

		commandPool.Dispose();
		graphicsPipeline.Dispose();
		pipelineLayout.Dispose();
		renderPass.Dispose();

		foreach (var x in framebuffers)
			x.Dispose();

		foreach (var x in imageViews)
			x.Dispose();

		swapchain.Dispose();
		device.Dispose();
		debugUtilsMessenger.Dispose();
		instance.Dispose();

		allocator.Dispose();
	}

	#pragma warning disable CS8618
	public Program(GLFW.Window window, in AllocationCallbacks? allocator) 
	{
		this.window = window;
		this.allocator = (allocator is AllocationCallbacks x) ? new(x) : default;

		this.debugMessageCallback = (DebugUtilsMessageSeverity severity, DebugUtilsMessageType type, in DebugUtilsMessengerCallbackData data, nint userData) => 
		{
			OnDebugMessage?.Invoke(this, new(severity, type, data.Message, data.MessageIdName, data.MessageIdNumber, data.QueueLabels, data.CommandBufferLabels, data.Objects, userData));
			return false;
		};
	}
	#pragma warning restore
}
