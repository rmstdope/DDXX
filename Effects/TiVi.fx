#include <CommonFunctions.hlsl>
 
struct VertexShaderInput
{
	float4	Position			: POSITION;			// Vertex Position
	float3	BlendWeights	: BLENDWEIGHT;	// Blend weight
	int4		BlendIndices	: BLENDINDICES;	// Bland indices
	float3	Normal				: NORMAL;				// Vertex Normal
	float3	Tangent				: TANGENT;			// Vertex Tangent
	float2	TexCoords			: TEXCOORD0;		// Vertex Texture Coordinate
};

struct SolidPixelInput
{
	float4	Position			:	POSITION;
	float2	TexCoords			:	TEXCOORD0;
	float3	Diffuse				:	TEXCOORD1;
	float3	Specular			:	TEXCOORD2;
};

SolidPixelInput
SolidVertexShader(VertexShaderInput input,
									uniform int numWeights)
{
	SolidPixelInput output;

	// Transform the position from object space to homogeneous projection space
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);
	output.Position = mul(animated.Position, WorldViewProjectionT);
	float3 normal = normalize(mul(animated.Normal, WorldT));
	float vDotN = saturate(dot(normal, float3(0, 0, -1)));
	output.Diffuse = AmbientColor;
	output.Diffuse += vDotN * MaterialDiffuseColor;
	output.Specular = pow(vDotN, MaterialShininess) * MaterialSpecularColor;
	output.TexCoords = input.TexCoords;
	return output;
}

float4
SolidPixelShader(SolidPixelInput input,
								 uniform sampler textureSampler) : COLOR0
{
	return float4(input.Diffuse, 1) * tex2D(textureSampler, input.TexCoords) + float4(input.Specular, 1);
}

struct LinePixelInput
{
	float4	Position			:	POSITION;
	float3	Light					:	TEXCOORD1;
};

LinePixelInput
LineVertexShader(VertexShaderInput input,
								 uniform int numWeights)
{
	LinePixelInput output;

	// Transform the position from object space to homogeneous projection space
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);
	output.Position = mul(animated.Position, WorldViewProjectionT);
	float3 normal = normalize(mul(animated.Normal, WorldT));
	float vDotN = saturate(dot(normal, float3(0, 0, -1)));
	output.Light = AmbientColor;
	output.Light += vDotN * MaterialDiffuseColor;
	output.Light += pow(vDotN, MaterialShininess) * MaterialSpecularColor;
	return output;
}

float4
LinePixelShader(LinePixelInput input) : COLOR0
{
	return float4(input.Light, 1);
}

technique SolidSkinning
<
	bool NormalMapping = false;
	bool Skinning = true;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SolidVertexShader(2);
		PixelShader				= compile ps_2_0 SolidPixelShader(BaseTextureSampler);
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode					= CCW;
	}
}

technique TvScreenSkinning
<
	bool NormalMapping = false;
	bool Skinning = true;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SolidVertexShader(2);
		PixelShader				= compile ps_2_0 SolidPixelShader(BaseTextureSamplerBordered);
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode					= CCW;
	}
}

technique LineDrawerSkinning
<
	bool NormalMapping = false;
	bool Skinning = true;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 LineVertexShader(2);
		PixelShader				= compile ps_2_0 LinePixelShader();
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Wireframe;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode					= None;
	}
}