#ifndef SHARED_VARIABLES_FX
#define SHARED_VARIABLES_FX

#include "EffectConstants.h"

/**
 * This file contains all effect variables that are shared between different effects
 */

/// Blend variables
shared DWORD BlendOperation = ADD;
shared DWORD SourceBlend = ONE;
shared DWORD DestBlend = ZERO;
shared DWORD BlendFactor = 0x80808080;
shared DWORD FillMode = FILLMODE_SOLID;

/** 
 * Color for the ambient light. Used for ambient passes only.
 */
shared float4 AmbientColor;

/** 
 * Light direction
 * Used for directional lights and spot lights
 */
shared float4 LightDirections[5];

/** 
 * Diffuse Light and Material color
 * Used for all types of lights
 */
shared float4 LightDiffuseColors[5];
shared float4 MaterialDiffuseColor;

/** 
 * Specular Light and Material color
 * Used for all types of lights
 */
shared float4 LightSpecularColors[5];
shared float4 MaterialSpecularColor;
shared float MaterialShininess;

/** 
 * Light position
 * Used for point lights and spot lights
 */
shared float4 LightPositions[5];

/** 
 * Light range
 * Used for point lights and spot lights
 */
shared float LightRanges[5];

/**
 * Light radius 
 * Used for point lights and spot lights
 */
shared float LightRadius;
/**
 * Light decay type
 * 0=None, 1=Linear, 2=Squared
 * Used for point lights and spot lights
 */
shared int LightDecayType;

/**
 * The number of lights used.
 */
shared int NumLights;

/** 
 * Position of the camera in world space
 */
shared float4 EyePosition;

/** 
 * Normalization texture
 * Texture used for normalization on pre 2.0 pixel shaders
 */
shared textureCUBE NormalizationTexture;

/**
 * Attenuation texture for x and y 
 */
shared texture AttenuationTextureXY;

/** 
 * Attenuation texture for z 
 */
shared texture1D AttenuationTextureZ;

/**
 * Animation matrices. One for each bone 
 */
shared float4x4 AnimationMatrices[40];


/** World * View * Projection matrix transposed*/
float4x4 WorldViewProjectionT;

/** World * View * Projection matrix inverse transposed */
float4x4 InvWorldViewProjectionT;

/** World * View matrix transposed*/
float4x4 WorldViewT;

/** World matrix transposed */
float4x4 WorldT;

/** View * Projection matrix transposed */
float4x4 ViewProjectionT;

/** Base texture for mesh */
texture BaseTexture;

/** Reflective texture for mesh */
textureCUBE ReflectiveTexture;

/** Normal texture for mesh */
texture NormalTexture;

/** The reflective factor of the material */
float ReflectiveFactor;

/** Texture samplers */
sampler BaseTextureSampler = sampler_state
{
    Texture = (BaseTexture);
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU	= Wrap;//Mirror;
    AddressV	= Wrap;//Mirror;
    //AddressW	= Wrap;//Mirror;
};

sampler BaseTextureSamplerMirrored = sampler_state
{
    Texture = (BaseTexture);
    MipFilter = Point;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU	= Mirror;
    AddressV	= Mirror;
};


sampler BaseTextureSamplerBordered = sampler_state
{
    Texture = (BaseTexture);
    MipFilter = Point;
    MinFilter = Point;
    MagFilter = Point;
    AddressU	= Border;
    AddressV	= Border;
    //BorderColor = 0;
};

samplerCUBE ReflectiveTextureSampler = sampler_state
{
    Texture = (ReflectiveTexture);
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
    //AddressU	= WRAP;
    //AddressV	= WRAP;
    //AddressW	= WRAP;
};
sampler NormalTextureSampler = sampler_state
{
    Texture = (NormalTexture);
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
    //AddressU	= WRAP;
    //AddressV	= WRAP;
    //AddressW	= WRAP;
};
samplerCUBE NormalizationTextureSampler = sampler_state
{
    Texture = (NormalizationTexture);
    MipFilter = Linear;
    MinFilter = Point;
    MagFilter = Point;
    AddressU	= Clamp;
    AddressV	= Clamp;
    AddressW	= Clamp;
};
sampler AttenuationTextureXYSampler = sampler_state
{
    Texture = (AttenuationTextureXY);
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU	= CLAMP;
    AddressV	= CLAMP;
    AddressW	= CLAMP;
};

sampler AttenuationTextureZSampler = sampler_state
{
    Texture = (AttenuationTextureZ);
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU	= CLAMP;
    AddressV	= CLAMP;
    AddressW	= CLAMP;
};

samplerCUBE SkyBoxTextureSampler = sampler_state
{
    Texture = (BaseTexture);
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    //AddressU	= WRAP;
    //AddressV	= WRAP;
    //AddressW	= WRAP;
};

/**
 * Time variable for animations, e.g. water surface
 */ 
shared float Time;

#endif
