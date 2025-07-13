using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vulkan;

public interface IVertex 
{
	int Stride { get; }
	VertexInputBindingDescription BindingDescription { get; }
	VertexInputAttributeDescription[] AttributeDescriptions { get; }
}
