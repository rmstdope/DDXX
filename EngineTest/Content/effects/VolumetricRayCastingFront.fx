#include "../../../Effects/Content/effects/CommonVariables.hlsl"

sampler2D DiffuseTextureSampler = sampler_state
{
	Texture = <Texture>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

struct VertexShaderInput
{
    float4 Position		: POSITION0;
};

struct VertexShaderOutput
{
    float4 Position				: POSITION0;
	float3 CubePosition			: TEXCOORD0;
	float2 BackSideCoordinates	: TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World[0]);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	float2 coords = (output.Position.xy / output.Position.w + 1) / 2;
	coords = float2(coords.x, 1-coords.y);
    output.BackSideCoordinates = coords;
    output.CubePosition = input.Position.xyz;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	const int NumSteps = 9;
	const float StepSize = 1.0 / NumSteps;
	float3 back = tex2D(DiffuseTextureSampler, input.BackSideCoordinates);//coords);
	float3 front = input.CubePosition;
	float3 dir = back - front;
	float3 step = dir / NumSteps;
	float3 pos = front;
	float maxValue = 0;
	for (int i = 0; i < NumSteps; i++)
	{
		maxValue += (0.4f - length(pos)) * StepSize;
		pos += step;
	}
	return float4(maxValue, maxValue, maxValue, maxValue);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}
