#include <CommonFunctions.hlsl>
 
struct InputVS
{
	float4	Position			: POSITION;			// Vertex Position
	//float3	Normal				: NORMAL;				// Vertex Normal
	//float3	Tangent				: TANGENT;			// Vertex Tangent
	float2	TextureCoord	: TEXCOORD0;		// Vertex Texture Coordinate
};

struct InputPS
{
	float4 Position			:	POSITION;			// Vertex position 
	float2 TextureCoord	:	TEXCOORD0;		// Vertex texture coords 
	//float3 LightVector	:	TEXCOORD1;		// Light vector in tangent space
	//float3 EyeVector		:	TEXCOORD2;		// Eye vector in tangent space
	//float2 AttCoord1		:	TEXCOORD3;		// X and Y coordinates for attenuaton texture
	//float2 AttCoord2		:	TEXCOORD4;		// Z coordinate for attenuation texture
};


InputPS
VertexShader(InputVS input)
{
	InputPS output;

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(input.Position, WorldViewProjectionT);
	
	output.TextureCoord = input.TextureCoord;
	//float3 N = normalize(mul(input.Normal, (float3x3)WorldT));

	//float3 L1 = normalize(LightPosition[0] - mul(input.Position, WorldT));
	//float3 L2 = normalize(LightPosition[1] - mul(input.Position, WorldT));
	//output.LightVector = max(0, dot(N, L1)) * LightDiffuseColor[0] + max(0, dot(N, L2)) * LightDiffuseColor[1];
	//float3 V = normalize(EyePosition.xyz - input.Position.xyz);
	//float3 H = normalize(V + normalize(output.L));
	
	return output;    
}

float4
PixelShader(InputPS input) : COLOR0
{
	return AmbientColor * tex2D(BaseTextureSampler, input.TextureCoord.xy);
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
		VertexShader			= compile vs_2_0 VertexShader();
		PixelShader				= compile ps_2_0 PixelShader();
		AlphaBlendEnable	= true;
		BlendOp						= Add;
		SrcBlend					= BlendFactor;
		DestBlend					= One;
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

