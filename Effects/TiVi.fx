#include <CommonFunctions.hlsl>
#include <AtmosphereShader.hlsl>
#include <Simple.hlsl>
#include <Reflective.hlsl>
#include <DepthOfField.hlsl>
 
struct TerrainPixelInput
{
	float4 Position				:	POSITION;			// Vertex position 
	float4 Color					: COLOR0;				// Vertex color
	float2 TextureCoord		:	TEXCOORD0;		// Vertex texture coords 
};

struct DiamondPixelInput
{
	float4 Position				:	POSITION;			// Vertex position 
	float4 DiffuseColor		: COLOR0;				// Vertex diffuse color
	float4 SpecularColor	: COLOR1;				// Vertex specular color
	float2 TextureCoord		:	TEXCOORD0;		// Vertex texture coords 
};

struct InputVS
{
	float4	Position			: POSITION;			// Vertex Position
	float3	BlendWeights	: BLENDWEIGHT;	// Blend weight
	int4		BlendIndices	: BLENDINDICES;	// Bland indices
	float3	Normal				: NORMAL;				// Vertex Normal
	float3	Tangent				: TANGENT;			// Vertex Tangent
	float2	TextureCoord	: TEXCOORD0;		// Vertex Texture Coordinate
};

struct SolidPixelInput
{
	float4	Position			:	POSITION;
	float2	TexCoords			:	TEXCOORD0;
	float3	Diffuse				:	TEXCOORD1;
	float3	Specular			:	TEXCOORD2;
};

SolidPixelInput
SolidVertexShader(VertexShaderInput input,
									uniform int numWeights)
{
	SolidPixelInput output;

	// Transform the position from object space to homogeneous projection space
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);
	output.Position = mul(animated.Position, WorldViewProjectionT);
	float3 normal = normalize(mul(animated.Normal, WorldViewT));
	float vDotN = saturate(dot(normal, float3(0, 0, -1)));
	output.Diffuse = AmbientColor;
	output.Diffuse += vDotN * MaterialDiffuseColor;
	output.Specular = pow(vDotN, MaterialShininess) * MaterialSpecularColor;
	output.TexCoords = input.TexCoords;
	return output;
}

float4
SolidPixelShader(SolidPixelInput input,
								 uniform sampler textureSampler) : COLOR0
{
	return float4(input.Diffuse, 1) * tex2D(textureSampler, input.TexCoords) + float4(input.Specular, 1);
}

float4
SolidPixelShader2(SolidPixelInput input) : COLOR0
{
	return float4(input.Diffuse, 1) * tex2D(BaseTextureSamplerBordered, input.TexCoords) + float4(input.Specular, 1);
}

float4
WhitePixelShader(SolidPixelInput input) : COLOR0
{
	return 1;
}

float4
AmbientPixelShader(SolidPixelInput input) : COLOR0
{
	return AmbientColor;
}

float4
NoTexturePixelShader(TerrainPixelInput input) : COLOR0
{
	return input.Color;
}

float4
ScreenPixelShader(SolidPixelInput input,
								 uniform sampler textureSampler) : COLOR0
{
	return tex2D(textureSampler, input.TexCoords);
}

struct LinePixelInput
{
	float4	Position			:	POSITION;
	float3	Light					:	TEXCOORD1;
};

LinePixelInput
LineVertexShader(VertexShaderInput input,
								 uniform int numWeights)
{
	LinePixelInput output;

	// Transform the position from object space to homogeneous projection space
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);
	output.Position = mul(animated.Position, WorldViewProjectionT);
	float3 normal = normalize(mul(animated.Normal, WorldViewT));
	float vDotN = saturate(dot(normal, float3(0, 0, -1)));
	output.Light = AmbientColor;
	output.Light += vDotN * MaterialDiffuseColor;
	output.Light += pow(vDotN, MaterialShininess) * MaterialSpecularColor;
	return output;
}

float4
LinePixelShader(LinePixelInput input) : COLOR0
{
	return float4(input.Light, 1);
}

technique SolidSkinning
<
	bool NormalMapping = false;
	bool Skinning = true;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SolidVertexShader(4);
		PixelShader				= compile ps_2_0 ScreenPixelShader(BaseTextureSampler);
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
	}
}

float4
TiViReflectivePixelShader(ReflectivePixelInput input) : COLOR0
{
	float3 reflection = texCUBE(ReflectiveTextureSampler, input.Reflection);
	float m = min(min(reflection.r, reflection.g), reflection.b);
	m = 4 * pow(m , 16);
	reflection = reflection * m;
	float3 color = tex2D(BaseTextureSampler, input.TexCoords) * (input.Diffuse + AmbientColor);
	//float factor = ReflectiveFactor;// * reflection.a;
	float factor = saturate((input.VdN * input.VdN) * ReflectiveFactor);
	return float4(lerp(color, reflection, factor), ReflectiveFactor);
	//return input.Diffuse;
	//return float4(reflection * ReflectiveFactor + color, 1);
}

