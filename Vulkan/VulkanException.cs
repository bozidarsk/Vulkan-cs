using System;

namespace Vulkan;

public class VulkanException : Exception
{
	public VulkanException() : base("Vulkan error.") {}
	public VulkanException(Result result) : base($"Method returned '{result}'.") {}
	public VulkanException(string? message) : base(message) {}
	public VulkanException(string? message, Exception? innerException) : base(message, innerException) {}
}
