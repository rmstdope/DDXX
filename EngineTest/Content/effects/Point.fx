#include "../../../Effects/Content/effects/CommonVariables.hlsl"

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
	float3 Color		: COLOR0;
	float  Size			: PSIZE;
	float2 TextureUV	: TEXCOORD0;
};

struct PixelShaderInput
{
	float4 Position		: POSITION0;
	float3 Color		: COLOR0;
#ifdef XBOX
	float2 TextureUV	: SPRITETEXCOORD;
#else
	float2 TextureUV	: TEXCOORD0;
#endif
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

	texCoord = input.TextureUV;

	return float4(input.Color.rgb * tex2D(TextureSampler, texCoord).rgb, 1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
		//PointSpriteEnable = true;
		ZEnable						= false;
		ZWriteEnable			= false;
		FillMode					= Solid;
	}
}
