/**
 * This is a dummy effect used only for defining the shared variables used in other effects.
 */

#include "CommonVariables.hlsl"
#include "SharedVariables.hlsl"

float4
DefaultPixelShader(PixelInput input) : COLOR
{
	return 0.0f
}

technique T0
{
	pass P0
	{
		//VertexShader = NULL;
		PixelShader  = compile PS_SHADERMODEL DefaultPixelShader();
	}
}
