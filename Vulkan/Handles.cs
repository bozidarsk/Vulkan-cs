global using AllocationCallbacksHandle = Vulkan.Handle<Vulkan.AllocationCallbacks>;
global using BufferHandle = Vulkan.Handle<Vulkan.Buffer>;
global using BufferViewHandle = Vulkan.Handle<Vulkan.BufferView>;
global using CommandBufferHandle = Vulkan.Handle<Vulkan.CommandBuffer>;
global using CommandPoolHandle = Vulkan.Handle<Vulkan.CommandPool>;
global using DebugUtilsMessengerHandle = Vulkan.Handle<Vulkan.DebugUtilsMessenger>;
global using DescriptorPoolHandle = Vulkan.Handle<Vulkan.DescriptorPool>;
global using DescriptorSetHandle = Vulkan.Handle<Vulkan.DescriptorSet>;
global using DescriptorSetLayoutHandle = Vulkan.Handle<Vulkan.DescriptorSetLayout>;
global using DeviceHandle = Vulkan.Handle<Vulkan.Device>;
global using DeviceMemoryHandle = Vulkan.Handle<Vulkan.DeviceMemory>;
global using FenceHandle = Vulkan.Handle<Vulkan.Fence>;
global using FramebufferHandle = Vulkan.Handle<Vulkan.Framebuffer>;
global using ImageHandle = Vulkan.Handle<Vulkan.Image>;
global using ImageViewHandle = Vulkan.Handle<Vulkan.ImageView>;
global using InstanceHandle = Vulkan.Handle<Vulkan.Instance>;
global using PhysicalDeviceHandle = Vulkan.Handle<Vulkan.PhysicalDevice>;
global using PipelineHandle = Vulkan.Handle<Vulkan.Pipeline>;
global using PipelineLayoutHandle = Vulkan.Handle<Vulkan.PipelineLayout>;
global using PipelineCacheHandle = Vulkan.Handle<Vulkan.PipelineCache>;
global using QueueHandle = Vulkan.Handle<Vulkan.Queue>;
global using RenderPassHandle = Vulkan.Handle<Vulkan.RenderPass>;
global using SamplerHandle = Vulkan.Handle<Vulkan.Sampler>;
global using SemaphoreHandle = Vulkan.Handle<Vulkan.Semaphore>;
global using ShaderModuleHandle = Vulkan.Handle<Vulkan.ShaderModule>;
global using SurfaceHandle = Vulkan.Handle<Vulkan.Surface>;
global using SwapchainHandle = Vulkan.Handle<Vulkan.Swapchain>;
global using QueryPoolHandle = Vulkan.Handle<Vulkan.QueryPool>;

namespace Vulkan;

internal readonly struct Handle<T> where T : class
{
	private readonly nint value;

	public static explicit operator nint(Handle<T> handle) => handle.value;

	public static bool operator ==(Handle<T> a, Handle<T> b) => a.value == b.value;
	public static bool operator !=(Handle<T> a, Handle<T> b) => a.value != b.value;
	public override bool Equals(object? other) => (other is Handle<T> x) ? x.value == value : false;

	public override string ToString() => $"0x{value:x}";
	public override int GetHashCode() => value.GetHashCode();
}
