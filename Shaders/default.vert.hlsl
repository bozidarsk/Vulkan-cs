#include <common.h>

Fragment main(/*Vertex input*/ uint vertexIndex : SV_VertexID) 
{
	Fragment output;

	if (vertexIndex == 0) 
	{
		output.position = float4(0.0, -0.5, 0, 0);
	}
	if (vertexIndex == 1) 
	{
		output.position = float4(0.5, 0.5, 0, 0);
	}
	if (vertexIndex == 2) 
	{
		output.position = float4(-0.5, 0.5, 0, 0);
	}

	output.color = float4(float(vertexIndex).rrr / 5, 1);

	return output;
}
