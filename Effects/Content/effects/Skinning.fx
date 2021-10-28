#include "CommonVariables.hlsl"

sampler2D SkinningEffectSampler = sampler_state
{
    Texture = <Texture>;
    MinFilter = linear;
    MagFilter = linear;
    MipFilter = linear;
};

struct VertexInput
{
	float4 Position			: POSITION0;
	float3 Normal				: NORMAL0;
	float2 TexCoord			: TEXCOORD0;
	float4 BoneIndices	: BLENDINDICES0;
	float4 BoneWeights	: BLENDWEIGHT0;
};

struct PixelInput
{
	float4 Position	: POSITION0;
	//float3 Lighting	: COLOR0;
	float2 TexCoord	: TEXCOORD0;
};

PixelInput
SkinningVertexShader(VertexInput input)
{
	PixelInput output;

	float4x4 skinTransform;
	skinTransform  = World[input.BoneIndices.x] * input.BoneWeights.x;
	skinTransform += World[input.BoneIndices.y] * input.BoneWeights.y;
	skinTransform += World[input.BoneIndices.z] * input.BoneWeights.z;
	skinTransform += World[input.BoneIndices.w] * input.BoneWeights.w;
	
	// transform the position into projection space
	output.Position = mul(input.Position, skinTransform);
	output.Position = mul(output.Position, View);
	output.Position = mul(output.Position, Projection);
	
	output.TexCoord = input.TexCoord;
	
	return output;
}

float4
SkinningPixelShader(PixelInput input) : COLOR
{
    return tex2D(SkinningEffectSampler, input.TexCoord);
}

Technique Skinning
{
	Pass BasePass
	{
		VertexShader = compile VS_SHADERMODEL SkinningVertexShader();
		PixelShader = compile PS_SHADERMODEL SkinningPixelShader();
		ZEnable = true;
		ZWriteEnable = true;
	}
}