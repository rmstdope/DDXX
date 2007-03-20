/**
 *
 * TestEffect
 * 
*/

#include <CommonFunctions.hlsl>

struct BlinnPhongInputVS
{
	float4	Position			: POSITION;			// Vertex Position
	float3	BlendWeights	: BLENDWEIGHT;	// Blend weight
	int4		BlendIndices	: BLENDINDICES;	// Bland indices
	float3	Normal				: NORMAL;				// Vertex Normal
	float3	Tangent				: TANGENT;			// Vertex Tangent
	float2	TextureCoord	: TEXCOORD0;		// Vertex Texture Coordinate
};

struct BlinnPhongInputPS
{
	float4 Position			:	POSITION;			// Vertex position 
	float2 TextureCoord	:	TEXCOORD0;		// Vertex texture coords 
	float3 LightVector	:	TEXCOORD1;		// Light vector in tangent space
	float3 EyeVector		:	TEXCOORD2;		// Eye vector in tangent space
	float2 AttCoord1		:	TEXCOORD3;		// X and Y coordinates for attenuaton texture
	float2 AttCoord2		:	TEXCOORD4;		// Z coordinate for attenuation texture
};

/**
 * Calculate normalized light vector and eye vector in tangent space
 * @param Position The position of the vertex in world space
 * @param ToTangent The base vector of the tangent space.
 * @param LightType The type of light
 * @returns the normalized light and half vectors in tangent space
 */
LightVectors
CalculateLightVectors_2(float4 Position,
												float3x3 ToTangent,
												uniform int LightType)
{
	LightVectors output;
	
	if(LightType == DIRECTIONAL) {
		output.L = LightDirection;
	} else if(LightType == POINT) {
		output.L = LightPosition - Position;
	}
	
	output.L = output.L;

	// Hmm, what is wrong?
	//output.H = EyePosition - Position;
	
	// Do this for now!
	float3 V = normalize(EyePosition.xyz - Position.xyz);
	// Calculate and normalize Half vector
	output.H = normalize(V + normalize(output.L));

	output.L = mul(output.L, ToTangent);
	output.H = mul(output.H, ToTangent);

	return output;	
}

/**
 * The Blinn-Phong vertex shader function for 2.x shaders
 * @param inp The input stream of the shader
 * @param LightType set to DIRECTIONAL or POINT depending on light type
 * @returns A struct containing the input stream for the pixel shader
 */
BlinnPhongInputPS
BlinnPhongVertexShader_2_0(BlinnPhongInputVS inp,
													 uniform bool useTextures,
													 uniform int numWeights,
													 uniform int LightType)
{
	BlinnPhongInputPS output;

	// Calculate new position, normal and tangent depending on animation
	AnimatedVertex_PNT vertex = AnimateVertex(inp.Position, inp.Normal, inp.Tangent, inp.BlendIndices, inp.BlendWeights, numWeights);	
	
	// Transform the position from object space to world space
	float4 vertexPosition = mul(vertex.Position, WorldT);

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(vertex.Position, WorldViewProjectionT);
	
	// Generate tangent space base vectors
	float3x3 toTangent = GetTangentSpaceBase(vertex.Normal, vertex.Tangent, WorldT);

	// Calculate light (and eye) vectors im tangent space
	LightVectors lightVectors = CalculateLightVectors_2(vertexPosition, toTangent, LightType);

	// Set vectors in output stream
	output.LightVector = lightVectors.L;
	output.EyeVector = lightVectors.H;

#ifdef DEBUG_SHOW_NORMALS_AS_COLORS
	output.LightVector = mul(vertex.Normal, (float3x3)WorldT);
#endif
#ifdef DEBUG_SHOW_LIGHTVECTORS_AS_COLORS
		output.LightVector = LightPosition - vertexPosition;
#endif

	float3 attenuation = GetAttenuation(vertexPosition, LightType);
	output.AttCoord1.xy = attenuation.xy;
	output.AttCoord2.xy = float2(attenuation.z, 0.0f);
	
	// Just copy the texture coordinate through
	if (useTextures)
		output.TextureCoord = inp.TextureCoord;
	else 
		output.TextureCoord = 0;

	return output;    
}

/**
 * The Blinn-Phong pixel shader function for 2.x shaders
 * @param inp The input stream of the shader
 * @param UseTextures set to true for use of base texture, false otherwise
 * @returns A struct containing the RGBA color
 */
float4
BlinnPhongPixelShader_2_0(BlinnPhongInputPS inp,
													uniform bool useTextures,
													uniform bool normalMapping,
													uniform bool reflectionMapping,
													uniform int LightType) : COLOR0
{
	float4 output;
	float att;
	float3 normal;
	
	// Get the normal from the normal map
	if(normalMapping)
		normal = tex2D(NormalTextureSampler, inp.TextureCoord).xyz * 2.0 - 1.0;
	else
		normal = float3(0,0,1);
	
	// Normalize light and eye vectors
	float3 lightVector	= normalize(inp.LightVector);
	float3 eyeVector		= normalize(inp.EyeVector);

	// Get diffuse and specular components
	LightComponents light = GetDiffuseAndSpecular2(normal, lightVector, eyeVector);
	
	if(LightType == POINT) {
		att = tex2D(AttenuationTextureXYSampler, inp.AttCoord1.xy).w;
		att *= tex2D(AttenuationTextureZSampler, inp.AttCoord2.xy).w;
	}
	if(useTextures) {
		float3 tex = tex2D(BaseTextureSampler, inp.TextureCoord.xy).rgb;
		if(reflectionMapping) {
			float3 ref = tex2D(ReflectionTextureSampler, inp.TextureCoord.xy).rgb;
			output.rgb =  (tex * InvReflectionFactor + ref * ReflectionFactor) * light.Diffuse * DiffuseColor + light.Specular * SpecularColor;
		} else {
			output.rgb = tex * light.Diffuse * DiffuseColor + light.Specular * SpecularColor;
		}
	} else {
		output.rgb = light.Diffuse * DiffuseColor + light.Specular * SpecularColor;
	}


	// Lookup mesh texture and modulate it with ambient
	//if(light.Diffuse > 0.0f) {
	//output.RGBColor.rgb *= (light.Diffuse + light.Specular);
	//} else {
	//	output.RGBColor.rgb = 0;
	//}
	if(LightType == POINT) {
		output.rgb *= att;
	}
	output.a = 1.0;

	//output.rgb = light.Diffuse;
#ifdef DEBUG_SHOW_NORMALS_AS_COLORS
		output.rgb = lightVector;
#endif
#ifdef DEBUG_SHOW_LIGHTVECTORS_AS_COLORS
		output.rgb = lightVector;
#endif

	return output;
}
