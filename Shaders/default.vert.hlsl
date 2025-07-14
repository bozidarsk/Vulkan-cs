#include <common.h>

Fragment main(Vertex input) 
{
	Fragment output;

	float4 transform = float4(0, 15, 0, 0.8);
	transform.xyz *= 3.14 / 180;

	float4x4 scale =  
	{
		0.2, 0.0, 0.0, 0.0,
		0.0, 0.6, 0.0, 0.0,
		0.0, 0.0, 1, 0.0,
		0.0, 0.0, 0.0, 1.0,
	};
	float4x4 rotx = 
	{
		1.0, 0.0, 0.0, 0.0,
		0.0, cos(transform.x), -sin(transform.x), 0.0,
		0.0, sin(transform.x), cos(transform.x), 0.0,
		0.0, 0.0, 0.0, 1.0,
	};
	float4x4 roty = 
	{
		cos(transform.y), 0.0, sin(transform.y), 0.0,
		0.0, 1, 0.0, 0.0,
		-sin(transform.y), 0.0, cos(transform.y), 0.0,
		0.0, 0.0, 0.0, 1.0,
	};
	float4x4 rotz = 
	{
		cos(transform.z), -sin(transform.z), 0.0, 0.0,
		sin(transform.z), cos(transform.z), 0.0, 0.0,
		0.0, 0.0, 1.0, 0.0,
		0.0, 0.0, 0.0, 1.0,
	};

	output.color = input.position.xyzz;
	output.position = (rotx * roty * rotz * scale) * float4(input.position, 1);
	output.position.y *= -1;

	return output;
}
