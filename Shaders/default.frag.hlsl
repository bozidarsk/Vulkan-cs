#pragma stage fragment

#include <common.hlsl>

[vk::binding(2)] Texture2D _MainTex : register(t0);
[vk::binding(2)] SamplerState _MainTexSampler : register(s0);

float4 main(Fragment input) 
{
	return _MainTex.Sample(_MainTexSampler, input.uv.xy);

	float depth = input.position.z / input.position.w;

	if (depth < 0)
		return float4(1, 0, 0, 1);
	else if (depth > 1.1)
		return float4(0, 1, 0, 1);
	else if (depth > 1)
		return float4(0, 0, 1, 1);
	else
		return float4(depth.xxx, 1);
}
