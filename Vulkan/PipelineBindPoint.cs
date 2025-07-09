namespace Vulkan;

public enum PipelineBindPoint : uint
{
	Graphics = 0,
	Compute = 1,
	ExecutionGraphAmdx = 1000134000,
	RayTracing = 1000165000,
	SubpassShadingHuawei = 1000369003,
	DataGraphArm = 1000507000,
	RayTracingNv = RayTracing,
}
