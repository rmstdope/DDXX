#include "CommonVariables.hlsl"

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
#ifdef XBOX
	float4 TextureUV	: SPRITETEXCOORD;
#else
	float2 TextureUV	: TEXCOORD0;
#endif
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
	float4 pos = mul(Position, World[0]);
	pos = mul(pos, View);

	// Use 600 temporary. This should probably be a function of FOV?!?
	// This is taken from DX docs but does not feel good...
	// Anyway, it should not be 600 but height of viewport (which is not window height!)
	output.Size = 720 * Size / length(pos.xyz / pos.w);
	
	// Transform particle to Projection space
	output.Position = mul(pos, Projection);

	// Strange, this seems to be needed...
	output.TextureUV = float2(0,0);

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
SimplePixelShader(const PixelInputStream input) : COLOR0
{
	float2 texCoord;

#ifdef XBOX
	texCoord = abs(input.TextureUV.zw);
#else
	texCoord = input.TextureUV.xy;
#endif

	return (float4(AmbientColor, 1) + input.Color) * tex2D(TextureSampler, texCoord);
}

technique PointSprite
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 PointSizeVertexShader();
		PixelShader				= compile ps_2_0 SimplePixelShader();
		CullMode					= None;
		PointSpriteEnable = true;
		ZEnable						= true;
		ZWriteEnable			= false;
		FillMode					= Solid;
  }
}
