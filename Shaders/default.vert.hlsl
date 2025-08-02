#pragma stage vertex
#pragma cull none

#include <common.hlsl>

Fragment main(Vertex input) 
{
	Fragment output;

	output.color = float4(input.position, 1);
	output.position = PROJECTION * (VIEW * (MODEL * float4(input.position, 1)));

	return output;
}
