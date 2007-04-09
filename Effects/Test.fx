#include <CommonFunctions.hlsl>
#include <Glass.hlsl>
#include <SkyBox.hlsl>
#include <ChessBoard.hlsl>
 
struct InputVS
{
	float4	Position			: POSITION;			// Vertex Position
	float3	BlendWeights	: BLENDWEIGHT;	// Blend weight
	int4		BlendIndices	: BLENDINDICES;	// Bland indices
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
SimpleVertexShader(InputVS input,
									 uniform int numWeights)
{
	InputPS output;

	// Calculate new position, normal and tangent depending on animation
	AnimatedVertex_PNT vertex = AnimateVertex(input.Position, input.Normal, input.Tangent, input.BlendIndices, input.BlendWeights, numWeights);

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(vertex.Position, WorldViewProjectionT);

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

struct TestStruct
{
	float4	Position			:	POSITION;
	//float2	TextureCoord	:	TEXCOORD0;
	//float		Light					:	TEXCOORD1;
};

TestStruct
TestVertexShader(InputVS input)
{
	TestStruct output;

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(input.Position, WorldViewProjectionT);
	//float3 normal = normalize(mul(input.Normal, WorldT));
	//output.Light = abs(dot(normal, normalize(float3(0, 0, -1))));
	//output.Light = output.Light * output.Light;
	//output.TextureCoord = input.TextureCoord;
	return output;
}

float4
TestPixelShader(TestStruct input) : COLOR0
{
	//float diffuse = 0.1f + 0.9f * input.Light;
	return 1;//diffuse * tex2D(BaseTextureSampler, input.TextureCoord.xy);
}



technique Test
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 TestVertexShader();
		PixelShader				= compile ps_2_0 TestPixelShader();
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	false;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode					= None;
	}
}

technique TransparentText
<
	bool NormalMapping = false;
	bool Skinning = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SimpleVertexShader(0);
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

technique SkinningNone
<
	bool NormalMapping = false;
	bool Skinning = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SimpleVertexShader(0);
		PixelShader				= compile ps_2_0 SimplePixelShader();
		AlphaBlendEnable	= false;
		FillMode					= Solid;//<FillMode>;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false; //true;
	}
}

technique Skinning
<
	bool NormalMapping = false;
	bool Skinning = true;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SimpleVertexShader(2);
		PixelShader				= compile ps_2_0 SimplePixelShader();
		AlphaBlendEnable	= false;
		FillMode					= Solid;//<FillMode>;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false; //true;
	}
}

technique NormalMapping
<
	bool NormalMapping = true;
	bool Skinning = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 NormalMappingVertexShader();
		PixelShader				= compile ps_2_0 NormalMappingPixelShader();
		AlphaBlendEnable	= false;
		FillMode					= Solid;//<FillMode>;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false; //true;
	}
}
