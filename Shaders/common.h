// #pragma stage {Vulkan.ShaderStage} [main|...]
// #pragma cull {Vulkan.CullMode}
// #pragma frontface {Vulkan.FrontFace}
// #pragma blend {Vulkan.BlendFactor} {Vulkan.BlendFactor}
// #pragma language {glsl|hlsl}

#ifndef COMMON_H
#define COMMON_H

struct Vertex 
{
	float3 position;
	float4 color;
};

struct Fragment 
{
	float4 position : SV_POSITION;
	float4 color;
};

#endif
