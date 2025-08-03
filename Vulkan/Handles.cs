global using BufferHandle = Vulkan.Handle;
global using BufferViewHandle = Vulkan.Handle;
global using CommandBufferHandle = Vulkan.Handle;
global using CommandPoolHandle = Vulkan.Handle;
global using DebugEventArgsHandle = Vulkan.Handle;
global using DebugUtilsMessengerHandle = Vulkan.Handle;
global using DescriptorPoolHandle = Vulkan.Handle;
global using DescriptorSetHandle = Vulkan.Handle;
global using DescriptorSetLayoutHandle = Vulkan.Handle;
global using DeviceHandle = Vulkan.Handle;
global using DeviceMemoryHandle = Vulkan.Handle;
global using FenceHandle = Vulkan.Handle;
global using FramebufferHandle = Vulkan.Handle;
global using ImageHandle = Vulkan.Handle;
global using ImageViewHandle = Vulkan.Handle;
global using InstanceHandle = Vulkan.Handle;
global using PipelineHandle = Vulkan.Handle;
global using PipelineLayoutHandle = Vulkan.Handle;
global using RenderPassHandle = Vulkan.Handle;
global using SamplerHandle = Vulkan.Handle;
global using SemaphoreHandle = Vulkan.Handle;
global using ShaderModuleHandle = Vulkan.Handle;
global using SwapchainHandle = Vulkan.Handle;
global using SwapchainPropertiesHandle = Vulkan.Handle;

namespace Vulkan;

#pragma warning disable CS0649

internal readonly struct Handle 
{
	private readonly nint value;

	public static bool operator == (Handle a, Handle b) => a.value == b.value;
	public static bool operator != (Handle a, Handle b) => a.value != b.value;
	public override bool Equals(object? other) => (other is Handle x) ? x.value == value : false;

	public override string ToString() => value.ToString();
	public override int GetHashCode() => value.GetHashCode();
}

internal static class HandleExtensions 
{
	public static Buffer GetBuffer(this BufferHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new Buffer(handle, device, allocator);
	public static CommandBuffer GetCommandBuffer(this CommandBufferHandle handle) => new CommandBuffer(handle);
	public static CommandPool GetCommandPool(this CommandPoolHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new CommandPool(handle, device, allocator);
	public static DescriptorPool GetDescriptorPool(this DescriptorPoolHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new DescriptorPool(handle, device, allocator);
	public static DebugUtilsMessenger GetDebugUtilsMessenger(this DebugUtilsMessengerHandle handle, Instance instance, Handle<AllocationCallbacks> allocator) => new DebugUtilsMessenger(handle, instance, allocator);
	public static DescriptorSet GetDescriptorSet(this DescriptorSetHandle handle, Device device, DescriptorPool descriptorPool) => new DescriptorSet(handle, device, descriptorPool);
	public static DescriptorSetLayout GetDescriptorSetLayout(this DescriptorSetLayoutHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new DescriptorSetLayout(handle, device, allocator);
	public static Device GetDevice(this DeviceHandle handle, Handle<AllocationCallbacks> allocator) => new Device(handle, allocator);
	public static DeviceMemory GetDeviceMemory(this DeviceMemoryHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new DeviceMemory(handle, device, allocator);
	public static Fence GetFence(this FenceHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new Fence(handle, device, allocator);
	public static Framebuffer GetFramebuffer(this FramebufferHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new Framebuffer(handle, device, allocator);
	public static Image GetImage(this ImageHandle handle) => new Image(handle);
	public static ImageView GetImageView(this ImageViewHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new ImageView(handle, device, allocator);
	public static Instance GetInstance(this InstanceHandle handle, Handle<AllocationCallbacks> allocator) => new Instance(handle, allocator);
	public static Pipeline GetPipeline(this PipelineHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new Pipeline(handle, device, allocator);
	public static PipelineLayout GetPipelineLayout(this PipelineLayoutHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new PipelineLayout(handle, device, allocator);
	public static RenderPass GetRenderPass(this RenderPassHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new RenderPass(handle, device, allocator);
	public static Sampler GetSampler(this SamplerHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new Sampler(handle, device, allocator);
	public static Semaphore GetSemaphore(this SemaphoreHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new Semaphore(handle, device, allocator);
	public static ShaderModule GetShaderModule(this ShaderModuleHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new ShaderModule(handle, device, allocator);
	public static Swapchain GetSwapchain(this SwapchainHandle handle, Device device, Handle<AllocationCallbacks> allocator) => new Swapchain(handle, device, allocator);
}
