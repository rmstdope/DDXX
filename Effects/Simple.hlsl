#include <DataTypes.hlsl>

struct SimplePixelInput
{
	float4	Position			:	POSITION;
	float2	TexCoords			:	TEXCOORD0;
};

SimplePixelInput
SimpleVertexShader(VertexShaderInput input,
									uniform int numWeights)
{
	SimplePixelInput output;

	// Transform the position from object space to homogeneous projection space
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);

	// Transform to screen space
	output.Position = mul(animated.Position, WorldViewProjectionT);

	// Copy texture coordinates
	output.TexCoords = input.TexCoords;

	return output;
}

float4
SimplePixelShader(SimplePixelInput input,
								 uniform sampler textureSampler) : COLOR0
{
	return tex2D(textureSampler, input.TexCoords) * AmbientColor;
}

technique Simple
<
	bool NormalMapping = false;
	bool Skinning = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SimpleVertexShader(0);
		PixelShader				= compile ps_2_0 SimplePixelShader(BaseTextureSampler);
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