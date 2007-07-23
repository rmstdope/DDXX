#ifndef ATMOSPHERE_SHADER_FX
#define ATMOSPHERE_SHADER_FX

/**
 *
 * Atmosphere shader
 *
*/

#include "SharedVariables.hlsl"

/***********************************************************************************
 * ------------------
 * Variables
 * ------------------
***********************************************************************************/

float4 AtmosphereSunColor;
float4 AtmosphereSkyColor = float4(0.2, 0.2, 0.4, 0) * 0.5;
float4 AtmosphereCloudColor1 = float4(0.5, 0.5, 0.2, 0) * 1;
float4 AtmosphereCloudColor2 = float4(0.5, 0.5, 0.2, 0) * 1;
float2 AtmosphereSunPosition;

float4 AtmosphereTime = float4(1, 1, 1, 1);

struct AtmosphereInputVS
{
	float4 Position				: POSITION0;
	float2 TextureCoords	: TEXCOORD0;
};

struct AtmosphereInputPS
{
	float4 Position					: POSITION0;
	float2 TextureCoords[4]	: TEXCOORD0;
};

AtmosphereInputPS
AtmosphereVertexShader(AtmosphereInputVS Input)
{
	AtmosphereInputPS Output;
	
	// Transform coordinates
	Output.Position = mul(Input.Position, WorldViewProjectionT);

	Output.TextureCoords[0] = Input.TextureCoords + AtmosphereTime.xy;
	Output.TextureCoords[1] = Input.TextureCoords - AtmosphereTime.xy;
	Output.TextureCoords[2] = Input.TextureCoords + AtmosphereTime.zw;
	Output.TextureCoords[3] = Input.TextureCoords - AtmosphereTime.zw;

	return Output;
}

float4
AtmospherePixelShader(AtmosphereInputPS input) : COLOR
{
	float4 s1 = tex2D(BaseTextureSamplerMirrored, input.TextureCoords[1]);
	float4 s2 = tex2D(BaseTextureSamplerMirrored, input.TextureCoords[3]);
  s1 = (s1 - 0.5) * 2;
  s2 = (s2 - 0.5) * 2;
	float4 v1 = tex2D(BaseTextureSamplerMirrored, input.TextureCoords[0] + s1 * float2(0.12, 0.03));
	float4 v2 = tex2D(BaseTextureSamplerMirrored, input.TextureCoords[2] + s2 * float2(0.07, 0.19));

	float4 c1 = v1.a * AtmosphereCloudColor1;
	float4 c2 = v2.a * AtmosphereCloudColor2;
	return AtmosphereSkyColor + lerp(c1, c2, 0.5);//v2.a);
}

technique Atmosphere
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 AtmosphereVertexShader();
		PixelShader				= compile ps_2_0 AtmospherePixelShader();
		AlphaBlendEnable	= true;
		BlendOp						= Add;
		SrcBlend					= One;
		DestBlend					= One;
		ZEnable						= true;
		ZFunc							= Always;
		ZWriteEnable			= true;
	}
}

#endif
