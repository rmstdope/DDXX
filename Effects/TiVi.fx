#include <CommonFunctions.hlsl>
 
struct VertexShaderInput
{
	float4	Position			: POSITION;			// Vertex Position
	float3	BlendWeights	: BLENDWEIGHT;	// Blend weight
	int4		BlendIndices	: BLENDINDICES;	// Bland indices
	float3	Normal				: NORMAL;				// Vertex Normal
	float3	Tangent				: TANGENT;			// Vertex Tangent
	float2	TextureCoord	: TEXCOORD0;		// Vertex Texture Coordinate
};

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
	//output.Light = 1.0f;
	return output;
}

float4
LinePixelShader(LinePixelInput input) : COLOR0
{
	return float4(input.Light, 1);
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
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode					= None;
	}
}
