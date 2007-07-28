#include <DataTypes.hlsl>

struct ReflectivePixelInput
{
	float4	Position			:	POSITION;
	float2	TexCoords			:	TEXCOORD0;
	float3	Reflection		:	TEXCOORD1;
	float		VdN						: TEXCOORD2;
	float4	Diffuse				:	COLOR0;
};

ReflectivePixelInput
ReflectiveVertexShader(VertexShaderInput input,
									uniform int numWeights)
{
	ReflectivePixelInput output;

	float3 n = input.Normal;

	// Transform the position from object space to homogeneous projection space
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);

	// Transform position and normal to world space
	float4 worldPosition = mul(animated.Position, WorldT);
	float3 worldNormal = normalize(mul(animated.Normal, WorldT));

	// Transform to screen space
	output.Position = mul(animated.Position, WorldViewProjectionT);

	// Copy texture coordinates
	output.TexCoords = input.TexCoords;

	float4 diffuse = 0;
	for (int i = 0; i < NumLights; i++) {
		float3 dir = LightPositions[i] - worldPosition;
		float d = length(dir);
		float att = 1;//min(1, 1 / (d * d * LightRanges[i]));
		diffuse += att * LightDiffuseColors[i] * max(0, dot(worldNormal, normalize(dir)));
	}
	output.Diffuse = diffuse * MaterialDiffuseColor;

	// Get eye vector
	float3 eyeVector = normalize(EyePosition - worldPosition);
	// Reflect eye vector around normal
	output.Reflection = reflect(worldNormal, eyeVector);
	output.VdN = dot(eyeVector, worldNormal);

	// Reflection vector is R = (2V.N)N - V
	//float3 viewVector = normalize(eyeVector);
	//float normal = worldNormal;
	//output.Reflection = dot(viewVector * 2, normal) * normal - viewVector;

	return output;
}

float4
ReflectivePixelShader(ReflectivePixelInput input) : COLOR0
{
	float3 reflection = texCUBE(ReflectiveTextureSampler, input.Reflection);
	//float m = min(min(reflection.r, reflection.g), reflection.b);
	//m = pow(m , 8);
	//reflection = reflection * m;
	//reflection *= 2;//reflection.a * reflection.a * 2;
	float3 color = tex2D(BaseTextureSampler, input.TexCoords) * (input.Diffuse + AmbientColor);
	//float factor = ReflectiveFactor;// * reflection.a;
	float factor = saturate((input.VdN * input.VdN) * ReflectiveFactor);
	return float4(lerp(color, reflection, factor), ReflectiveFactor);
	//return color;
}

technique Reflective
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
		StencilEnable			= false;
	}
}

technique ReflectiveTransparent
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
	}
}
