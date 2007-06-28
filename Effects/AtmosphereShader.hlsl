#ifndef ATMOSPHERE_SHADER_FX
#define ATMOSPHERE_SHADER_FX

/**
 *
 * Atmosphere shader
 *
*/

#include "SharedVariables.hlsl"

/***********************************************************************************
 * ------------------
 * Variables
 * ------------------
***********************************************************************************/

float4 AtmosphereSunColor;
float4 AtmosphereSkyColor = float4(0.0, 0.0, 0.0, 0);
float4 AtmosphereCloudColor1 = float4(0.5, 0.5, 0.5, 0);
float4 AtmosphereCloudColor2 = float4(0.5, 0.5, 0.5, 0);
float2 AtmosphereSunPosition;

float2 AtmosphereTime = float2(1, 1);

//texture AtmosphereTexture1;
//texture AtmosphereTexture2;
//texture AtmosphereTexture3;

struct AtmosphereInputVS
{
	float4 Position				: POSITION0;
	float2 TextureCoords	: TEXCOORD0;
};

struct AtmosphereInputPS
{
	float4 Position					: POSITION0;
	float2 TextureCoords[5]	: TEXCOORD0;
};

AtmosphereInputPS
AtmosphereVertexShader(AtmosphereInputVS Input)
{
	AtmosphereInputPS Output;
	
	// Transform coordinates
	Output.Position = mul(Input.Position, WorldViewProjectionT);

	Output.TextureCoords[0] = Input.TextureCoords + AtmosphereTime.x;
	Output.TextureCoords[1] = Input.TextureCoords - AtmosphereTime.x;
	Output.TextureCoords[2] = Input.TextureCoords + AtmosphereTime.y;
	Output.TextureCoords[3] = Input.TextureCoords - AtmosphereTime.y;
	Output.TextureCoords[4] = Input.TextureCoords;// / 4;
	Output.TextureCoords[4].y *= 0.5f;
	Output.TextureCoords[4] += AtmosphereSunPosition;

	return Output;
}

technique Atmosphere
{
	pass BasePass
	{
		Texture[0]				= <BaseTexture>;
		Texture[1]				= <BaseTexture>;
		Texture[2]				= <BaseTexture>;
		AddressU[0]				= Mirror;
		AddressV[0]				= Mirror;
		AddressU[1]				= Mirror;
		AddressV[1]				= Mirror;
		AddressU[2]				= Mirror;
		AddressV[2]				= Mirror;
		AddressV[1]				= Clamp;
		AddressU[2]				= Clamp;
		AddressV[2]				= Clamp;
		MagFilter[0]			=	Linear;
		MinFilter[0]			= Linear;
		MipFilter[0]			=	Linear;
		MagFilter[1]			=	Linear;
		MinFilter[1]			= Linear;
		MipFilter[1]			=	Linear;
		MagFilter[2]			=	Linear;
		MinFilter[2]			= Linear;
		MipFilter[2]			=	Linear;
		PixelShaderConstant4[2] = <AtmosphereCloudColor1>;
		PixelShaderConstant4[4] = <AtmosphereCloudColor2>;
		PixelShaderConstant4[5] = <AtmosphereSkyColor>;
		PixelShaderConstant4[6] = <AtmosphereSunColor>;
		VertexShader			= compile vs_1_1 AtmosphereVertexShader();
		PixelShader					= 
		asm {
			ps_1_4
			def c0, 0.0, 0.5, 1.0, 0.75
			def c1, 0.2, 0.34, 0, 0
			//def c2, 0.5, 0.7, 1, 1
			def c3, 0.39, 0.51, 0, 0
			//def c4, 0.5, 0.7, 1, 1
			//def c5, 0.3, 0.3, 0.6, 1 // Atmosphere color
				texld r0, t1
				texld r1, t3
        texcrd r2.rgb, t0
        texcrd r3.rgb, t2
        mad r0.rgb, c1, r0_bx2, r2
        mad r1.rgb, c3, r1_bx2, r3
        
				phase
        
				texld r0, r0
				texld r1, r1
				texld r2, t4
				mul_sat r4.rgb, r0.a, c2
				+mul r4.a, r0.a, c2.a
				mul_sat r3.rgb, r1.a, c4
				+mul r3.a, r1.a, c4.a
        lrp_sat r0, r1.a, r3, r4
        add r0, c5, r0
        mad r0, r2, c6, r0
        //mov r0.rgba, c2.r
				//mov r0, r4
    };
		AlphaBlendEnable	= true;
		BlendOp						= Add;
		SrcBlend					= One;
		DestBlend					= One;
		ZEnable						= true;
		ZFunc							= Always;
		ZWriteEnable			= true;
	}
}

/*technique AtmosphereAmbient
{
	pass BasePass
	{
		Texture[0]				= <AtmosphereTexture1>;
		Texture[1]				= <AtmosphereTexture2>;
		//Texture[2]				= <AtmosphereTexture1>;
		//Texture[3]				= <AtmosphereTexture2>;
		AddressU[0]				= Mirror;
		AddressV[0]				= Mirror;
		AddressU[1]				= Mirror;
		AddressV[1]				= Mirror;
		AddressU[2]				= Mirror;
		AddressV[2]				= Mirror;
		AddressU[3]				= Mirror;
		AddressV[3]				= Mirror;
		VertexShader			= null;
		//PixelShader				= compile ps_1_4 AtmospherePixelShader();
		PixelShader					= 
		asm {
			ps_1_4
			def c0, 0.0, 0.5, 1.0, 0.75
			def c1, 0.2, 0.34, 0, 0
			def c2, 1, 0.7, 0.5, 1
			def c3, 0.39, 0.51, 0, 0
			def c4, 1, 0.7, 0.5, 1
				texld r0, t1
				texld r1, t3
        texcrd r2.rgb, t0
        texcrd r3.rgb, t2
        mad r0.rgb, c1, r0_bx2, r2
        mad r1.rgb, c3, r1_bx2, r3
        
				phase
        
				texld r0, r0
				texld r1, r1
				mul_x2_sat r4.rgb, r0.a, c2
				+mul r4.a, r0.a, c2.a
				mul_x2_sat r3.rgb, r1.a, c4
				+mul r3.a, r1.a, c4.a
        lrp_sat r0, r1.a, r3, r4
				//mov r0, r4
    };
		AlphaBlendEnable	= false;
		//BlendOp						= Add;
		//SrcBlend					= One;
		//DestBlend					= One;
		ZEnable						= false;
	}
}*/

#endif
