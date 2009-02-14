#include "../../../Effects/Content/effects/CommonVariables.hlsl"

struct INPUT
{
	float4 position : POSITION0;
};

INPUT
DefaultVertexShader(INPUT input)
{
	INPUT output;
	
	output.position = mul(input.position, World[0]);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);
	
	return output;
}

float4
DefaultPixelShader(INPUT input) : COLOR
{
	return float4(AmbientColor, Transparency);
}

Technique Default
{
	Pass BasePass
	{
		VertexShader = compile vs_2_0 DefaultVertexShader();
		PixelShader = compile ps_2_0 DefaultPixelShader();
		AlphaBlendEnable = true;
		BlendOp = Add;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;
		ZEnable = false;
		ZWriteEnable = false;
	}
}