#include "CommonVariables.hlsl"
#include "DepthOfField.hlsl"

sampler2D DefaultEffectSampler = sampler_state
{
    Texture = <Texture>;
    MinFilter = linear;
    MagFilter = linear;
    MipFilter = linear;
};

struct VertexInput
{
	float4 position : POSITION0;
	float3 normal : NORMAL0;
	float2 texCoord : TEXCOORD0;
};

struct PixelInput
{
	float4 position : POSITION0;
	float2 texCoord : TEXCOORD0;
	float4 diffuseColor : COLOR0;
	float diffuseFactor : COLOR1;
};

PixelInput
DefaultVertexShader(VertexInput input)
{
	PixelInput output;
	
	// transform the position into projection space
	float4 worldSpacePos = mul(input.position, World[0]);
	output.position = mul(worldSpacePos, View);
	output.position = mul(output.position, Projection);

	// N dot V
	float3 normal = mul(input.normal, (float3x3)World[0]);
	//normal = mul(normal, (float3x3)View);
	float3 eyePosition = mul(-View._m30_m31_m32, transpose(View));
	float3 eyeVector = normalize(worldSpacePos.xyz - eyePosition);

	float nDotV = abs(dot(normal, eyeVector));
	// Reverse scale
	//nDotV = min(1, nDotV * 2.0f);
	float diffuse = (1 - nDotV);
	output.diffuseColor = float4((diffuse * DiffuseColor + AmbientLightColor * AmbientColor), 1);
	output.diffuseFactor = diffuse;
	
	output.texCoord = input.texCoord;
	
	return output;
}

float4
DefaultPixelShader(PixelInput input) : COLOR
{
    float specular = pow(input.diffuseFactor, SpecularPower);
    return tex2D(DefaultEffectSampler, input.texCoord) * input.diffuseColor + float4(specular * SpecularColor * Shininess, 1);
}

Technique Default
{
	Pass BasePass
	{
		VertexShader			= compile VS_SHADERMODEL DefaultVertexShader();
		PixelShader				= compile PS_SHADERMODEL DefaultPixelShader();
		ZEnable						= true;
		ZWriteEnable			= true;
		//AlphaBlendEnable	= false;
		//FillMode					= Solid;
		ZFunc							= Less;
		ColorWriteEnable = true;
	}
	
	pass DofPass
	{
		VertexShader			= compile VS_SHADERMODEL PureDoFVertexShader();
		PixelShader				= compile PS_SHADERMODEL PureDoFPixelShader();
		//AlphaTestEnable		= false;
		//AlphaBlendEnable	= false;
		//FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= false;
		ZFunc							= Equal;
		//StencilEnable			= false;
		ColorWriteEnable	= alpha;
	}
}