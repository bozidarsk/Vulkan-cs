using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct ShaderModuleCreateInfo : IDisposable
{
	public readonly StructureType Type = StructureType.ShaderModuleCreateInfo;
	public readonly nint Next;
	public readonly ShaderModuleCreateFlags Flags;
	private readonly nuint codeSize;
	private readonly Box<byte> code;

	public byte[] Code => code.ToArray((uint)codeSize)!;

	public ShaderModule CreateShaderModule(Device device, AllocationCallbacks? allocator)
	{
		Result result = vkCreateShaderModule(device.Handle, in this, allocator?.Handle ?? default, out ShaderModuleHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(handle, device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateShaderModule(DeviceHandle device, in ShaderModuleCreateInfo createInfo, AllocationCallbacksHandle allocator, out ShaderModuleHandle shaderModule);
	}

	public void Dispose()
	{
		code.Dispose();
	}

	public ShaderModuleCreateInfo(nint next, ShaderModuleCreateFlags flags, byte[] code)
	{
		if (code == null)
			throw new ArgumentNullException();

		this.Next = next;
		this.Flags = flags;

		this.codeSize = (nuint)code.Length;
		this.code = new(code);
	}
}
