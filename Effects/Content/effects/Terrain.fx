#include "CommonVariables.hlsl"

sampler2D DiffuseTextureSampler = sampler_state
{
	Texture = <Texture>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
};

struct VertexShaderInput
{
	float4 position : POSITION0;
	float2 texCoord : TEXCOORD0;
	float3 normal : NORMAL0;    
};

struct VertexShaderOutput
{
	float4 position : POSITION0;
	float2 texCoord : TEXCOORD0;
	float3 color : COLOR0;
};

VertexShaderOutput
MyVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output;
    
	// transform the position into projection space
	float4 worldSpacePos = mul(input.position, World[0]);
	output.position = mul(worldSpacePos, View);
	output.position = mul(output.position, Projection);
    
	// calculate the light direction ( from the surface to the light ), which is not
	// normalized and is in world space
	float3 lightDirection = normalize(LightPositions[0] - worldSpacePos);
        
	// similarly, calculate the view direction, from the eye to the surface.  not
	// normalized, in world space.
	float3 eyePosition = mul(-View._m30_m31_m32, transpose(View));
	float3 viewDirection = normalize(worldSpacePos - eyePosition);

	float nDotL = max(dot(lightDirection, viewDirection), 0);
	output.color = LightDiffuseColors[0] * nDotL * DiffuseColor;
    
	// pass the texture coordinate through without additional processing
	output.texCoord = input.texCoord;
    
	return output;
}

float4
MyPixelShader(VertexShaderOutput input) : COLOR0
{
	float3 diffuseTexture = tex2D(DiffuseTextureSampler, input.texCoord);

	// return the combined result.
	float3 color = (input.color + AmbientLightColor * AmbientColor) * diffuseTexture;
	return float4(color, 1);
}

Technique NormalMapping
{
	Pass Go
	{
		VertexShader = compile VS_SHADERMODEL MyVertexShader();
		PixelShader = compile PS_SHADERMODEL MyPixelShader();
	}
}