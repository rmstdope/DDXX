#include <DataTypes.hlsl>

struct ReflectivePixelInput
{
	float4	Position			:	POSITION;
	float2	TexCoords			:	TEXCOORD0;
	float3	Reflection		:	TEXCOORD1;
};

ReflectivePixelInput
ReflectiveVertexShader(VertexShaderInput input,
									uniform int numWeights)
{
	ReflectivePixelInput output;

	// Transform the position from object space to homogeneous projection space
	AnimatedVertex_PN animated = AnimateVertex(input.Position, input.Normal, input.BlendIndices, input.BlendWeights, numWeights);

	// Transform position and normal to world space
	float3 worldPosition = mul(animated.Position, WorldT);
	float3 worldNormal = mul(animated.Normal, (float3x3)WorldT);
	// Get eye vector
	float3 eyeVector = EyePosition - worldPosition;
	// Reflect eye vector around normal
	output.Reflection = reflect(eyeVector, worldNormal);

	// Transform to screen space
	output.Position = mul(animated.Position, WorldViewProjectionT);

	// Copy texture coordinates
	output.TexCoords = input.TexCoords;

	return output;
}

float4
ReflectivePixelShader(ReflectivePixelInput input) : COLOR0
{
	float4 reflection = texCUBE(ReflectiveTextureSampler, input.Reflection);
	reflection = reflection.a;
	float4 color = tex2D(BaseTextureSampler, input.TexCoords) * AmbientColor;
	float factor = ReflectiveFactor;// * reflection.a;
	return lerp(color, reflection, factor);
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
		CullMode					= CCW;
	}
}
