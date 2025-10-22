#pragma stage fragment
#pragma cull none

#include <common.hlsl>

float4 main(Fragment input) 
{
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
