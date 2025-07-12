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
