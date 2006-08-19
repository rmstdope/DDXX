/** 
 * This file contains the following shaders:
 * - Blinn-Phong per-pixel lightning for 1.1 shaders or above
 * - Blinn-Phong per-pixel lightning for 1.4 shaders or above
 * - Blinn-Phong per-pixel lightning for 2.x shaders or above
 */

#include <CommonFunctions.hlsl>
 
//#define DEBUG_SHOW_NORMALS_AS_COLORS
//#define DEBUG_SHOW_LIGHTVECTORS_AS_COLORS

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
 * Calculate normalized light vector and half vector in tangent space
 * @param Position The position of the vertex in world space
 * @param ToTangent The base vector of the tangent space.
 * @param LightType The type of light
 * @returns the normalized light and half vectors in tangent space
 */
LightVectors
CalculateLightVectors(float4 Position,
											float3x3 ToTangent,
											uniform int LightType)
{
	LightVectors output;
	
	if(LightType == DIRECTIONAL) {
		output.L = LightDirection;
	} else if(LightType == POINT) {
		output.L = LightPosition - Position;
	}
	
	// Normalize Light and View vectors
	output.L = output.L;
	float3 V = normalize(EyePosition.xyz - Position.xyz);

	// Calculate and normalize Half vector
	output.H = normalize(V + normalize(output.L));

	output.L = mul(output.L, ToTangent);
	output.H = mul(output.H, ToTangent);

	return output;	
}

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
 * Calculate coordinates of attenuation textures based on x,y, and z distance
 * to light and range of light.
 * @param Position The position of the vertex in world space
 * @param LightType The type of light
 * @returns the distance to the light in x, y, and z where [-LightRange..LightRange] is
 * mapped to [0..1] (Negative values and >1 will be clamped to 0 and 1 respectively.
 */
float3
GetAttenuation(float4 Position, 
							 uniform int LightType)
{
	float3 output;
	
	if(LightType == DIRECTIONAL) {
		output = float3(0.5f, 0.5f, 0.5f);
	} else if(LightType == POINT) {
		float3 lightDir = LightPosition - Position;
		// Make [-LightRange..LightRange] become [0..1]
		output = (lightDir / LightRange) * 0.5f + 0.5f;
	}

	return output;	
}

/**
 * Calculate diffuse and specular terms according to the
 * Phong lighting model.
 * @param normalVector The normal (unitvector)
 * @param lightVector A vector to the light (unitvector)
 * @param eyeVector A vector to the eye (unitvector)
 * @param glossiness The glossiness of the specular component
 * @returns Diffuse and Specular components
 */
LightComponents
GetDiffuseAndSpecular2(float3 normalVector,
											 float3 lightVector,
											 float3 eyeVector)
{							
	LightComponents light;
	float halfVector;
	
  // Get diffuse component
	light.Diffuse = saturate(dot(normalVector, lightVector));
	// Get specular component
	float specular = saturate(dot(normalVector, eyeVector));
			
  // Get diffuse component
	//light.Diffuse = saturate(dot(normalVector, lightVector));
	// Get half vector
	//halfVector = normalize(eyeVector + lightVector);
	//float specular = saturate(dot(normalVector, halfVector));
	light.Specular = pow(specular, Shininess);
	
	return light;
}

/**
 * Calculate diffuse and specular terms according to the
 * Phong lighting model.
 * @param normalVector The normal (unitvector)
 * @param lightVector A vector to the light (unitvector)
 * @param eyeVector A vector to the eye (unitvector)
 * @param glossiness The glossiness of the specular component
 * @returns Diffuse and Specular components
 */
LightComponents
GetDiffuseAndSpecular(float3 normalVector,
											float3 lightVector,
											float3 halfVector,
											uniform int glossiness)
{							
	LightComponents light;
			
  // Get diffuse component
	light.Diffuse = saturate(dot(normalVector, lightVector));
	// Get specular component
	float specular = saturate(dot(normalVector, halfVector));
	
	// Use different techniques to make pow(N.H, n), where n is the specular glossiness
	if(glossiness <= 3 || glossiness == 4) {
		light.Specular = pow(specular, glossiness);
	}
	if(glossiness == 16) {
		light.Specular = pow(saturate(4 * (pow(specular,2) - 0.75)),2);
	}
	if(glossiness == 32 || glossiness == 64 || glossiness == 128) {
		light.Specular = pow(saturate(4 * (pow(specular,2) - 0.75)),2);
		light.Specular = pow(light.Specular, glossiness / 16);
		//light.Specular = pow(specular, glossiness);
	}
	return light;
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


/**
 * The Blinn-Phong vertex shader function for 1.4 shaders
 * @param inp The input stream of the shader
 * @param LightType set to DIRECTIONAL or POINT depending on light type
 * @returns A struct containing the input stream for the pixel shader
 */
VertexOutputStream_1_4
BlinnPhongVertexShader_1_4(BlinnPhongInputVS inp,
													 uniform int numWeights,
													 uniform int LightType)
{
	VertexOutputStream_1_4 output;

	// Calculate new position, normal and tangent depending on animation
	AnimatedVertex_PNT vertex = AnimateVertex(inp.Position, inp.Normal, inp.Tangent, inp.BlendIndices, inp.BlendWeights, numWeights);	
	
	// Transform the position from object space to world space
	float4 vertexPosition = mul(vertex.Position, WorldT);

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(vertex.Position, WorldViewProjectionT);
	
	// Generate tangent space base vectors
	float3x3 toTangent = GetTangentSpaceBase(vertex.Normal, vertex.Tangent, WorldT);

	// Calculate light (and eye) vectors im tangent space
	LightVectors lightVectors = CalculateLightVectors(vertexPosition, toTangent, LightType);

	// Set vectors in output stream
	output.LightVector = lightVectors.L;
	output.HalfVector = lightVectors.H;

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
	output.TextureCoord = inp.TextureCoord; 
    
	return output;    
}

/**
 * The Blinn-Phong pixel shader function for 1.4 shaders
 * @param inp The input stream of the shader
 * @param UseTextures set to true for use of base texture, false otherwise
 * @returns A struct containing the RGBA color
 */
float4
BlinnPhongPixelShader_1_4(VertexOutputStream_1_4 inp,
													uniform bool useTextures,
													uniform bool normalMapping,
													uniform bool reflectionMapping,
													uniform int glossiness,
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
	
  // Sample light vector and eye vector from normalization texture 
	float3 lightVector	= Normalize(inp.LightVector);
	float3 halfVector		= Normalize(inp.HalfVector);

	// Get diffuse and specular components
	LightComponents light = GetDiffuseAndSpecular(normal, lightVector, halfVector, glossiness);
	
	if(LightType == POINT) {
		att = tex2D(AttenuationTextureXYSampler, inp.AttCoord1.xy).w;
		att *= tex2D(AttenuationTextureZSampler, inp.AttCoord2.xy).w;
	}
	if(useTextures) {
		float4 tex = tex2D(BaseTextureSampler, inp.TextureCoord.xy);
		if(reflectionMapping) {
			float3 ref = tex2D(ReflectionTextureSampler, inp.TextureCoord.xy).rgb;
			output.rgb = (tex.rgb * tex.a + ref * (tex.a - 1)) * light.Diffuse * DiffuseColor + light.Specular * SpecularColor;
			//output.rgb = (tex * InvReflectionFactor + ref * ReflectionFactor) * light.Diffuse * DiffuseColor + light.Specular * SpecularColor;
		} else {
			output.rgb = tex * light.Diffuse * DiffuseColor + light.Specular * SpecularColor;
		}
	} else {
		output.rgb = light.Diffuse * DiffuseColor + light.Specular * SpecularColor;
	}


	// Lookup mesh texture and modulate it with ambient
	//if(light.Diffuse > 0.0f) {
	//output.rgb *= (light.Diffuse + light.Specular);
	//} else {
	//	output.rgb = 0;
	//}
	if(LightType == POINT) {
		output.rgb *= att;
	}
	output.a = 1.0;

#ifdef DEBUG_SHOW_NORMALS_AS_COLORS
		output.rgb = lightVector;
#endif
#ifdef DEBUG_SHOW_LIGHTVECTORS_AS_COLORS
		output.rgb = lightVector;
#endif
	//output.rgb = halfVector;

	return output;
}


/**
 * The Blinn-Phong vertex shader function for 1.1 shaders (pass2)
 * @param inp The input stream of the shader
 * @param LightType set to DIRECTIONAL or POINT depending on light type
 * @returns A struct containing the input stream for the pixel shader
 */
VertexOutputStream_1_1_Pass2
BlinnPhongVertexShader_1_1_Pass2(BlinnPhongInputVS inp,
																 uniform int numWeights,
																 uniform int LightType)
{
	VertexOutputStream_1_1_Pass2 output;

	// Calculate new position, normal and tangent depending on animation
	AnimatedVertex_PNT vertex = AnimateVertex(inp.Position, inp.Normal, inp.Tangent, inp.BlendIndices, inp.BlendWeights, numWeights);	
	
	// Transform the position from object space to world space
	float4 vertexPosition = mul(vertex.Position, WorldT);

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(vertex.Position, WorldViewProjectionT);
	
	// Generate tangent space base vectors
	float3x3 toTangent = GetTangentSpaceBase(vertex.Normal, vertex.Tangent, WorldT);

	// Calculate light (and eye) vectors im tangent space
	LightVectors lightVectors = CalculateLightVectors(vertexPosition, toTangent, LightType);

	// Set vectors in output stream
	output.LightVector = lightVectors.L;
	output.HalfVector = lightVectors.H;
	
	// Just copy the texture coordinate through
	output.TextureCoord = inp.TextureCoord; 
	output.NormalCoord = inp.TextureCoord; 
    
	return output;    
}

/**
 * The Blinn-Phong vertex shader function for 1.1 shaders (pass1)
 * @param inp The input stream of the shader
 * @param LightType set to DIRECTIONAL or POINT depending on light type
 * @returns A struct containing the input stream for the pixel shader
 */
VertexOutputStream_1_1_Pass1
BlinnPhongVertexShader_1_1_Pass1(BlinnPhongInputVS inp,
																 uniform int numWeights)
{
	VertexOutputStream_1_1_Pass1 output;

	// Calculate new position, normal and tangent depending on animation
	AnimatedVertex_PNT vertex = AnimateVertex(inp.Position, inp.Normal, inp.Tangent, inp.BlendIndices, inp.BlendWeights, numWeights);	
	
	// Transform the position from object space to world space
	float4 vertexPosition = mul(vertex.Position, WorldT);

	// Transform the position from object space to homogeneous projection space
	output.Position = mul(vertex.Position, WorldViewProjectionT);
		
	float3 attenuation = GetAttenuation(vertexPosition, POINT);
	output.AttCoord1.xy = attenuation.xy;
	output.AttCoord2.xy = float2(attenuation.z, 0.0f);
	
	return output;    
}

/**
 * The Blinn-Phong pixel shader function for 1.1 shaders (pass2)
 * @param inp The input stream of the shader
 * @param UseTextures set to true for use of base texture, false otherwise
 * @returns A struct containing the RGBA color
 */
float4
BlinnPhongPixelShader_1_1_Pass2(VertexOutputStream_1_1_Pass2 inp,
																uniform bool useTextures,
																uniform bool normalMapping,
																uniform bool reflectionMapping,
																uniform int glossiness,
																uniform int LightType) : COLOR0
{
	float4 output;
	float3 normal;
	
	// Get the normal from the normal map
	if(normalMapping)
		normal = tex2D(NormalTextureSampler, inp.NormalCoord).xyz * 2.0 - 1.0;
	else
		normal = float3(0,0,1);
	
  // Sample light vector and eye vector from normalization texture 
	float3 lightVector	= Normalize(inp.LightVector);
	float3 halfVector		= Normalize(inp.HalfVector);
	
	// Get diffuse and specular components
	LightComponents light = GetDiffuseAndSpecular(normal, lightVector, halfVector, glossiness);

	if(useTextures) {
		output.rgb = tex2D(BaseTextureSampler, inp.TextureCoord.xy).rgb * light.Diffuse * DiffuseColor + light.Specular * SpecularColor;
	} else {
		output.rgb = light.Diffuse * DiffuseColor + light.Specular * SpecularColor;
	}

	output.a = 1.0;

	return output;
}

/**
 * The Blinn-Phong pixel shader function for 1.1 shaders (pass1)
 * @param inp The input stream of the shader
 * @param UseTextures set to true for use of base texture, false otherwise
 * @returns A struct containing the RGBA color
 */
float4
BlinnPhongPixelShader_1_1_Pass1(VertexOutputStream_1_1_Pass1 inp) : COLOR0
{
	float att;
	att = tex2D(AttenuationTextureXYSampler, inp.AttCoord1.xy).w;
	att *= tex2D(AttenuationTextureZSampler, inp.AttCoord2.xy).w;
	
	return float4(1, 1, 1, att);
}

/*technique BlinnPhongPoint_1_4
{
	pass BasePass
	{
		VertexShader			= compile vs_1_1 BlinnPhongVertexShader_1_4(0, POINT);
		PixelShader				= compile ps_1_4 BlinnPhongPixelShader_1_4(true, false, false, 16, POINT);
		AlphaBlendEnable	= true;
		BlendOp						= <BlendOperation>;
		SrcBlend					= <SourceBlend>;
		DestBlend					= <DestBlend>;
		BlendFactor				= <BlendFactor>;
		FillMode					= <FillMode>;
		ZFunc							= Equal;
		StencilEnable			= true;
		StencilFunc				= Equal;
		StencilPass				= Incr;
	}
}*/

technique BlinnPhongPoint_2_0
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 BlinnPhongVertexShader_2_0(true, 0, POINT);
		PixelShader				= compile ps_2_0 BlinnPhongPixelShader_2_0(true, false, false, POINT);
		AlphaBlendEnable	= true;
		BlendOp						= <BlendOperation>;
		SrcBlend					= <SourceBlend>;
		DestBlend					= <DestBlend>;
		BlendFactor				= <BlendFactor>;
		FillMode					= <FillMode>;
		ZFunc							= Equal;
		StencilEnable			= true;
		StencilFunc				= Equal;
		StencilPass				= Incr;
	}
}

technique BlinnPhongPointNoTex_2_0
{
	pass BasePass
	{
		VertexShader			= compile vs_2_0 BlinnPhongVertexShader_2_0(false, 0, POINT);
		PixelShader				= compile ps_2_0 BlinnPhongPixelShader_2_0(false, false, false, POINT);
		AlphaBlendEnable	= true;
		BlendOp						= <BlendOperation>;
		SrcBlend					= <SourceBlend>;
		DestBlend					= <DestBlend>;
		BlendFactor				= <BlendFactor>;
		FillMode					= <FillMode>;
		ZFunc							= Equal;
		StencilEnable			= true;
		StencilFunc				= Equal;
		StencilPass				= Incr;
	}
}


