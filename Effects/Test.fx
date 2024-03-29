#include <CommonFunctions.hlsl>
#include <Glass.hlsl>
#include <SkyBox.hlsl>
#include <ChessBoard.hlsl>
#include <DepthOfField.hlsl>
 
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
	float		Light					:	TEXCOORD1;
};

TestStruct
TestVertexShader(InputVS input,
								 uniform int numWeights)
{
	TestStruct output;

	// Transform the position from object space to homogeneous projection space
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);
	output.Position = mul(animated.Position, WorldViewProjectionT);
	float3 normal = normalize(mul(animated.Normal, WorldT));
	output.Light = saturate(dot(normal, float3(0, 0, -1)));
	//output.Position = 0;
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
	return 0.1f + input.Light;//diffuse * tex2D(BaseTextureSampler, input.TextureCoord.xy);
}

technique LineTest
<
	bool NormalMapping = false;
	bool Skinning = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 TestVertexShader(0);
		PixelShader				= compile ps_2_0 TestPixelShader();
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode					= None;
	}
}

technique LineTestSkinning
<
	bool NormalMapping = false;
	bool Skinning = true;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 TestVertexShader(2);
		PixelShader				= compile ps_2_0 TestPixelShader();
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
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
		CullMode						= None;
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

struct TerrainPixelInput
{
	float4 Position				:	POSITION;			// Vertex position 
	float4 Color					: COLOR0;				// Vertex color
	float2 TextureCoord		:	TEXCOORD0;		// Vertex texture coords 
};

float3 lightDir = normalize(float3(1, 1, 0));

TerrainPixelInput
TerrainVertexShader(InputVS input,
										uniform float yAdd,
										uniform int numWeights)
{
	TerrainPixelInput output;

	// Calculate new position, normal and tangent depending on animation
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);
	animated.Position.y += yAdd;
	float3 normal = normalize(mul(animated.Normal, WorldT));
	//normal = float3(0, -1, 0);

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(animated.Position, WorldViewProjectionT);

	output.TextureCoord = input.TextureCoord;
	output.Color = AmbientColor + MaterialDiffuseColor * saturate(dot(normal, lightDir));
	
	return output;    
}

float4
TerrainPixelShader(TerrainPixelInput input) : COLOR0
{
	return input.Color * tex2D(BaseTextureSampler, input.TextureCoord.xy);
}

float4
ColorOutput(uniform float4 color) : COLOR0
{
	return color;
}

technique Terrain
<
	bool NormalMapping = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 TerrainVertexShader(0, 0);
		PixelShader				= compile ps_2_0 TerrainPixelShader();
		AlphaBlendEnable	= false;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode						= None;
	}
	pass WireframePass
	{
		VertexShader			= compile vs_2_0 TerrainVertexShader(0.01, 0);
		PixelShader				= compile ps_2_0 ColorOutput(float4(1.0, 1.0, 1.0, 1));
		AlphaBlendEnable	= false;
		ZEnable						=	true;
		ZFunc							= LessEqual;
		StencilEnable			= false;
		CullMode					= None;
		FillMode					= Wireframe;
	}
}


float WaveTextTime;

float4
WaveTextPixelShader(float2 Tex : TEXCOORD0) : COLOR0
{
	float2 centered = Tex - 0.5;
	centered = centered * (sin(WaveTextTime + length(centered.x + centered.y) * 0.5) + 2) / 3 * (5 / WaveTextTime);// + length(Tex));
	centered += 0.5f;
	//float2 sc;
	//sincos(WaveTextTime, sc.x, sc.y);
	return tex2D(BaseTextureSampler, centered);
}

technique WaveText
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 WaveTextPixelShader();
		ZEnable						= false;
		CullMode					= None;
	}
}

