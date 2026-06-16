global using AllocationCallbacksHandle = Vulkan.Handle;
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
global using PhysicalDeviceHandle = Vulkan.Handle;
global using PipelineHandle = Vulkan.Handle;
global using PipelineLayoutHandle = Vulkan.Handle;
global using PipelineCacheHandle = Vulkan.Handle;
global using QueueHandle = Vulkan.Handle;
global using RenderPassHandle = Vulkan.Handle;
global using SamplerHandle = Vulkan.Handle;
global using SemaphoreHandle = Vulkan.Handle;
global using ShaderModuleHandle = Vulkan.Handle;
global using SurfaceHandle = Vulkan.Handle;
global using SwapchainHandle = Vulkan.Handle;

namespace Vulkan;

#pragma warning disable CS0649

internal readonly struct Handle
{
	private readonly nint value;

	public static bool operator ==(Handle a, Handle b) => a.value == b.value;
	public static bool operator !=(Handle a, Handle b) => a.value != b.value;
	public override bool Equals(object? other) => (other is Handle x) ? x.value == value : false;

	public override string ToString() => value.ToString();
	public override int GetHashCode() => value.GetHashCode();
}
