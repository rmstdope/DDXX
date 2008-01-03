#ifndef COMMON_FUNCTIONS_FX
#define COMMON_FUNCTIONS_FX

#include <DataTypes.hlsl>
#include <SharedVariables.hlsl>

/**
 * Return the base vectors of the tangent space for this vertex
 * @param normal The normal of the vertex
 * @param tangent The tangent of the vertex
 * @param worldTransposed The transposed World matrix
 * @returns the base vectors in a 3 by 3 matrix
 */
float3x3
GetTangentSpaceBase(float3 normal, 
										float3 tangent,
										float3x3 worldTransposed)
{
	float3x3 base;
	
	// Transform tangent to world space
	base[0] = normalize(mul(tangent, worldTransposed));
	// Transform normal to world space
	base[2] = normalize(mul(normal,	worldTransposed));
	// Create binormal vector
	base[1] = cross(base[0], base[2]);
	
	//return base;
	return transpose(base);
}

/**
 * Animates a vertex according to the blend indices and blend weights
 * @param Position The original position of the vertex
 * @param Normal The normal of the vertex
 * @param Tangent The tangent of the vertex
 * @param BlendIndices Index 0 and 1 of the bones affecting the vertex
 * @param BlendWeight The weight of index 0. Index 1 has weight 1.0 - BlendWeight
 * @param Animated True if the vertex shall be animated. False otherwise
 * @returns the position, normal and tangent of the animated vertex
 */
AnimatedVertex_PNT
AnimateVertex(float4	position,
							float3	normal,
							float3	tangent,
							int4		blendIndices,
							float3	blendWeights,
							uniform int numWeights)
{
	AnimatedVertex_PNT output;

	if(numWeights > 0) {
		// Animation the vertex based on time and the vertex's object space position
		float lastWeight = 0.0f;
		float4x4 weightedMatrix = 0.0f;
		for(int b = 0; b < numWeights - 1; b++) {
			lastWeight = lastWeight + blendWeights[b];
			weightedMatrix += AnimationMatrices[blendIndices[b]] * blendWeights[b];
		}
    lastWeight = 1.0f - lastWeight; 
		weightedMatrix += AnimationMatrices[blendIndices[numWeights - 1]] * lastWeight;
		output.Position = mul(position, weightedMatrix);
		output.Normal		= normalize(mul(normal, (float3x3)weightedMatrix));
		output.Tangent	= normalize(mul(tangent, (float3x3)weightedMatrix));
	} else {
		output.Position	= position;
		output.Normal		= normal;
		output.Tangent	= tangent;
	}
	
	return output;
}

/**
 * Animates a vertex according to the blend indices and blend weights
 * @param Position The original position of the vertex
 * @param Normal The normal of the vertex
 * @param BlendIndices Index 0 and 1 of the bones affecting the vertex
 * @param BlendWeight The weight of index 0. Index 1 has weight 1.0 - BlendWeight
 * @param Animated True if the vertex shall be animated. False otherwise
 * @returns the position and normal of the animated vertex
 */
AnimatedVertex_PN
AnimateVertex(float4	position,
							float3	normal,
							int4		blendIndices,
							float3	blendWeights,
							uniform int numWeights)
{
	AnimatedVertex_PN output;

	if(numWeights) {
		// Animation the vertex based on time and the vertex's object space position
		float lastWeight = 0.0f;
		float4x4 weightedMatrix = 0.0f;
		for(int b = 0; b < numWeights - 1; b++) {
			lastWeight = lastWeight + blendWeights[b];
			weightedMatrix += AnimationMatrices[blendIndices[b]] * blendWeights[b];
		}
    lastWeight = 1.0f - lastWeight; 
		weightedMatrix += AnimationMatrices[blendIndices[numWeights - 1]] * lastWeight;
		output.Position = mul(position, weightedMatrix);
		output.Normal		= normalize(mul(normal, (float3x3)weightedMatrix));
	} else {
		output.Position	= position;
		output.Normal		= normal;
	}
	
	return output;
}

/**
 * Animates a vertex according to the blend indices and blend weights
 * @param Position The original position of the vertex
 * @param BlendIndices Index 0 and 1 of the bones affecting the vertex
 * @param BlendWeight The weight of index 0. Index 1 has weight 1.0 - BlendWeight
 * @param Animated True if the vertex shall be animated. False otherwise
 * @returns the position, normal and tangent of the animated vertex
 */
float4
AnimateVertex(float4	position,
							int4		blendIndices,
							float3	blendWeights,
							uniform int numWeights)
{
	float4 output;

	if(numWeights) {
		// Animation the vertex based on time and the vertex's object space position
		float lastWeight = 0.0f;
		float4x4 weightedMatrix = 0.0f;
		for(int b = 0; b < numWeights - 1; b++) {
			lastWeight = lastWeight + blendWeights[b];
			weightedMatrix += AnimationMatrices[blendIndices[b]] * blendWeights[b];
		}
    lastWeight = 1.0f - lastWeight; 
		weightedMatrix += AnimationMatrices[blendIndices[numWeights - 1]] * lastWeight;
		output = mul(position, weightedMatrix);
	} else {
		output = position;
	}
	
	return output;
}

#endif
