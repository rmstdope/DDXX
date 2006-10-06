//-----------------------------------------------------------------------------
// Global variables
//-----------------------------------------------------------------------------
/** Projection matrix transposed */
float4x4 ProjectionT;

/** World * View matrix transposed */
float4x4 WorldViewT;

/** World * View * Projection matrix transposed */
float4x4 WorldViewProjectionT;

/** Particle texture */
texture Texture;

//-----------------------------------------------------------------------------
// Texture samplers
//-----------------------------------------------------------------------------
sampler TextureSampler = 
sampler_state
{
    Texture = <Texture>;    
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
PixelShader(const PixelInputStream input) : COLOR0
{ 
	// Lookup texture
	return input.Color * tex2D(TextureSampler, input.TextureUV);
}

technique PointSprite
{
	pass P0
	{
		VertexShader			= compile vs_1_1 PointSizeVertexShader();
		PixelShader				= compile ps_1_1 PixelShader();
		CullMode					= None;
		PointSpriteEnable = true;
		ZFunc							= Less;
		ZWriteEnable			= false;
		AlphaBlendEnable	= true;
		SrcBlend					= SrcAlpha;
		DestBlend					= One;
  }
}

technique PreTransformed
{
	pass P0
	{
		VertexShader			= compile vs_1_1 PreTransformedVertexShader();
		PixelShader				= compile ps_1_1 PixelShader();
		CullMode					= None;
		ZFunc							= Less;
		ZWriteEnable			= false;
		AlphaBlendEnable	= true;
		SrcBlend					= SrcAlpha;
		DestBlend					= One;
  }
}

technique Generic
{
	pass P0
	{
		VertexShader			= compile vs_1_1 GenericVertexShader();
		PixelShader				= compile ps_1_1 PixelShader();
		CullMode					= None;
		ZFunc							= Less;
		ZWriteEnable			= false;
		AlphaBlendEnable	= true;
		SrcBlend					= SrcAlpha;
		DestBlend					= One;
  }
}
