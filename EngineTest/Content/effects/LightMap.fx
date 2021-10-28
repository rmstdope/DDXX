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
	float distance	: TEXCOORD1;
};

OUTPUT
DefaultVertexShader(INPUT input)
{
	OUTPUT output;
	
	// transform the position into projection space
	float4 worldPosition = mul(input.position, World[0]);
	float4 viewPosition = mul(worldPosition, View);
	output.position = mul(viewPosition, Projection);

	float3 normal = mul(input.normal, (float3x3)World[0]);
	normal = mul(normal, (float3x3)View);
	normal = normalize(normal);
	output.texCoord = float2(normal.x, normal.y);

	output.distance = -viewPosition.z;// / viewPosition.w;
	output.distance -= 5;
	output.distance /= 10;
	output.distance = clamp(output.distance, 0, 1);
	output.distance = 1 - output.distance;
	
	return output;
}

float4
DefaultPixelShader(OUTPUT input) : COLOR
{
	return input.distance * (float4(0.4, 0.4, 0.4, 0) + tex2D(DefaultEffectSampler, input.texCoord) * float4(DiffuseColor, 1));
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