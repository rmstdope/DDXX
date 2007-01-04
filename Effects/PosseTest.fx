#include <CommonFunctions.hlsl>

shared float ZMAX =50;
shared float ZMIN =-50;

int HeightMapMaxSize = 200;
float HeightMap[200];

texture PosseSourceTexture;
 
sampler2D PosseTextureSampler = sampler_state
{
	texture = <PosseSourceTexture>;
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = None;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct InputVS
{
	float4	Position			: POSITION;			// Vertex Position
	float3	Normal				: NORMAL;				// Vertex Normal
//	float3	Tangent				: TANGENT;			// Vertex Tangent
};

struct InputPS
{
	float4 Position			:	POSITION;			// Vertex position 
	float Height			:   COLOR0;
};

struct TexturedInputVS
{
	float4	Position			: POSITION;			// Vertex Position
	float3	Normal				: NORMAL;				// Vertex Normal
 	float2	TextureCoord	: TEXCOORD0;		// Vertex Texture Coordinate
};

struct TexturedInputPS
{
	float4 Position			:	POSITION;			// Vertex position 
	float2 TextureCoord		:	TEXCOORD0;		// Vertex texture coords 
};


InputPS
MyVertexShader(InputVS input)
{
	InputPS output;
		
	// Transform the position from object space to homogeneous projection space
	
	output.Position = mul(input.Position, WorldViewProjectionT);

	float4 heightpos = output.Position; //mul(input.Position, WorldViewT);
	
	//if (output.Position.z < ZMIN)
	//	ZMIN = output.Position.z;
	//if (output.Position.z > ZMAX)
	//	ZMAX = output.Position.z;
	
	output.Height = (heightpos.z + ZMIN) / (ZMAX-ZMIN);
	
	//output.Height = output.Position.y;
	
	return output;    
}


float4
MyPixelShader(InputPS input) : COLOR0
{
	//return AmbientColor;
	return float4(input.Height, 0, 0, 1);
}


TexturedInputPS
TexturedVertexShader(TexturedInputVS input)
{
	TexturedInputPS output;
	float4 pos = input.Position;	
	
	//int mapIndex = pos.z;
	//pos.z = HeightMap[mapIndex];
	//float4 z4 = tex2Dlod(PosseTextureSampler, float4(input.TextureCoord.x, input.TextureCoord.y, 0, 0));
	//pos.z = z4.x*(ZMAX-ZMIN)-ZMIN;
	
	// Transform the position from object space to homogeneous projection space
	output.Position = mul(pos, WorldViewProjectionT);
	
	output.TextureCoord = input.TextureCoord;
	
	return output;    
}


float4
TexturedPixelShader(TexturedInputPS input) : COLOR0
{
	//return AmbientColor * tex2D(PosseTextureSampler, input.TextureCoord.xy);
	return AmbientColor * tex2D(BaseTextureSampler, input.TextureCoord.xy);
}


technique TexPosseTestTechnique
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 TexturedVertexShader();
		PixelShader				= compile ps_2_0 TexturedPixelShader();
		AlphaBlendEnable	= false;
		CullMode					= CCW;
		FillMode					= Solid;//<FillMode>;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false; //true;
	}
}

technique NoTexPosseTestTechnique
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 MyVertexShader();
		PixelShader				= compile ps_2_0 MyPixelShader();
		AlphaBlendEnable	= false;
		CullMode					= CCW;
		FillMode					= Solid;//<FillMode>;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false; //true;
	}
}

