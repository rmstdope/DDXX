#include "CommonVariables.hlsl"

sampler2D DefaultEffectSampler = sampler_state
{
    Texture	= <Texture>;
    MinFilter	= linear;
    MagFilter	= linear;
    MipFilter	= linear;
	AddressU	= Mirror;
	AddressV	= Mirror;
};

struct INPUT
{
	float4 position : POSITION0;
	float2 texCoord : TEXCOORD0;
};

INPUT
DefaultVertexShader(INPUT input)
{
	INPUT output;
	
	// transform the position into projection space
	output.position = mul(input.position, World[0]);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);
	
	output.texCoord = input.texCoord;
	
	return output;
}

float4
DefaultPixelShader(INPUT input) : COLOR
{
	return tex2D(DefaultEffectSampler, input.texCoord);
}

Technique Default
{
	Pass BasePass
	{
		VertexShader = compile VS_SHADERMODEL DefaultVertexShader();
		PixelShader = compile PS_SHADERMODEL DefaultPixelShader();
		ZEnable = true;
		ZWriteEnable = true;
	}
}