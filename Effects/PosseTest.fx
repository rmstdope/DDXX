#include <CommonFunctions.hlsl>

float YMAX = 10;
float YMIN = -10;

texture PosseSourceTexture;
 
sampler2D PosseTextureSampler = sampler_state
{
	texture = <PosseSourceTexture>;
	MinFilter = Point;
	MagFilter = Linear;
	MipFilter = Linear;
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
VertexShader(InputVS input)
{
	InputPS output;
		
	// Transform the position from object space to homogeneous projection space
	output.Position = mul(input.Position, WorldViewProjectionT);
	output.Height = (output.Position.y + YMIN) / (YMAX-YMIN);
	//output.Height = output.Position.y;
	
	return output;    
}


float4
PixelShader(InputPS input) : COLOR0
{
	//return AmbientColor;
	return float4(input.Height, 0, 0, 1);
}


TexturedInputPS
TexturedVertexShader(TexturedInputVS input)
{
	TexturedInputPS output;
		
	// Transform the position from object space to homogeneous projection space
	output.Position = mul(input.Position, WorldViewProjectionT);
	
	output.TextureCoord = input.TextureCoord;
	
	return output;    
}


float4
TexturedPixelShader(TexturedInputPS input) : COLOR0
{
	return AmbientColor * tex2D(PosseTextureSampler, input.TextureCoord.xy);
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
		VertexShader			= compile vs_2_0 VertexShader();
		PixelShader				= compile ps_2_0 PixelShader();
		AlphaBlendEnable	= false;
		CullMode					= CCW;
		FillMode					= Solid;//<FillMode>;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false; //true;
	}
}

