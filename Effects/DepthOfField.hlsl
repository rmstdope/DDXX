struct DoFVertexInput
{
	float4	Position					: POSITION;
	float3	Normal						: NORMAL;
	float2	TextureCoord			: TEXCOORD0;
};

struct DoFPixelInput
{
	float4	Position					:	POSITION;
	float3	Normal						:	TEXCOORD0;
};

//float3 lightVector = normalize(float3(1, 2, -5));

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
	float coord = max(0, dot(LightDirections[0], normalize(input.Normal)));
	float4 diffuse = tex1D(BaseTextureSampler, coord);
	float specular = pow(coord, 16);
	specular = smoothstep(0.299, 0.3, specular);
	return diffuse + specular;
	//return specular;
}

float4
OutlineVertexShader(DoFVertexInput input) : POSITION
{
	return mul(input.Position + float4(input.Normal * 0.02, 0), WorldViewProjectionT);
}

float4
OutlinePixelShader() : COLOR0
{
	return 0;
}

technique CelWithDoF
<
	bool NormalMapping = false;
	bool Skinning = false;
>
{
	pass Outline
	{
		VertexShader			= compile vs_2_0 OutlineVertexShader();
		PixelShader				= compile ps_2_0 OutlinePixelShader();
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode					= CW;
	}
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



