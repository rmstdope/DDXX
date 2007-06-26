#ifndef DATA_TYPES_FX
#define DATA_TYPES_FX

#include <EffectConstants.h>

/**
 * Internal structure used to calculate light components
 */
struct LightComponents
{
	float Diffuse;											// Diffuse light component
	float Specular;											// Specular light component
};

/**
 * Internal structure used to calculate position, normal and tangent
 * from animation matrices
 */
 struct AnimatedVertex_PNT
 {
	 float4	Position;
	 float3	Normal;
	 float3 Tangent;
 };

/**
 * Internal structure used to calculate position and normal
 * from animation matrices
 */
 struct AnimatedVertex_PN
 {
	 float4	Position;
	 float3	Normal;
 };

/**
 * Internal structure used to calculate V and H in the Blinn-Phong equation
 * L is light vector and H is half vector
 */
struct LightVectors
{
	/** Light vector, normalized */
	float3 L;
	/** Half vector, normalized */
	float3 H;
};

/**
 * Vertex shader output stream and pixel shader input stream for 1.1 shaders.
 */
struct VertexOutputStream_1_1_Pass2
{
	float4 Position			:	POSITION;			// Vertex position 
	float2 TextureCoord	:	TEXCOORD0;		// Vertex texture coords 
	float2 NormalCoord	:	TEXCOORD1;		// Vertex texture coords 
	float3 LightVector	:	TEXCOORD2;		// Light vector in tangent space
	float3 HalfVector		:	TEXCOORD3;		// Half vector in tangent space
};

/**
 * Vertex shader output stream and pixel shader input stream for 1.1 shaders.
 */
struct VertexOutputStream_1_1_Pass1
{
	float4 Position			:	POSITION;			// Vertex position 
	float2 AttCoord1		:	TEXCOORD0;		// X and Y coordinates for attenuaton texture
	float2 AttCoord2		:	TEXCOORD1;		// Z coordinate for attenuation texture
};

/**
 * Vertex shader output stream and pixel shader input stream for 1.4 shaders.
 */
struct VertexOutputStream_1_4
{
	float4 Position			:	POSITION;			// Vertex position 
	float2 TextureCoord	:	TEXCOORD0;		// Vertex texture coords 
	float3 LightVector	:	TEXCOORD1;		// Light vector in tangent space
	float3 HalfVector		:	TEXCOORD2;		// Half vector in tangent space
	float2 AttCoord1		:	TEXCOORD3;		// X and Y coordinates for attenuaton texture
	float2 AttCoord2		:	TEXCOORD4;		// Z coordinate for attenuation texture
};

/** Ambient vertex shader output structure*/
//struct AmbientVertexOutputStream
//{
//	float4 Position			:	POSITION;				// Vertex position 
//	float2 TextureUV		:	TEXCOORD0;			// Vertex texture coords 
//	float2 ReflectionUV	:	TEXCOORD1;			// Vertex reflection texture coords 
//};

/** Halo vertex shader output structure*/
struct HaloVertexOutputStream
{
	float4 Position		:	POSITION;				// Vertex position 
	float4 Color			:	COLOR0;					// Vertex color
};

/** SkyBox vertex shader output structure*/
struct SkyBoxVertexOutputStream
{
	float4 Position			:	POSITION;				// Vertex position 
	float3 TextureUVW		:	TEXCOORD0;			// Vertex texture coords 
};


/** 
 * Shadow vertex shader output structure
 */
struct ShadowVertexOutputStream_1_1
{
	float4 Position   : POSITION;   // vertex position 
};

/// Common struct for ordinary vertex shaders
struct VertexShaderInput
{
	float4	Position			: POSITION;			// Vertex Position
	float3	BlendWeights	: BLENDWEIGHT;	// Blend weight
	int4		BlendIndices	: BLENDINDICES;	// Bland indices
	float3	Normal				: NORMAL;				// Vertex Normal
	float3	Tangent				: TANGENT;			// Vertex Tangent
	float2	TexCoords			: TEXCOORD0;		// Vertex Texture Coordinate
};


#endif
