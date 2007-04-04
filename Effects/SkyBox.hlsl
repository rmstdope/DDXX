#include "SharedVariables.hlsl"

struct SkyBoxVertexInput
{
	float4 Position : POSITION0;
};

struct SkyBoxPixelInput
{
	float4 Position				: POSITION;
	float3 TextureCoords	: TEXCOORD0;
};

SkyBoxPixelInput
SkyboxVertexShader(SkyBoxVertexInput Input)
{
	SkyBoxPixelInput Output;

	Output.Position = Input.Position;
	Output.TextureCoords = normalize(mul(Input.Position, InvWorldViewProjectionT));

	return Output;
}

float4
SkyboxPixelShader(SkyBoxPixelInput Input) : COLOR
{
	float4 color = texCUBE(ReflectiveTextureSampler, Input.TextureCoords);
	return color;
}

technique SkyBox
{
    pass p0
    {
        VertexShader = compile vs_2_0 SkyboxVertexShader();
        PixelShader = compile ps_2_0 SkyboxPixelShader();
				CullMode = None;
				ZEnable = true;
				ZwriteEnable = false;
    }
}
