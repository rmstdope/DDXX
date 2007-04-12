struct GlassVertexInput
{
	float4	Position					: POSITION;
	float3	Normal						: NORMAL;
	float3	Tangent						: TANGENT;
	float2	TextureCoord			: TEXCOORD0;
};

struct GlassPixelInput
{
	float4	Position					:	POSITION;
	float4	Diffuse						:	COLOR0;
	float2	TextureCoord			:	TEXCOORD0;
	float3	ReflectionVector	: TEXCOORD2;
};

GlassPixelInput
GlassVertexShader(GlassVertexInput input)
{
	GlassPixelInput output;

	// Transform the position from object space to homogeneous projection space
	float3 positionWS = mul(input.Position, WorldT);
	output.Position = mul(input.Position, WorldViewProjectionT);
	float3 normal = normalize(mul(input.Normal, WorldT));
	output.Diffuse = 0;
	for (int i = 0; i < 1; i++)
	{
		float3 lightVec = LightPositions[i] - positionWS;
		float d = length(lightVec);
		float attenuation = saturate(6 / (d * d));
		output.Diffuse += LightDiffuseColors[i];// * saturate(dot(normal, normalize(lightVec))) * attenuation;
	}
	output.Diffuse *= MaterialDiffuseColor;
	output.TextureCoord = input.TextureCoord;
	float3 eyeVector =  positionWS - EyePosition.xyz;
	output.ReflectionVector = reflect(eyeVector, normal);

	return output;
}

float4
GlassPixelShader(GlassPixelInput input) : COLOR0
{
	float4 diffuse = 0.05 + 0.95 * input.Diffuse;
	float4 color = diffuse * tex2D(BaseTextureSampler, input.TextureCoord.xy);
	float4 reflection = texCUBE(ReflectiveTextureSampler, input.ReflectionVector.xyz);
	return lerp(color, reflection, ReflectiveFactor);
}

technique GlassEffect
<
	bool NormalMapping = false;
	bool Skinning = false;
>
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 GlassVertexShader();
		PixelShader				= compile ps_2_0 GlassPixelShader();
		AlphaBlendEnable	= false;
		FillMode					= Solid;
		//FillMode					= Wireframe;
		ZEnable						=	true;
		ZFunc							= Less;
		StencilEnable			= false;
		CullMode					= None;
	}
}



