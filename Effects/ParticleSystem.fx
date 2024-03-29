#include "SharedVariables.hlsl"

//-----------------------------------------------------------------------------
// Global variables
//-----------------------------------------------------------------------------
/** Projection matrix transposed */
float4x4 ProjectionT;

//-----------------------------------------------------------------------------
// Texture samplers
//-----------------------------------------------------------------------------
sampler TextureSampler = 
sampler_state
{
    Texture = <BaseTexture>;    
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU	= CLAMP;
    AddressV	= CLAMP;
};

//-----------------------------------------------------------------------------
// Vertex shader output structure
//-----------------------------------------------------------------------------
struct VertexOutputStream
{
	// Position
	float4	Position	:	POSITION;
	// Texture coords
	float Size				: PSIZE;
	// Texture coords
	float2 TextureUV	: TEXCOORD0;
	// Sprite color and alpha
	float4 Color			:	COLOR0;
};

//-----------------------------------------------------------------------------
// Pixel shader input structure
//-----------------------------------------------------------------------------
struct PixelInputStream
{
	// Position
	float4	Position	:	POSITION;
	// Texture coords
	float2 TextureUV	: TEXCOORD0;
	// Sprite color and alpha
	float4 Color			:	COLOR0;
};

/**
 * Vertex shader for particle systems with point sizes.
 * @param Position The 3D position of the particle.
 * @param Color The color of the particle
 * @param Size The size of the particle.
 * @return The Output stream.
 */
VertexOutputStream
PointSizeVertexShader(float4 Position	:	POSITION,
											float4 Color			:	COLOR0,
											float Size				:	PSIZE)
{
	VertexOutputStream output;

	// Transform particle to view space
	float4 pos = mul(Position, WorldViewT);

	// Use 600 temporary. This should probably be a function of FOV?!?
	// This is taken from DX docs but does not feel good...
	// Anyway, it should not be 600 but height of viewport (which is not window height!)
	output.Size = 600 * Size / length(pos.xyz / pos.w);
	
	// Transform particle to Projection space
	output.Position = mul(pos, ProjectionT);

	// Strange, this seems to be needed...
	output.TextureUV = float2(0,0);

	// Copy color
	output.Color = Color;
	
	return output;
}

VertexOutputStream
GlitterVertexShader(float4 Position		:	POSITION,
										float Size				:	PSIZE,
										float3 Normal			: NORMAL,
										float4 Color			:	COLOR0)
{
	VertexOutputStream output;

	// Transform particle to view space
	float4 pos = mul(Position, WorldViewT);

	// Use 600 temporary. This should probably be a function of FOV?!?
	// This is taken from DX docs but does not feel good...
	// Anyway, it should not be 600 but height of viewport (which is not window height!)
	output.Size = 600 * Size / length(pos.xyz / pos.w);
	
	// Transform particle to Projection space
	output.Position = mul(pos, ProjectionT);

	// Strange, this seems to be needed...
	output.TextureUV = float2(0,0);

	// Copy color
	float3 normal = mul(Normal, (float4x3)WorldViewT);
	float d = max(0, dot(normal, float3(0, 0, -1)));
	float p = pow(d, 8);
	output.Color = Color * p;
	
	return output;
}

/**
 * Vertex shader for particle systems with no point sizes and pre transformated vertices.
 * @param Position The transformated 3D position of the particle.
 * @param Color The color of the particle
 * @param TextureUV The texture UV coordinates of the particle.
 * @return The Output stream.
 */
PixelInputStream
PreTransformedVertexShader(float4 Position	:	POSITION,
													 float4 Color		:	COLOR0,
													 float2 TextureUV	: TEXCOORD0)
{
	PixelInputStream output;

	// Transform particle to Projection space
	output.Position = mul(Position, ProjectionT);

	// Copy texture coordinates
	output.TextureUV = TextureUV;

	// Copy color
	output.Color = Color;
	
	return output;
}

/**
 * Vertex shader for particle systems with no point sizes.
 * @param Position The transformated 3D position of the particle.
 * @param Color The color of the particle
 * @param TextureUV The texture UV coordinates of the particle.
 * @return The Output stream.
 */
PixelInputStream
GenericVertexShader(float4 Position		:	POSITION,
										float4 Color			:	COLOR0,
										float2 TextureUV	: TEXCOORD0)
{
	PixelInputStream output;

	// Transform particle to Projection space
	output.Position = mul(Position, WorldViewProjectionT);

	// Copy texture coordinates
	output.TextureUV = TextureUV;

	// Copy color
	output.Color = Color;
	
	return output;
}

/**

 * Pixel shader for particle system.
 * @param input The input stream for the shader.
 * @return the Output stream.
 */
float4
SimplePixelShader(const PixelInputStream input, uniform bool useTexture) : COLOR0
{ 
	// Lookup texture
	if (useTexture)
		return (AmbientColor + input.Color) * tex2D(TextureSampler, input.TextureUV);
	else
		return (AmbientColor + input.Color);
}

technique PointSpriteNoTexture
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 PointSizeVertexShader();
		PixelShader				= compile ps_2_0 SimplePixelShader(false);
		CullMode					= None;
		PointSpriteEnable = true;
		ZFunc							= Less;
		ZWriteEnable			= false;
  }
}

technique PointSprite
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 PointSizeVertexShader();
		PixelShader				= compile ps_2_0 SimplePixelShader(true);
		CullMode					= None;
		PointSpriteEnable = true;
		ZFunc							= Less;
		ZWriteEnable			= false;
  }
}

technique Glitter
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 GlitterVertexShader();
		PixelShader				= compile ps_2_0 SimplePixelShader(true);
		CullMode					= None;
		PointSpriteEnable = true;
		ZFunc							= Less;
		ZWriteEnable			= false;
  }
}

technique XXXPreTransformed
{
	pass P0
	{
		VertexShader			= compile vs_2_0 PreTransformedVertexShader();
		PixelShader				= compile ps_2_0 SimplePixelShader(true);
		CullMode					= None;
		ZFunc							= Less;
		ZWriteEnable			= false;
		AlphaBlendEnable	= true;
		SrcBlend					= SrcAlpha;
		DestBlend					= One;
  }
}

technique XXXGeneric
{
	pass P0
	{
		VertexShader			= compile vs_2_0 GenericVertexShader();
		PixelShader				= compile ps_2_0 SimplePixelShader(true);
		CullMode					= None;
		ZFunc							= Less;
		ZWriteEnable			= false;
		AlphaBlendEnable	= true;
		SrcBlend					= SrcAlpha;
		DestBlend					= One;
  }
}
