#include "../../../Effects/Content/effects/CommonVariables.hlsl"

sampler2D DefaultEffectSampler = sampler_state
{
	Texture	= <Texture>;
	MinFilter	= linear;
	MagFilter	= linear;
	MipFilter	= linear;
	AddressU	= Wrap;
	AddressV	= Wrap;
};

struct INPUT
{
	float4 position : POSITION0;
	float3 normal		: NORMAL0;
	float2 texCoord : TEXCOORD0;
};

struct OUTPUT
{
	float4 position : POSITION0;
	float2 texCoord : TEXCOORD0;
};

OUTPUT
DefaultVertexShader(INPUT input)
{
	OUTPUT output;
	
	// transform the position into projection space
	output.position = mul(input.position, World[0]);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);

	float3 normal = mul(input.normal, (float3x3)World[0]);
	normal = mul(normal, (float3x3)View);
	normal = normalize(normal);
	output.texCoord = float2(normal.x, normal.y);
	
	return output;
}

float4
DefaultPixelShader(OUTPUT input) : COLOR
{
	return float4(0.4, 0.4, 0.4, 0) + tex2D(DefaultEffectSampler, input.texCoord) * float4(DiffuseColor, 1);
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