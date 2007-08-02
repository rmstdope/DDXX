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

technique SimpleWithAlpha
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
		AlphaBlendEnable	= true;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= false;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode					= CCW;
		BlendOp						= Add;
		SrcBlend					= SrcColor;
		DestBlend					= InvSrcColor;
		//BlendFactor				= 0x80808080;
	}
}

technique SimpleMirroredTexture
<
	bool NormalMapping = false;
	bool Skinning = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SimpleVertexShader(0);
		PixelShader				= compile ps_2_0 SimplePixelShader(BaseTextureSamplerMirrored);
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

technique FilmRoll
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
		AlphaBlendEnable	= true;
		SrcBlend					= SrcAlpha;
		DestBlend					= InvSrcAlpha;
		BlendOp						= Add;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode					= None;
	}
}