technique TiViReflectiveSkinning
<
	bool NormalMapping = false;
	bool Skinning = true;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 ReflectiveVertexShader(4);
		PixelShader				= compile ps_2_0 ReflectivePixelShader();
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
	}
}

technique BallzieReflectiveSkinning
<
	bool NormalMapping = false;
	bool Skinning = true;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 ReflectiveVertexShader(1);
		PixelShader				= compile ps_2_0 ReflectivePixelShader();
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
	}
}

technique TvScreenSkinning
<
	bool NormalMapping = false;
	bool Skinning = true;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SolidVertexShader(4);
		PixelShader				= compile ps_2_0 SimplePixelShader(BaseTextureSamplerBordered);
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
	}
}

technique WhiteScreenSkinning
<
	bool NormalMapping = false;
	bool Skinning = true;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 SolidVertexShader(4);
		PixelShader				= compile ps_2_0 WhitePixelShader();
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
	}
}

technique LineDrawerSkinning
<
	bool NormalMapping = false;
	bool Skinning = true;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 LineVertexShader(4);
		PixelShader				= compile ps_2_0 LinePixelShader();
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Wireframe;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
		CullMode					= None;
	}
}

float3 lightDir = normalize(float3(1, 1, 0));

TerrainPixelInput
TerrainVertexShader(InputVS input,
									 uniform int numWeights)
{
	TerrainPixelInput output;

	// Calculate new position, normal and tangent depending on animation
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);
	float3 normal = normalize(mul(animated.Normal, WorldT));
	//normal = float3(0, -1, 0);

	// Transform the position from object space to world space
	float3 worldPosition = mul(animated.Position, WorldT);

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(animated.Position, WorldViewProjectionT);

	output.TextureCoord = input.TextureCoord;
	float4 diffuse = 0;
	for (int i = 0; i < NumLights; i++) {
		float3 dir = LightPositions[i] - worldPosition;
		float att = 1;//1 - min(1, length(dir) * length(dir) / 5000);
		diffuse += att * LightDiffuseColors[i] * max(0, dot(normal, normalize(dir)));
	}
	output.Color = AmbientColor + MaterialDiffuseColor * diffuse;
	
	return output;    
}

TerrainPixelInput
BrickVertexShader(InputVS input,
									uniform int numWeights)
{
	TerrainPixelInput output;

	// Calculate new position, normal and tangent depending on animation
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);
	float3 normal = normalize(mul(animated.Normal, WorldT));
	//normal = float3(0, -1, 0);

	// Transform the position from object space to world space
	float3 worldPosition = mul(animated.Position, WorldT);

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(animated.Position, WorldViewProjectionT);

	output.TextureCoord = input.TextureCoord;
	float4 diffuse = 0;
	for (int i = 0; i < NumLights; i++) {
		float3 dir = LightPositions[i] - worldPosition;
		float d = length(dir);
		float att = 1 / (d * d * 0.05f);
		diffuse += att * LightDiffuseColors[i] * abs(dot(normal, normalize(dir)));
	}
	output.Color = AmbientColor + MaterialDiffuseColor * diffuse;
	
	return output;    
}

DiamondPixelInput
DiamondVertexShader(InputVS input,
									 uniform int numWeights)
{
	DiamondPixelInput output;

	// Calculate new position, normal and tangent depending on animation
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);

	// Transform the position from object space to world space
	float3 viewNormal = normalize(mul(animated.Normal, WorldViewT));

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(animated.Position, WorldViewProjectionT);

	output.TextureCoord = input.TextureCoord;

	float diffuse = abs(dot(viewNormal, float3(0, 0, -1)));
	float specular = 20 * pow(diffuse, 64);
	output.DiffuseColor = AmbientColor + MaterialDiffuseColor * diffuse;
	output.SpecularColor = specular * MaterialSpecularColor;
	
	return output;    
}

float4
DiamondPixelShader(DiamondPixelInput input) : COLOR0
{
	return input.SpecularColor + input.DiffuseColor * tex2D(BaseTextureSampler, input.TextureCoord.xy);
}

float4
TerrainPixelShader(TerrainPixelInput input) : COLOR0
{
	return input.Color * tex2D(BaseTextureSampler, input.TextureCoord.xy);
}

technique Terrain
<
	bool NormalMapping = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 TerrainVertexShader(0);
		PixelShader				= compile ps_2_0 TerrainPixelShader();
		AlphaBlendEnable	= false;
		ZEnable						=	true;
		ZFunc							= Less;
	}
}

technique NoTexture
<
	bool NormalMapping = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 TerrainVertexShader(0);
		PixelShader				= compile ps_2_0 NoTexturePixelShader();
		AlphaBlendEnable	= false;
		ZEnable						=	true;
		ZFunc							= Less;
	}
}

technique NoTextureAmbient
<
	bool NormalMapping = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 TerrainVertexShader(0);
		PixelShader				= compile ps_2_0 AmbientPixelShader();
		AlphaBlendEnable	= false;
		ZEnable						=	true;
		ZFunc							= Less;
	}
}

