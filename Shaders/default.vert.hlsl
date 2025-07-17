#pragma stage vertex
#pragma cull none

#include <common.h>

Fragment main(Vertex input) 
{
	Fragment output;

	output.color = input.color;
	output.position = float4(input.position, 1);
	output.position.y *= -1;

	return output;
}
