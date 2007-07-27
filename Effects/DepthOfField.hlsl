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
	float3	InvEye						:	TEXCOORD1;
	float4	Diffuse						: COLOR0;
};

struct OutlinePixelInput
{
	float4	Position					:	POSITION;
	float4	Diffuse						: COLOR0;
};

float4 FocalPlane = float4(0.0f, 0.0f, 1.0f, -55.0f);
float HyperfocalDistance = 0.08f;
//float MaxBlurFactor = 3.0f / 4.0f;
float MaxBlurFactor = 12.0f / 13.0f;
float ChamferAdd = 0.06;

DoFPixelInput
DoFVertexShader(DoFVertexInput input)
{
	DoFPixelInput output;

	//if (input.Position.x > 0)
	//	input.Position.x += ChamferAdd;
	//else
	//	input.Position.x -= ChamferAdd;
	//if (input.Position.y > 0)
	//	input.Position.y += ChamferAdd;
	//else
	//	input.Position.y -= ChamferAdd;
	//if (input.Position.z > 0)
	//	input.Position.z += ChamferAdd;
	//else
	//	input.Position.z -= ChamferAdd;
	float3 positionWS = mul(input.Position, WorldT);
	output.Position = mul(input.Position, WorldViewProjectionT);
	output.Normal = mul(input.Normal, (float3x3)WorldViewT);
	output.InvEye = EyePosition.xyz - positionWS;

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
	float3 color = AmbientColor;
	float3 normalVector = normalize(input.Normal);
	for (int i = 0; i < 2; i++) {
		float3 halfVector = normalize((normalize(input.InvEye) + LightDirections[i]) / 2);
		float coord = max(0, dot(LightDirections[i], normalVector));
		float3 diffuse = tex1D(BaseTextureSampler, coord);
		float specular = max(0, dot(halfVector, normalVector));
		specular = pow(specular, 8);
		//specular = smoothstep(0.49, 0.5, specular);
		color += (diffuse * MaterialDiffuseColor) + (specular * MaterialSpecularColor);
	}
	return float4(color, input.Diffuse.r);
}

OutlinePixelInput
OutlineVertexShader(DoFVertexInput input)
{
	OutlinePixelInput output;

	if (input.Position.x > 0)
		input.Position.x += ChamferAdd;
	else
		input.Position.x -= ChamferAdd;
	if (input.Position.y > 0)
		input.Position.y += ChamferAdd;
	else
		input.Position.y -= ChamferAdd;
	if (input.Position.z > 0)
		input.Position.z += ChamferAdd;
	else
		input.Position.z -= ChamferAdd;
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
	output.Position = mul(input.Position + float4(input.Normal * 0.02, 0), WorldViewProjectionT);
	return output;
}

float4
OutlinePixelShader(OutlinePixelInput input) : COLOR0
{
	return float4(0, 0, 0, input.Diffuse.r);
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

technique CelWithDoFMirrored
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
		CullMode					= CCW;
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
		CullMode					= CW;
	}
}

DoFPixelInput
PureDoFVertexShader(DoFVertexInput input)
{
	DoFPixelInput output;

	float3 positionWS = mul(input.Position, WorldT);
	output.Position = mul(input.Position, WorldViewProjectionT);
	output.Normal = mul(input.Normal, (float3x3)WorldViewT);
	output.InvEye = EyePosition.xyz - positionWS;

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
PureDoFPixelShader(DoFPixelInput input) : COLOR0
{
	return input.Diffuse;
}