technique Bricks
<
	bool NormalMapping = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 BrickVertexShader(0);
		PixelShader				= compile ps_2_0 TerrainPixelShader();
		AlphaBlendEnable	= true;
		BlendOp						= Add;
		SrcBlend					= One;
		DestBlend					= One;
		ZEnable						=	true;
		ZFunc							= Less;
		CullMode					= CCW;
	}
}

float4
TestPixelShader(TerrainPixelInput input) : COLOR0
{
	return input.Color * tex2D(BaseTextureSampler, input.TextureCoord.xy);
}

technique AlphaTest
<
	bool NormalMapping = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 TerrainVertexShader(0);
		PixelShader				= compile ps_2_0 TestPixelShader();
		AlphaBlendEnable	= true;
		BlendOp						= Add;
		SrcBlend					= One;
		DestBlend					= One;
		ZEnable						=	true;
		ZFunc							= Less;
		CullMode					= None;
	}
}

technique Diamond
<
	bool NormalMapping = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 DiamondVertexShader(0);
		PixelShader				= compile ps_2_0 DiamondPixelShader();
		AlphaBlendEnable	= false;
		//BlendOp						= Add;
		//SrcBlend					= SrcAlpha;
		//DestBlend					= One;
		ZEnable						=	true;
		ZFunc							= Less;
	}
}


struct RoomPixelInput
{
	float4	Position			:	POSITION;
	float4	Diffuse				:	COLOR0;
};

RoomPixelInput
RoomVertexShader(VertexShaderInput input,
									uniform int numWeights)
{
	RoomPixelInput output;

	// Transform the position from object space to homogeneous projection space
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);

	// Transform position and normal to world space
	float3 worldPosition = mul(animated.Position, WorldT);
	float3 worldNormal = normalize(mul(animated.Normal, (float3x3)WorldT));

	// Transform to screen space
	output.Position = mul(animated.Position, WorldViewProjectionT);

	float4 diffuse = 0;
	for (int i = 0; i < NumLights; i++) {
		float3 dir = LightPositions[i] - worldPosition;
		float d = length(dir);
		float att = 1 / (d * d * 0.02f);
		diffuse += att * LightDiffuseColors[i] * max(0, dot(worldNormal, normalize(dir)));
	}
	output.Diffuse = diffuse * MaterialDiffuseColor;

	return output;
}

float4
RoomPixelShader(RoomPixelInput input) : COLOR0
{
	float4 color = (input.Diffuse + AmbientColor);
	return color;
}

technique Room
<
	bool NormalMapping = false;
	bool Skinning = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 RoomVertexShader(0);
		PixelShader				= compile ps_2_0 RoomPixelShader();
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
		CullMode					= CCW;
	}
}

technique TiViWalkwayMirror
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 AtmosphereVertexShader();
		PixelShader				= compile ps_2_0 AtmospherePixelShader();
		AlphaBlendEnable	= true;
		BlendOp						= Add;
		SrcBlend					= One;
		DestBlend					= One;
		ZEnable						= true;
		ZFunc							= Always;
		ZWriteEnable			= true;
	}
}

float4
RudimentaryVertexShader(float4 position : POSITION) : POSITION
{
	return mul(position, WorldViewProjectionT);
}

float4
RudimentaryPixelShader() : COLOR
{
	return 1;
}

technique StencilOnly
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 RudimentaryVertexShader();
		PixelShader				= compile ps_2_0 RudimentaryPixelShader();
		AlphaBlendEnable	= false;
		ZEnable						= true;
		ZFunc							= Never;
		//ZFunc							= Always;
		ZWriteEnable			= false;
		StencilEnable			= true;
		StencilFunc				=	Always;
		StencilZFail			= Replace;
		//StencilPass				= Replace;
		StencilRef				= 1;
	}
}

technique TiviChessPiece
<
	bool NormalMapping = false;
	bool Skinning = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 ReflectiveVertexShader(0);
		PixelShader				= compile ps_2_0 ReflectivePixelShader();
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
		ColorWriteEnable	= 0xF;
	}
	pass DofPass
	{
		VertexShader			= compile vs_2_0 PureDoFVertexShader();
		PixelShader				= compile ps_2_0 PureDoFPixelShader();
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= false;
		ZFunc							= Equal;
		StencilEnable			= false;
		ColorWriteEnable	= 0x8;
	}
}


technique TiViChessBoard
<
	bool NormalMapping = false;
	bool Skinning = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 ReflectiveVertexShader(0);
		PixelShader				= compile ps_2_0 ReflectivePixelShader();
		AlphaTestEnable		= false;
		AlphaBlendEnable	= true;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= true;
		ZFunc							= Less;
		StencilEnable			= false;
		BlendOp						= Add;
		SrcBlend					= One;
		DestBlend					= SrcAlpha;
		ColorWriteEnable	= 0xF;
	}
	pass DofPass
	{
		VertexShader			= compile vs_2_0 PureDoFVertexShader();
		PixelShader				= compile ps_2_0 PureDoFPixelShader();
		AlphaTestEnable		= false;
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZWriteEnable			= false;
		ZFunc							= Equal;
		StencilEnable			= false;
		ColorWriteEnable	= 0x8;
	}
}
