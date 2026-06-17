global using CompilerHandle = Vulkan.ShaderCompiler.Handle<Vulkan.ShaderCompiler.Compiler>;
global using CompilerOptionsHandle = Vulkan.ShaderCompiler.Handle<Vulkan.ShaderCompiler.CompilerOptions>;
global using CompilationResultHandle = Vulkan.ShaderCompiler.Handle<Vulkan.ShaderCompiler.CompilationResult>;

namespace Vulkan.ShaderCompiler;

#pragma warning disable CS0649

internal readonly struct Handle<T> where T : class
{
	private readonly nint value;

	public static bool operator ==(Handle<T> a, Handle<T> b) => a.value == b.value;
	public static bool operator !=(Handle<T> a, Handle<T> b) => a.value != b.value;
	public override bool Equals(object? other) => (other is Handle<T> x) ? x.value == value : false;

	public override string ToString() => $"0x{value:x}";
	public override int GetHashCode() => value.GetHashCode();
}
