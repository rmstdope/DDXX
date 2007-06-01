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
	float4	Diffuse						: COLOR0;
};

float4 FocalPlane = float4(0.0f, 0.0f, 1.0f, -5.0f);
float  HyperfocalDistance = 2.5f;
float  MaxBlurFactor = 12.0f / 13.0f; //3.0f / 4.0f;

DoFPixelInput
DoFVertexShader(DoFVertexInput input)
{
	DoFPixelInput output;

	output.Position = mul(input.Position, WorldViewProjectionT);
	output.Normal = mul(input.Normal, (float3x3)WorldViewT);

	// Tranform the position from object space to view space
	float3 ViewPosition = mul(input.Position, (float4x3)WorldViewT);

	// Compute blur factor and place in output alpha
	float BlurFactor = dot(float4(ViewPosition, 1.0), FocalPlane) * HyperfocalDistance;
	output.Diffuse.a = BlurFactor * BlurFactor;

	// Put a cap on the max blur value.  This is required to ensure that the center pixel
	// is always weighted in the blurred image.  I.E. in the PS11 case, the correct maximum
	// value is (NumSamples - 1) / NumSamples, otherwise at BlurFactor == 1.0f, only the outer
	// samples are contributing to the blurred image which causes annoying ring artifacts
	output.Diffuse.rgba = min(output.Diffuse.a, MaxBlurFactor);

	return output;
}

float4
DoFPixelShader(DoFPixelInput input) : COLOR0
{
	float coord = max(0, dot(LightDirections[0], normalize(input.Normal)));
	float3 diffuse = tex1D(BaseTextureSampler, coord);
	float specular = pow(coord, 16);
	specular = smoothstep(0.299, 0.3, specular);
	float3 color = diffuse + specular;
	//return float4(color, input.Diffuse.r);
	return input.Diffuse;
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



