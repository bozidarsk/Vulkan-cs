#include <common.h>

Fragment main(Vertex input) 
{
	Fragment output;

	output.position = float4(input.position, 1);
	output.color = input.color;

	return output;
}
