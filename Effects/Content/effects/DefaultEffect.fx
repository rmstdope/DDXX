#include "CommonVariables.hlsl"

sampler2D DefaultEffectSampler = sampler_state
{
    Texture = <Texture>;
    MinFilter = linear;
    MagFilter = linear;
    MipFilter = linear;
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
		VertexShader = compile vs_2_0 DefaultVertexShader();
		PixelShader = compile ps_2_0 DefaultPixelShader();
		ZEnable = true;
		ZWriteEnable = true;
	}
}