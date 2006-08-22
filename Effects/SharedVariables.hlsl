#ifndef SHARED_VARIABLES_FX
#define SHARED_VARIABLES_FX

#include <EffectConstants.h>

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
shared float3 LightDirection;

/** 
 * Diffuse Light and Material color
 * Used for all types of lights
 */
shared float4 DiffuseColor;

/** 
 * Specular Light and Material color
 * Used for all types of lights
 */
shared float4 SpecularColor;
shared float Shininess;

/** 
 * Light position
 * Used for point lights and spot lights
 */
shared float3 LightPosition;

/** 
 * Light range
 * Used for point lights and spot lights
 */
shared float LightRange;

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
shared float4x4 AnimationMatrix[60];


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

/** Reflection texture for mesh */
texture ReflectionTexture;

/** Normal texture for mesh */
texture NormalTexture;

/** The reflective factor of the material */
float ReflectionFactor;

/** 1 - the reflective factor of the material */
float InvReflectionFactor;

/** Texture samplers */
sampler BaseTextureSampler = sampler_state
{
    Texture = (BaseTexture);
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU	= Mirror;
    AddressV	= Mirror;
    AddressW	= Mirror;
};
sampler ReflectionTextureSampler = sampler_state
{
    Texture = (ReflectionTexture);
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    //AddressU	= WRAP;
    //AddressV	= WRAP;
    //AddressW	= WRAP;
};
sampler NormalTextureSampler = sampler_state
{
    Texture = (NormalTexture);
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    //AddressU	= WRAP;
    //AddressV	= WRAP;
    //AddressW	= WRAP;
};
samplerCUBE NormalizationTextureSampler = sampler_state
{
    Texture = (NormalizationTexture);
    MipFilter = LINEAR;
    MinFilter = POINT;
    MagFilter = POINT;
    AddressU	= CLAMP;
    AddressV	= CLAMP;
    AddressW	= CLAMP;
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