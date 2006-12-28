#include <CommonFunctions.hlsl>
 
struct InputVS
{
	float4	Position			: POSITION;			// Vertex Position
	float3	Normal				: NORMAL;				// Vertex Normal
	float3	Tangent				: TANGENT;			// Vertex Tangent
	float2	TextureCoord	: TEXCOORD0;		// Vertex Texture Coordinate
};

struct InputPS
{
	float4 Position				:	POSITION;			// Vertex position 
	float2 TextureCoord		:	TEXCOORD0;		// Vertex texture coords 
};

struct NormalMappingInputPS
{
	float4 Position				:	POSITION;			// Vertex position 
	float2 TextureCoord		:	TEXCOORD0;		// Vertex texture coords 
	float3 LightVector		:	TEXCOORD1;		// Light vector in tangent space
};


InputPS
SimpleVertexShader(InputVS input)
{
	InputPS output;

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(input.Position, WorldViewProjectionT);

	output.TextureCoord = input.TextureCoord;
	
	return output;    
}

NormalMappingInputPS
NormalMappingVertexShader(InputVS input)
{
	NormalMappingInputPS output;

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(input.Position, WorldViewProjectionT);

	// Generate tangent space base vectors
	float3x3 toTangent = GetTangentSpaceBase(input.Normal, input.Tangent, WorldT);

	float3 light = float3(0, 0, -1);
	output.LightVector = mul(light, toTangent);

	output.TextureCoord = input.TextureCoord;
	
	return output;    
}

float4
SimplePixelShader(InputPS input) : COLOR0
{
	return AmbientColor * tex2D(BaseTextureSampler, input.TextureCoord.xy);
}

float4
NormalMappingPixelShader(NormalMappingInputPS input) : COLOR0
{
	float3 normal = tex2D(NormalTextureSampler, input.TextureCoord.xy).rgb * 2.0 - 1.0;
	float diffuse = dot(normal, input.LightVector);
	return diffuse * tex2D(BaseTextureSampler, input.TextureCoord.xy);
}

technique Test
{
	pass BasePass
	{
		VertexShader			= null;//compile vs_2_0 VertexShader();
		PixelShader				= null;//compile ps_2_0 PixelShader();
		AlphaBlendEnable	= false;//true;
		FillMode					= Solid;//<FillMode>;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false; //true;
		StencilFunc				= Equal;
		StencilPass				= Incr;
	}
}

technique TransparentText
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SimpleVertexShader();
		PixelShader				= compile ps_2_0 SimplePixelShader();
		AlphaBlendEnable	= true;
		BlendOp						= ADD;
		SrcBlend					= BLENDFACTOR;
		DestBlend					= ONE;
		BlendFactor				= 0x80808080;
		CullMode					= None;
		FillMode					= Solid;//<FillMode>;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false; //true;
		StencilFunc				= Equal;
		StencilPass				= Incr;
	}
}

technique NoSkinning
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SimpleVertexShader();
		PixelShader				= compile ps_2_0 SimplePixelShader();
		AlphaBlendEnable	= false;
		CullMode					= CCW;
		FillMode					= Solid;//<FillMode>;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false; //true;
	}
}

technique NormalMapping
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 NormalMappingVertexShader();
		PixelShader				= compile ps_2_0 NormalMappingPixelShader();
		AlphaBlendEnable	= false;
		CullMode					= CCW;
		FillMode					= Solid;//<FillMode>;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false; //true;
	}
}
