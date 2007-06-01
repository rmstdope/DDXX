struct DoFVertexInput
{
	float4	Position					: POSITION;
	float3	Normal						: NORMAL;
	//float3	Tangent						: TANGENT;
	float2	TextureCoord			: TEXCOORD0;
};

struct DoFPixelInput
{
	float4	Position					:	POSITION;
	float3	Normal						:	TEXCOORD0;
	//float2	TextureCoord			:	TEXCOORD0;
	//float3	ReflectionVector	: TEXCOORD2;
};

float3 lightVector = float3(0, 0, -1);

DoFPixelInput
DoFVertexShader(DoFVertexInput input)
{
	DoFPixelInput output;

	output.Position = mul(input.Position, WorldViewProjectionT);
	output.Normal = mul(input.Normal, (float3x3)WorldViewT);

	return output;
}

float4
DoFPixelShader(DoFPixelInput input) : COLOR0
{
	float coord = dot(lightVector, input.Normal);
	float4 color = tex1D(BaseTextureSampler, coord);
	return color;
}

technique CelWithDoF
<
	bool NormalMapping = false;
	bool Skinning = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 DoFVertexShader();
		PixelShader				= compile ps_2_0 DoFPixelShader();
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode					= CCW;
	}
}



