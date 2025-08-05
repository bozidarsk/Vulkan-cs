using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vulkan;

public partial class Program : IDisposable
{
	protected readonly GLFW.Window window;
	protected readonly Handle<AllocationCallbacks> allocator = default;

	protected Queue<IDisposable>[] toBeDisposed;
	protected uint graphicsQueueFamilyIndex, presentationQueueFamilyIndex;
	protected Format swapchainImageFormat, depthFormat;
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
	protected Framebuffer[] framebuffers;
	protected CommandPool commandPool;
	protected CommandBuffer[] commandBuffers;
	protected Semaphore[] imageAvailableSemaphore, renderFinishedSemaphore;
	protected Fence[] inFlightFence;
	protected Queue graphicsQueue, presentationQueue;
	protected DescriptorSetLayout[] descriptorSetLayouts;
	protected DescriptorPool descriptorPool;
	protected DescriptorSet[] descriptorSets;
	protected Buffer[] globalUniformsBuffers;
	protected DeviceMemory[] globalUniformsMemories;
	protected nint[] globalUniformsLocations;
	protected Image depthImage;
	protected ImageView depthImageView;

	public static uint MakeVersion(int major, int minor, int patch) => ((((uint)major) << 22) | (((uint)minor) << 12) | ((uint)patch));
	public static uint MakeApiVersion(int variant, int major, int minor, int patch) => ((((uint)variant) << 29) | (((uint)major) << 22) | (((uint)minor) << 12) | ((uint)patch));

	private readonly DebugUtilsMessengerCallback debugMessageCallback;
	public event DebugEventHandler? OnDebugMessage;

	protected uint FindMemoryType(uint typeFilter, MemoryProperty properties) 
	{
		var memProperties = physicalDevice.MemoryProperties;
		int i = 0;

		foreach (var x in memProperties.MemoryTypes) 
		{
			if ((typeFilter & (1 << i)) != 0 && x.Properties.HasFlag(properties))
				return (uint)i;

			i++;
		}

		throw new VulkanException("Failed to find suitable memory type.");
	}

	protected Format FindSupportedFormat(Format[] candidates, ImageTiling tiling, FormatFeatures features) 
	{
		foreach (var format in candidates) 
		{
			var properties = physicalDevice.GetFormatProperties(format);

			if (tiling == ImageTiling.Linear && properties.LinearTilingFeatures.HasFlag(features))
				return format;

			if (tiling == ImageTiling.Optimal && properties.OptimalTilingFeatures.HasFlag(features))
				return format;
		}

		throw new NullReferenceException("Failed to find supported format.");
	}

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
		extensions.Add("VK_KHR_get_physical_device_properties2");

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

		using var extendedDynamicStateFeatures = new Handle<PhysicalDeviceExtendedDynamicStateFeatures>(
			new PhysicalDeviceExtendedDynamicStateFeatures(
				type: StructureType.PhysicalDeviceExtendedDynamicStateFeatures,
				next: default,
				extendedDynamicState: true
			)
		);

		using var indexTypeUInt8Features = new Handle<PhysicalDeviceIndexTypeUInt8Features>(
			new PhysicalDeviceIndexTypeUInt8Features(
				type: StructureType.PhysicalDeviceIndexTypeUInt8Features,
				next: extendedDynamicStateFeatures,
				indexTypeUInt8: true
			)
		);

		// using var vertexInputDynamicStateFeatures = new Handle<PhysicalDeviceVertexInputDynamicStateFeatures>(
		// 	new PhysicalDeviceVertexInputDynamicStateFeatures(
		// 		type: StructureType.PhysicalDeviceVertexInputDynamicStateFeaturesExt,
		// 		next: indexTypeUInt8Features,
		// 		vertexInputDynamicState: true
		// 	)
		// );

