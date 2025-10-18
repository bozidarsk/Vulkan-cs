using System;
using System.Runtime.InteropServices;

using static Vulkan.Constants;

namespace Vulkan;

public readonly struct ShaderModuleCreateInfo : IDisposable
{
	public readonly StructureType Type;
	public readonly nint Next;
	public readonly ShaderModuleCreateFlags Flags;
	private readonly nuint codeSize;
	private readonly Handle<byte> code;

	public byte[] Code => code.ToArray((uint)codeSize)!;

	public ShaderModule CreateShaderModule(Device device, AllocationCallbacksHandle allocator) 
	{
		Result result = vkCreateShaderModule(device.Handle, in this, allocator, out ShaderModuleHandle handle);
		if (result != Result.Success) throw new VulkanException(result);

		return handle.GetShaderModule(device, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateShaderModule(DeviceHandle device, in ShaderModuleCreateInfo createInfo, AllocationCallbacksHandle allocator, out ShaderModuleHandle shaderModule);
	}

	public void Dispose() 
	{
		code.Dispose();
	}

	public ShaderModuleCreateInfo(StructureType type, nint next, ShaderModuleCreateFlags flags, byte[] code) 
	{
		if (code == null)
			throw new ArgumentNullException();

		this.Type = type;
		this.Next = next;
		this.Flags = flags;

		this.codeSize = (nuint)code.Length;
		this.code = new(code);
	}
}
