#pragma stage fragment
#pragma cull none

#include <common.hlsl>

float4 main(Fragment input)
{
	return float4(input.position.zzz, 1);
}
