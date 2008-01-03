struct DoFVertexInput
{
	float4	Position					: POSITION;
	float3	Normal						: NORMAL;
	float2	TextureCoord			: TEXCOORD0;
};

struct DoFPixelInput
{
	float4	Position					:	POSITION;
	float4	Diffuse						: COLOR0;
};

float4 FocalPlane = float4(0.0f, 0.0f, 1.0f, -6.0f);
float HyperfocalDistance = 0.08f;
float MaxBlurFactor = 12.0f / 13.0f;
float ChamferAdd = 0.06;

DoFPixelInput
PureDoFVertexShader(DoFVertexInput input)
{
	DoFPixelInput output;

	// transform the position into projection space
	output.Position = mul(input.Position, World[0]);
	output.Position = mul(output.Position, View);
	output.Position = mul(output.Position, Projection);

	// Tranform the position from object space to view space
	float3 ViewPosition = mul(input.Position, (float4x3)(World[0] * View));

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


