// #pragma stage {Vulkan.ShaderStage} [main|...]
// #pragma cull {Vulkan.CullMode}
// #pragma frontface {Vulkan.FrontFace}
// #pragma blend {{disable|off}|{Vulkan.BlendFactor} [Vulkan.BlendOp] {Vulkan.BlendFactor}}
// #pragma language {glsl|hlsl}

#ifndef COMMON_HLSL
#define COMMON_HLSL

struct Vertex 
{
	float3 position;
	float3 normal;
	float2 uv;
	float4 color;
};

struct Fragment 
{
	float4 position : SV_POSITION;
	float3 normal;
	float2 uv;
	float4 color;
};

cbuffer GlobalUniforms : register(b0)
{
	float4x4 VIEW;
	float4x4 PROJECTION;
}

[[vk::push_constant]]
cbuffer PushConstants 
{
	float4x4 MODEL;
	float3 CAMERA_POSITION;
}

#endif
