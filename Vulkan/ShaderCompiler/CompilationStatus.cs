namespace Vulkan.ShaderCompiler;

public enum CompilationStatus : int
{
	Success = 0,
	InvalidStage = 1, // error stage deduction
	CompilationError = 2,
	InternalError = 3, // unexpected failure
	NullResultObject = 4,
	InvalidAssembly = 5,
	ValidationError = 6,
	TransformationError = 7,
	ConfigurationError = 8,
}
