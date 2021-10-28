#include "CommonVariables.hlsl"

struct inOut
{
	float4 position : POSITION0;
	float4 color : COLOR0;
};

inOut
ColorLineVertexShader(inOut input)
{
	inOut output;
	
    // Transform the position into projection space
    output.position = mul(input.position, World[0]);
    output.position = mul(output.position, View);
    output.position = mul(output.position, Projection);

	// Pass color	
	output.color = input.color;
	
	return output;
}

float4
ColorLinePixelShader(inOut input) : COLOR
{
    return input.color;
}

Technique ColorLine
{
	Pass BasePass
	{
		VertexShader = compile VS_SHADERMODEL ColorLineVertexShader();
		PixelShader = compile PS_SHADERMODEL ColorLinePixelShader();
	}
}