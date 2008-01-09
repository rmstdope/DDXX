#include "CommonVariables.hlsl"

sampler2D NormalMapSampler = sampler_state
{
	Texture = <NormalMap>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU	= Mirror;
	AddressV	= Mirror;
};

sampler2D DiffuseTextureSampler = sampler_state
{
	Texture = <Texture>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU	= Mirror;
	AddressV	= Mirror;
};

struct VS_INPUT
{
	float4 position	: POSITION0;
	float2 texCoord	: TEXCOORD0;
	float3 normal		: NORMAL0;    
	float3 binormal	: BINORMAL0;
	float3 tangent	: TANGENT0;
};

// output from the vertex shader, and input to the pixel shader.
// lightDirection and viewDirection are in world space.
// NOTE: even though the tangentToWorld matrix is only marked 
// with TEXCOORD3, it will actually take TEXCOORD3, 4, and 5.
struct VS_OUTPUT
{
	float4 position					: POSITION0;
	float2 texCoord					: TEXCOORD0;
	float3 lightDirection		: TEXCOORD1;
	float3 viewDirection		: TEXCOORD2;
	float3x3 tangentToWorld	: TEXCOORD3;
};

VS_OUTPUT VertexShader( VS_INPUT input )
{
	VS_OUTPUT output;
    
	// transform the position into projection space
	float4 worldSpacePos = mul(input.position, World[0]);
	output.position = mul(worldSpacePos, View);
	output.position = mul(output.position, Projection);
    
	// calculate the light direction ( from the surface to the light ), which is not
	// normalized and is in world space
	output.lightDirection = LightPositions[0] - worldSpacePos;
        
	// similarly, calculate the view direction, from the eye to the surface.  not
	// normalized, in world space.
	float3 eyePosition = mul(-View._m30_m31_m32, transpose(View));
	output.viewDirection = worldSpacePos - eyePosition;    
    
	// calculate tangent space to world space matrix using the world space tangent,
	// binormal, and normal as basis vectors.  the pixel shader will normalize these
	// in case the world matrix has scaling.
	output.tangentToWorld[0] = mul(input.tangent, World[0]);
	output.tangentToWorld[1] = mul(input.normal, World[0]);
	output.tangentToWorld[2] = mul(input.binormal, World[0]);
    
	// pass the texture coordinate through without additional processing
	output.texCoord = input.texCoord;
    
	return output;
}

float4 PixelShader( VS_OUTPUT input ) : COLOR0
{
	// look up the normal from the normal map, and transform from tangent space
	// into world space using the matrix created above.  normalize the result
	// in case the matrix contains scaling.
	//float3 normalFromMap = float3(0, 0, 1);
	float3 normalFromMap = tex2D(NormalMapSampler, input.texCoord);
	normalFromMap = (normalFromMap * 2) - 1;    
	normalFromMap = mul(normalFromMap, input.tangentToWorld);
	normalFromMap = normalize(normalFromMap);
    
	// clean up our inputs a bit
	input.viewDirection = normalize(input.viewDirection);
	input.lightDirection = normalize(input.lightDirection);    
    
	// use the normal we looked up to do phong diffuse style lighting.    
	float nDotL = max(dot(normalFromMap, input.lightDirection), 0);
	float3 diffuse = LightColors[0] * nDotL * DiffuseColor;
    
	// use phong to calculate specular highlights: reflect the incoming light
	// vector off the normal, and use a dot product to see how "similar"
	// the reflected vector is to the view vector.    
	float3 reflectedLight = reflect(input.lightDirection, normalFromMap);
	float rDotV = max(dot(reflectedLight, input.viewDirection), 0);
	float3 specular = Shininess * SpecularColor * LightColors[0] * pow(rDotV, SpecularPower);
    
	float3 diffuseTexture = tex2D(DiffuseTextureSampler, input.texCoord);
    
	// return the combined result.
	//float3 color = specular;
	float3 color = (diffuse + AmbientLightColor * AmbientColor) * diffuseTexture + specular;
	return float4(color, 1);
	//return float4(nDotL, nDotL, nDotL, nDotL);
}

Technique NormalMapping
{
	Pass Go
	{
		VertexShader = compile vs_1_1 VertexShader();
		PixelShader = compile ps_2_0 PixelShader();
		ZEnable = true;
		ZWriteEnable = true;
		FillMode = Solid;
	}
}
