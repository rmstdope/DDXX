#include "../../Effects/Content/effects/CommonVariables.hlsl"

sampler TextureSampler = 
sampler_state
{
    Texture = <Texture>;    
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU	= CLAMP;
    AddressV	= CLAMP;
};

struct VertexShaderInput
{
	float4 Position		: POSITION0;
	float3 Color			: COLOR0;
	float  Size				: PSIZE;
};

struct VertexShaderOutput
{
	float4 Position		: POSITION0;
	float3 Color			: COLOR0;
	float  Size				: PSIZE;
#ifdef XBOX
	float4 TextureUV	: SPRITETEXCOORD;
#else
	float2 TextureUV	: TEXCOORD0;
#endif
};

struct PixelShaderInput
{
	float4 Position		: POSITION0;
	float3 Color			: COLOR0;
	float2 TextureUV	: TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	//float4 worldPosition = mul(input.Position, World);
	//float4 viewPosition = mul(worldPosition, View);
	output.Position = float4(input.Position.xyz, 1);
	output.Color = input.Color;
	output.Size = input.Size;
	output.TextureUV = float2(0,0);

	return output;
}

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
	float2 texCoord;

#ifdef XBOX
	texCoord = abs(input.TextureUV.zw);
#else
	texCoord = input.TextureUV.xy;
#endif
	return float4(input.Color * tex2D(TextureSampler, texCoord), 1);
}

technique Technique1
{
	pass Pass1
	{				
		VertexShader			= compile vs_1_1 VertexShaderFunction();
		PixelShader				= compile ps_1_1 PixelShaderFunction();
		PointSpriteEnable = true;
		ZEnable						= false;
		ZWriteEnable			= false;
		FillMode					= Solid;
	}
}