		using var deviceCreateInfo = new DeviceCreateInfo(
			type: StructureType.DeviceCreateInfo,
			next: indexTypeUInt8Features/*vertexInputDynamicStateFeatures*/,
			flags: default,
			queueCreateInfos: (graphicsQueueFamilyIndex != presentationQueueFamilyIndex) ? [ graphicsDeviceQueueCreateInfo, presentationDeviceQueueCreateInfo ] : [ graphicsDeviceQueueCreateInfo ],
			enabledLayerNames: [ "VK_LAYER_KHRONOS_validation" ],
			enabledExtensionNames: [ "VK_KHR_swapchain"/*, "VK_EXT_vertex_input_dynamic_state"*/, "VK_EXT_index_type_uint8", "VK_EXT_extended_dynamic_state", "VK_KHR_push_descriptor" ],
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
			imageUsage: ImageUsage.ColorAttachment,
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
			CreateImageView(swapchainImages[i], swapchainImageFormat, out imageViews[i]);
	}

	protected virtual void InitializeDescriptorSetLayout() 
	{
		var globalUniformsBinding = new DescriptorSetLayoutBinding(
			binding: 0,
			descriptorType: DescriptorType.UniformBuffer,
			descriptorCount: 1,
			stage: ShaderStage.AllGraphics,
			immutableSamplers: null
		);

		var objectUniformsBinding = new DescriptorSetLayoutBinding(
			binding: 1,
			descriptorType: DescriptorType.UniformBuffer,
			descriptorCount: 1,
			stage: ShaderStage.AllGraphics,
			immutableSamplers: null
		);

		var samplersBinding = new DescriptorSetLayoutBinding(
			binding: 2,
			descriptorType: DescriptorType.CombinedImageSampler,
			descriptorCount: 1,
			stage: ShaderStage.AllGraphics,
			immutableSamplers: null
		);

		using var descriptorSetLayoutCreateInfo = new DescriptorSetLayoutCreateInfo(
			type: StructureType.DescriptorSetLayoutCreateInfo,
			next: default,
			flags: DescriptorSetLayoutCreateFlags.PushDescriptor,
			bindings: [ globalUniformsBinding, objectUniformsBinding, samplersBinding ]
		);

		descriptorSetLayouts = new DescriptorSetLayout[maxFrames];

		for (int i = 0; i < maxFrames; i++)
			descriptorSetLayouts[i] = descriptorSetLayoutCreateInfo.CreateDescriptorSetLayout(device, allocator);
	}

	protected virtual void InitializeDescriptorPool() 
	{
		using var descriptorPoolCreateInfo = new DescriptorPoolCreateInfo(
			type: StructureType.DescriptorPoolCreateInfo,
			next: default,
			flags: DescriptorPoolCreateFlags.FreeDescriptorSet,
			maxSets: maxFrames,
			poolSizes: [ new(DescriptorType.UniformBuffer, maxFrames * 2) ]
		);

		descriptorPool = descriptorPoolCreateInfo.CreateDescriptorPool(device, allocator);
	}

	protected virtual void InitialzieDescriptorSets() 
	{
		using var descriptorSetAllocateInfo = new DescriptorSetAllocateInfo(
			type: StructureType.DescriptorSetAllocateInfo,
			next: default,
			descriptorPool: descriptorPool,
			setLayouts: descriptorSetLayouts
		);

		descriptorSets = descriptorSetAllocateInfo.CreateDescriptorSets(device, descriptorPool);
	}

	protected virtual void InitializeGlobalUniforms() 
	{
		DeviceSize size = (ulong)Marshal.SizeOf<GlobalUniforms>();

		globalUniformsBuffers = new Buffer[maxFrames];
		globalUniformsMemories = new DeviceMemory[maxFrames];
		globalUniformsLocations = new nint[maxFrames];

		for (int i = 0; i < maxFrames; i++) 
		{
			CreateBuffer(size, BufferUsage.UniformBuffer, MemoryProperty.HostVisible | MemoryProperty.HostCoherent, out Buffer buffer, out DeviceMemory memory);

			globalUniformsBuffers[i] = buffer;
			globalUniformsMemories[i] = memory;
			globalUniformsLocations[i] = memory.Map(size: size, offset: default, flags: default);
		}
	}

	protected virtual void InitializePipelineLayout() 
	{
		using var pipelineLayoutCreateInfo = new PipelineLayoutCreateInfo(
			type: StructureType.PipelineLayoutCreateInfo,
			next: default,
			flags: default,
			setLayouts: [ descriptorSetLayouts[0] ],
			pushConstantRanges: [ new(stage: ShaderStage.All, offset: 0, size: 128) ]
		);

		pipelineLayout = pipelineLayoutCreateInfo.CreatePipelineLayout(device, allocator);
	}

