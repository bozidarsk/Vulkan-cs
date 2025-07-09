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

	public ShaderModule CreateShaderModule(Device device, Handle<AllocationCallbacks> allocator) 
	{
		Result result = vkCreateShaderModule((nint)device, in this, allocator, out nint shaderModuleHandle);
		if (result != Result.Success) throw new VulkanException(result);

		return new(device, shaderModuleHandle, allocator);

		[DllImport(VK_LIB)] static extern Result vkCreateShaderModule(nint device, in ShaderModuleCreateInfo createInfo, nint allocator, out nint shaderModule);
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
