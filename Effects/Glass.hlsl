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
	float2	TextureCoord			:	TEXCOORD0;
	float		Light							:	TEXCOORD1;
	float3	ReflectionVector	: TEXCOORD2;
};

GlassPixelInput
GlassVertexShader(GlassVertexInput input)
{
	GlassPixelInput output;

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(input.Position, WorldViewProjectionT);
	float3 normal = normalize(mul(input.Normal, WorldT));
	output.Light = abs(dot(normal, normalize(float3(0, 0, -1))));
	output.Light = output.Light * output.Light;
	output.TextureCoord = input.TextureCoord;
	float3 eyeVector =  mul(input.Position.xyz, WorldT) - EyePosition.xyz;
	output.ReflectionVector = reflect(eyeVector, normal);

	return output;
}

float4
GlassPixelShader(GlassPixelInput input) : COLOR0
{
	float diffuse = 0.1f + 0.9f * input.Light;
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