	protected virtual void InitializeDepthImage() 
	{
		depthFormat = FindSupportedFormat(
			[ Format.D32SFloat, Format.D32SFloatS8UInt, Format.D24UNormS8UInt ],
			ImageTiling.Optimal,
			FormatFeatures.DepthStencilAttachment
		);

		using var imageCreateInfo = new ImageCreateInfo(
			type: StructureType.ImageCreateInfo,
			next: default,
			flags: default,
			imageType: ImageType.Generic2D,
			format: depthFormat,
			extent: new(extent.Width, extent.Height, 1),
			mipLevels: 1,
			arrayLayers: 1,
			samples: SampleCount.Bit1,
			tiling: ImageTiling.Optimal,
			usage: ImageUsage.DepthStencilAttachment,
			sharingMode: SharingMode.Exclusive,
			queueFamilyIndices: null,
			initialLayout: ImageLayout.Undefined
		);

		depthImage = imageCreateInfo.CreateImage(device, allocator);

		var imageViewCreateInfo = new ImageViewCreateInfo(
			type: StructureType.ImageViewCreateInfo,
			next: default,
			flags: default,
			image: depthImage,
			viewType: ImageViewType.Generic2D,
			format: depthFormat,
			components: default,
			subresourceRange: new(
				aspect: ImageAspect.Depth,
				baseMipLevel: 0,
				levelCount: 1,
				baseArrayLayer: 0,
				layerCount: 1
			)
		);

		vkGetImageMemoryRequirements(device.Handle, depthImage.Handle, out MemoryRequirements requirements);

		var allocateInfo = new MemoryAllocateInfo(
			type: StructureType.MemoryAllocateInfo,
			next: default,
			allocationSize: requirements.Size,
			memoryTypeIndex: FindMemoryType(requirements.MemoryType, MemoryProperty.DeviceLocal)
		);

		DeviceMemory depthImageMemory = allocateInfo.CreateDeviceMemory(device, allocator);
		vkBindImageMemory(device.Handle, depthImage.Handle, depthImageMemory.Handle, default);

		depthImageView = imageViewCreateInfo.CreateImageView(device, allocator);

		[DllImport(Vulkan.Constants.VK_LIB)] static extern void vkGetImageMemoryRequirements(DeviceHandle device, ImageHandle image, out MemoryRequirements requirements);
		[DllImport(Vulkan.Constants.VK_LIB)] static extern void vkBindImageMemory(DeviceHandle device, ImageHandle image, DeviceMemoryHandle memory, DeviceSize offset);
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

		var depthAttachment = new AttachmentDescription(
			flags: default,
			format: depthFormat,
			samples: SampleCount.Bit1,
			loadOp: AttachmentLoadOp.Clear,
			storeOp: AttachmentStoreOp.DontCare,
			stencilLoadOp: AttachmentLoadOp.DontCare,
			stencilStoreOp: AttachmentStoreOp.DontCare,
			initialLayout: ImageLayout.Undefined,
			finalLayout: ImageLayout.DepthStencilAttachmentOptimal
		);

		var depthAttachmentRef = new AttachmentReference(
			attachment: 1,
			layout: ImageLayout.DepthStencilAttachmentOptimal
		);

		using var subpass = new SubpassDescription(
			flags: default,
			pipelineBindPoint: PipelineBindPoint.Graphics,
			inputAttachments: null,
			colorAttachments: [ colorAttachmentRef ],
			resolveAttachments: null,
			depthStencilAttachment: depthAttachmentRef,
			preserveAttachments: null
		);

		var dependency = new SubpassDependency(
			srcSubpass: unchecked((uint)-1),
			dstSubpass: 0,
			srcStageMask: PipelineStage.ColorAttachmentOutput | PipelineStage.EarlyFragmentTests,
			dstStageMask: PipelineStage.ColorAttachmentOutput | PipelineStage.EarlyFragmentTests,
			srcAccessMask: 0,
			dstAccessMask: Access.ColorAttachmentWrite | Access.DepthStencilAttachmentWrite,
			dependencyFlags: default
		);

		using var renderPassCreateInfo = new RenderPassCreateInfo(
			type: StructureType.RenderPassCreateInfo,
			next: default,
			flags: default,
			attachments: [ colorAttachment, depthAttachment ],
			subpasses: [ subpass ],
			dependencies: [ dependency ]
		);

		renderPass = renderPassCreateInfo.CreateRenderPass(device, allocator);
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
				attachments: [ imageViews[i], depthImageView ],
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

		commandBuffers = commandBufferAllocateInfo.CreateCommandBuffers(device, commandPool);
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

	public void DeviceWaitIdle() => device.WaitIdle();

	public void RecreateSwapchain() 
	{
		(int framebufferWidth, int framebufferHeight) = window.FramebufferSize;
		while (framebufferWidth == 0 || framebufferHeight == 0) 
		{
			(framebufferWidth, framebufferHeight) = window.FramebufferSize;
			GLFW.Input.WaitForEvents();
		}

		DeviceWaitIdle();

		foreach (var x in framebuffers)
			x.Dispose();

		foreach (var x in imageViews)
			x.Dispose();

		swapchain.Dispose();

		InitializeSwapchain();
		InitializeImageViews();
		InitializeFramebuffers();
	}

	public virtual void Initialize() 
	{
		InitializeInstance();
		ExtensionDelegates.Initialize(instance);
		InitializeDebugMessages();

		instance.CreateSurface(window);

		InitializePhysicalDevice();
		InitializeDevice();
		InitializeSwapchain();
		InitializeImageViews();
		InitializeDescriptorSetLayout();
		// InitializeDescriptorPool();
		// InitialzieDescriptorSets();
		InitializeGlobalUniforms();
		InitializePipelineLayout();
		InitializeDepthImage();
		InitializeRenderPass();
		InitializeFramebuffers();
		InitializeCommandPool();
		InitializeCommandBuffers();
		InitializeSyncObjects();

		toBeDisposed = new Queue<IDisposable>[maxFrames];
		for (int i = 0; i < maxFrames; i++)
			toBeDisposed[i] = new();

		graphicsQueue = device.GetQueue(graphicsQueueFamilyIndex, 0);
		presentationQueue = device.GetQueue(presentationQueueFamilyIndex, 0);

		Console.WriteLine("Vulkan Initialized!");
	}

	public void Dispose() 
	{
		foreach (var x in toBeDisposed)
			while (x.Count > 0)
				x.Dequeue().Dispose();

		foreach (var x in globalUniformsMemories)
			x.Unmap();

		foreach (var x in imageAvailableSemaphore)
			x.Dispose();

		foreach (var x in renderFinishedSemaphore)
			x.Dispose();

		foreach (var x in inFlightFence)
			x.Dispose();

		commandPool.Dispose();
		pipelineLayout.Dispose();
		renderPass.Dispose();

		foreach ((RenderInfo i, Pipeline p) in graphicsPipelines)
			p.Dispose();

		foreach (var x in framebuffers)
			x.Dispose();

		foreach (var x in imageViews)
			x.Dispose();

		swapchain.Dispose();

		foreach (var x in globalUniformsBuffers)
			x.Dispose();
		foreach (var x in globalUniformsMemories)
			x.Dispose();

		foreach (var x in descriptorSets ?? [])
			x.Dispose();

		descriptorPool?.Dispose();

		foreach (var x in descriptorSetLayouts)
			x.Dispose();

		device.Dispose();
		debugUtilsMessenger.Dispose();
		instance.Dispose();

		allocator.Dispose();
	}

	#pragma warning disable CS8618
	public Program(GLFW.Window window) 
	{
		this.window = window;

		this.debugMessageCallback = (DebugUtilsMessageSeverity severity, DebugUtilsMessageType type, in DebugUtilsMessengerCallbackData data, nint userData) => 
		{
			OnDebugMessage?.Invoke(this, new(severity, type, data.Message, data.MessageIdName, data.MessageIdNumber, data.QueueLabels, data.CommandBufferLabels, data.Objects, userData));
			return false;
		};
	}

	public Program(GLFW.Window window, in AllocationCallbacks? allocator) : this(window) => 
		this.allocator = (allocator is AllocationCallbacks x) ? new(x) : default
	;
	#pragma warning restore
}
