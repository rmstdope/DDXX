/**
 *
 * Effects for PostProcessing
 *
*/

#include "EffectConstants.h"

////////////////////////////////////
// Texture Variables
///////////////////////////////////
texture SourceTexture;
texture SourceTexture2;

// 1x1 floating point texture for average luminance
texture LuminanceTexture;

// For glow effect
float Luminance = 0.06;
float Exposure = 0.3f;//0.18;
float WhiteCutoff = 0.1f;
float BloomScale = 1.5f;

// For color Effect
float4 Color = float4(1,1,1,1);

// For blend effect
float2 Offset = float2(0,0);

// For ZoomAdd effect
float ZoomFactor = 1.0f;

// For Wave effect
float4 WaveStrength;
float2 WaveTime;
float WaveScale;

// For Rings effect
float RingsDistance;
float RingsScale;

float2 Time2D;
float Time;

sampler TextureSampler : register(s0) = sampler_state
{
		AddressU	= Clamp;
		AddressV	= Clamp;
		MagFilter	=	Linear;
		MinFilter	=	Linear;
		MipFilter	=	Linear;
};
texture2D MiscTexture;
sampler AlternateTextureSampler = sampler_state
{
		Texture		= <MiscTexture>;
		AddressU	= Clamp;
		AddressV	= Clamp;
		MagFilter	=	Linear;
		MinFilter	=	Linear;
		MipFilter	=	Linear;
};

///////////////////////////////////////////////////
// Filtering kernel variables for common 1.4 kernels
///////////////////////////////////////////////////

static const int cKernelSize14 = 5;

float2 HorizontalKernel14[cKernelSize14]
<
	string ConvertPixelsToTexels = "PreScaledHorizontalKernel14";
>;

float2 VerticalKernel14[cKernelSize14]
<
	string ConvertPixelsToTexels = "PreScaledVerticalKernel14";
>;

float2 PreScaledHorizontalKernel14[cKernelSize14] =
{
	-2.5, 0.0,
	-1.5, 0.0,
	 0.0, 0.0,
	 1.5, 0.0,
	 2.5, 0.0,
};
float2 PreScaledVerticalKernel14[cKernelSize14] =
{
	0.0, -2.5,
	0.0, -1.5,
	0.0,  0.0,
	0.0,  1.5,
	0.0,  2.5,
};

///////////////////////////////////////////////////
// Filtering kernel variables for 1.4 smear kernels
///////////////////////////////////////////////////

static const float SmearWeight14 = 1/4.0f;

///////////////////////////////////////////////////
// Filtering kernel variables for 1.4 blur kernels
///////////////////////////////////////////////////

static const float BlurWeights14[cKernelSize14] = 
{
	0.054488685 * 1.0,
	0.244201342 * 1.0,
	0.402619947 * 1.0,
	0.244201342 * 1.0,
	0.054488685 * 1.0
};

///////////////////////////////////////////////////
// Filtering kernel variables for common 2.0 kernels
///////////////////////////////////////////////////

static const int cKernelSize20 = 13;

float2 HorizontalKernel20[cKernelSize20] <string ConvertPixelsToTexels = "PreScaledHorizontalKernel20";>;

float2 VerticalKernel20[cKernelSize20]
<
	string ConvertPixelsToTexels = "PreScaledVerticalKernel20";
>;

float2 PreScaledHorizontalKernel20[cKernelSize20] =
{
	-6.0, 0.0,
	-5.0, 0.0,
	-4.0, 0.0,
	-3.0, 0.0,
	-2.0, 0.0,
	-1.0, 0.0,
	 0.0, 0.0,
	 1.0, 0.0,
	 2.0, 0.0,
	 3.0, 0.0,
	 4.0, 0.0,
	 5.0, 0.0,
	 6.0, 0.0,
};
float2 PreScaledVerticalKernel20[cKernelSize20] =
{
	0.0, -6.0,
	0.0, -5.0,
	0.0, -4.0,
	0.0, -3.0,
	0.0, -2.0,
	0.0, -1.0,
	0.0,  0.0,
	0.0,  1.0,
	0.0,  2.0,
	0.0,  3.0,
	0.0,  4.0,
	0.0,  5.0,
	0.0,  6.0,
};

///////////////////////////////////////////////////
// Filtering kernel variables for 2.0 smear kernels
///////////////////////////////////////////////////

static const float SmearWeight20 = 1/11.0f;

///////////////////////////////////////////////////
// Filtering kernel variables for 2.0 blur kernels
///////////////////////////////////////////////////

static const float BlurWeights20[cKernelSize20] = 
{
	0.002216,
	0.008764,
	0.026995,
	0.064759,
	0.120985,
	0.176033,
	0.199471,
	0.176033,
	0.120985,
	0.064759,
	0.026995,
	0.008764,
	0.002216,
};

///////////////////////////////////////////////////
// Filtering kernel variables for DoF 2.0 kernels
///////////////////////////////////////////////////
float2 PreScaledDofKernel20[12] = 
{
	{ 1.0f,  0.0f},
	{ 0.5f,  0.8660f},
	{-0.5f,  0.8660f},
	{-1.0f,  0.0f},
	{-0.5f, -0.8660f},
	{ 0.5f, -0.8660f},
	
	{ 1.5f,  0.8660f},
	{ 0.0f,  1.7320f},
	{-1.5f,  0.8660f},
	{-1.5f, -0.8660f},
	{ 0.0f, -1.7320f},
	{ 1.5f, -0.8660f},
};
float2 DofKernel20[12]
<
	string ConvertPixelsToTexels = "PreScaledDofKernel20";
>;

///////////////////////////////////////////////////
// Other stuff
///////////////////////////////////////////////////

float2 TextureDimensions
<
	string ConvertPixelsToTexels = "PreScaledTextureDimensions";
>;

float2 PreScaledTextureDimensions[1] = { 1, 1 };


float2 PreScaledDownSample4xKernel[16] =
{
	{ 1.5,  -1.5 }, { 1.5,  -0.5 }, { 1.5,   0.5 }, { 1.5,   1.5 },
	{ 0.5,  -1.5 }, { 0.5,  -0.5 }, { 0.5,   0.5 }, { 0.5,   1.5 },
	{-0.5,  -1.5 }, {-0.5,  -0.5 }, {-0.5,   0.5 }, {-0.5,   1.5 },
	{-1.5,  -1.5 }, {-1.5,  -0.5 }, {-1.5,   0.5 }, {-1.5,   1.5 },
};
float2 DownSample4xKernel[16]
<
	string ConvertPixelsToTexels = "PreScaledDownSample4xKernel";
>;

struct VertexShaderOutput
{
	float4 Position :	SV_POSITION;
	float4 Color :		COLOR0;
	float2 Tex :		TEXCOORD0;
};

float4
CopyPS(VertexShaderOutput input) : COLOR0
{
	return tex2D(TextureSampler, input.Tex);
}

//-----------------------------------------------------------------------------
// Pixel Shader: Rings
// Desc: Create rings with sin function.
//-----------------------------------------------------------------------------
float4
RingsPS(VertexShaderOutput input) : COLOR0
{
	float2 fromCenter = input.Tex - 0.5f;

	float len = length(fromCenter);
	float2 distortion = sin(len * RingsDistance + WaveTime.x * PIx2);
	distortion *= cos(len * RingsDistance + WaveTime.x * PIx2);

	//distortion = distortion * distortion;
	distortion = distortion * RingsScale;
	distortion *= TextureDimensions;

	return tex2D(TextureSampler, input.Tex + fromCenter * distortion / len);
}


//-----------------------------------------------------------------------------
// Pixel Shader: Wave
// Desc: Waves the texture with cos and sin functions.
//-----------------------------------------------------------------------------
float4
WavePS(VertexShaderOutput input) : COLOR0
{
	float2 posDistortion = float2(input.Tex.x * WaveStrength.x +
								input.Tex.y * WaveStrength.y,
								input.Tex.x * WaveStrength.z +
								input.Tex.y * WaveStrength.w);

	float2 angles = float2(posDistortion.x + WaveTime.x * PIx2,
												 posDistortion.y + WaveTime.y * PIx2);

	float2 distortion = float2(cos(angles.x), sin(angles.y));

	distortion *= WaveScale;
	distortion *= TextureDimensions;

	return tex2D(TextureSampler, input.Tex + distortion);
}


//-----------------------------------------------------------------------------
// Pixel Shader: ZoomAdd
// Desc: Zooms the texture as well as adds it to the original one.
//-----------------------------------------------------------------------------
float4
ZoomAddPS(VertexShaderOutput input) : COLOR0
{
	float4 originalColor = tex2D(TextureSampler, input.Tex);
	float4 zoomColor = tex2D(TextureSampler, input.Tex * ZoomFactor + (1 - ZoomFactor) / 2);

	float4 color = (originalColor + zoomColor * ZoomFactor) / (1 + ZoomFactor);
	return float4(color.rgb, max(originalColor.a, zoomColor.a));
	//return lerp(zoomColor, originalColor, 0.25 / ZoomFactor);
}

//-----------------------------------------------------------------------------
// Pixel Shader: DownSample4x
// Desc: Downsamples the texture to one fourth the size (in each dimension).
//-----------------------------------------------------------------------------
float4
DownSample4xPS(VertexShaderOutput input) : COLOR0
{
	float4 color = 0;

    for (int i = 0; i < 16; i++) {
        color += tex2D(TextureSampler, input.Tex + DownSample4xKernel[i].xy);
    }

	return color / 16;
}

//-----------------------------------------------------------------------------
// Pixel Shader: HSmearPixelShader
// Desc: Smear the image horizontally.
//-----------------------------------------------------------------------------
float4
HSmearPixelShader(VertexShaderOutput input) : COLOR0
{
	float4 color = 0;

	for(int i = 0; i < cKernelSize20; i++) {    
		color += tex2D(TextureSampler, input.Tex + HorizontalKernel20[i].xy ) * SmearWeight20;
	}

	return color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: VSmearPixelShader
// Desc: Smear the image vertically.
//-----------------------------------------------------------------------------
float4
VSmearPixelShader(VertexShaderOutput input) : COLOR0
{
	float4 color = 0;

	for(int i = 0; i < cKernelSize20; i++) {    
		color += tex2D(TextureSampler, input.Tex + VerticalKernel20[i].xy ) * SmearWeight20;
	}

	return color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: HBlurPixelShader
// Desc: Blurs the image horizontally
//-----------------------------------------------------------------------------
float4
HBlurPixelShader(VertexShaderOutput input) : COLOR0
{
	float4 color = 0;

	for(int i = 0; i < cKernelSize20; i++) {    
		color += tex2D(TextureSampler, input.Tex + HorizontalKernel20[i].xy ) * BlurWeights20[i];
	}

	return color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: VBlurPixelShader
// Desc: Blurs the image vertically
//-----------------------------------------------------------------------------
float4
VBlurPixelShader(VertexShaderOutput input) : COLOR0
{
	float4 color = 0;

	for(int i = 0; i < cKernelSize20; i++) {    
		color += tex2D(TextureSampler, input.Tex + VerticalKernel20[i].xy ) * BlurWeights20[i];
	}

	return color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: CopyPixelShader
// Desc: Copies the image
//-----------------------------------------------------------------------------
float4
CopyPixelShader(VertexShaderOutput input) : COLOR0
{
	float4 color = float4(tex2D(TextureSampler, input.Tex.xy).rgb, 1.0);
	return color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: TextureAlphaPixelShader
// Desc: Copies the image and calculates alpha from the red channel of a third texture
//-----------------------------------------------------------------------------
float TextureAlphaFadeDelay;
float TextureAlphaFadeLength;
float4
TextureAlphaPixelShader(VertexShaderOutput input) : COLOR0
{
	float alpha = tex2D(AlternateTextureSampler, input.Tex.xy).r;
	alpha = 1 - clamp((Time - TextureAlphaFadeDelay * alpha) / TextureAlphaFadeLength, 0, 1);
	float4 color = float4(tex2D(TextureSampler, input.Tex.xy).rgb, alpha);
	return color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: BlendPixelShader
// Desc: Copies the image
//-----------------------------------------------------------------------------
float4
BlendPixelShader(VertexShaderOutput input) : COLOR0
{
	// Sample
	float4 tex;
	tex = float4(tex2D(TextureSampler, input.Tex.xy + Offset).rgb, 1.0);
// Modulate
	return tex* Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: MonoPixelShader
// Desc: Copies a monochrome version of the image
//-----------------------------------------------------------------------------
float4
MonoPixelShader(VertexShaderOutput input) : COLOR0
{
	const float3 LuminanceConv = { 0.2125f, 0.7154f, 0.0721f };
	return dot((float3)tex2D(TextureSampler, input.Tex), LuminanceConv);
}

//-----------------------------------------------------------------------------
// Pixel Shader: DoFPixelShader
// Desc: Post effect shader for ps 2.0 that creates a depth of field feel.
//       Alpha channel needs to be filled with focus values [0..1] where
//       0 is in focus and 1 is not.
//-----------------------------------------------------------------------------
float4
DoFPixelShader(VertexShaderOutput input) : COLOR0
{
	const int numSamples = 12;
	float2 kernelArray[numSamples] = DofKernel20;
	float4 original = tex2D(TextureSampler, input.Tex);
	float4 blurred = 0;
	
	for(int i = 0; i < numSamples; i++) {
		// Lookup into the rendertarget based by offsetting the 
		// original UV by KernelArray[i].
		float4 current = tex2D(TextureSampler, input.Tex + kernelArray[i] * 3);

		// Lerp between original rgb and the jitter rgb based on the alpha value
		blurred += lerp(original, current, saturate(original.a * current.a));
	}

	return blurred / numSamples;
	//return original.a;
}

//-----------------------------------------------------------------------------
// Pixel Shader: DoFPixelShaderVertical
// Desc: Post effect shader for ps 2.0 that creates a depth of field feel.
//       Alpha channel needs to be filled with focus values [0..1] where
//       0 is in focus and 1 is not.
//-----------------------------------------------------------------------------
float4
DoFPixelShaderVertical(VertexShaderOutput input) : COLOR0
{
	float4 original = tex2D(TextureSampler, input.Tex);
	float3 blurred = 0;

	for(int i = 0; i < cKernelSize20 - 1; i++) {    
		float4 current = tex2D(TextureSampler, input.Tex + VerticalKernel20[i].xy );// * DofBlurWeights20[i];

		// Lerp between original rgb and the jitter rgb based on the alpha value
		blurred += lerp(original.rgb, current.rgb, saturate(original.a * current.a)) * BlurWeights20[i];
	}

	return float4(blurred, original.a);
}

//-----------------------------------------------------------------------------
// Pixel Shader: DoFPixelShaderHorizontal
// Desc: Post effect shader for ps 2.0 that creates a depth of field feel.
//       Alpha channel needs to be filled with focus values [0..1] where
//       0 is in focus and 1 is not.
//-----------------------------------------------------------------------------
float4
DoFPixelShaderHorizontal(VertexShaderOutput input) : COLOR0
{
	float4 original = tex2D(TextureSampler, input.Tex);
	float3 blurred = 0;

	for(int i = 0; i < cKernelSize20 - 1; i++) {    
		float4 current = tex2D(TextureSampler, input.Tex + HorizontalKernel20[i].xy );// * DofBlurWeights20[i];

		// Lerp between original rgb and the jitter rgb based on the alpha value
		blurred += lerp(original.rgb, current.rgb, saturate(original.a * current.a)) * BlurWeights20[i];
	}

	//return float4(blurred / (cKernelSize20 - 1), 0.0f);
	return float4(blurred, original.a);
}

//-----------------------------------------------------------------------------
// Pixel Shader: TestPixelShader
// Desc: ...
//-----------------------------------------------------------------------------
float4
ColorPixelShader() : COLOR0
{
	//return float4(1, 1, 1, 0);
	return Color;
}

float4
NoisePixelShader(VertexShaderOutput input) : COLOR0
{
	float2 lookup = tex2D(TextureSampler, input.Tex * 2 + Time2D).xy;
	return tex2D(TextureSampler, lookup) * Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: TestPixelShader
// Desc: ...
//-----------------------------------------------------------------------------
float4
TestPixelShader(VertexShaderOutput input) : COLOR0
{
	float alpha = tex2D(TextureSampler, input.Tex).a;
	//alpha = alpha - 0.5f;  
	return alpha;//float4(alpha,0,0,0);//, alpha, alpha, alpha);
}

//-----------------------------------------------------------------------------
// Pixel Shader: BrightenPixelShader
// Desc: Post effect shader for ps 2.0 that filters out the brightest pixels
//-----------------------------------------------------------------------------
float4
BrightenPixelShader(VertexShaderOutput input) : COLOR0
{
	float4 sample = tex2D( TextureSampler, input.Tex );
	float3 ColorOut = sample.rgb;

	// Map to LDR
	ColorOut *= Exposure/ (Luminance + 0.001f);

	// Subtract out dark pixels
	ColorOut *= (1.0f + (ColorOut / (WhiteCutoff * WhiteCutoff)));
	ColorOut -= 5.0f;

	// Clamp to 0
	ColorOut = max(ColorOut, 0.0f);

	// Map the resulting value into the 0 to 1 range. Higher values than
	// 10 will isolate lights from illuminated scene 
	// objects.
	ColorOut /= (10.0f + ColorOut);

	return float4(ColorOut, sample.a);
}

float4
SimpleBrightenPixelShader(VertexShaderOutput input) : COLOR0
{
	float4 sample = tex2D( TextureSampler, input.Tex );
	float3 ColorOut = sample.rgb;

	// Subtract out dark pixels
	ColorOut -= WhiteCutoff;

	// Clamp to 0
	ColorOut = max(ColorOut, 0.0f);

	return float4(ColorOut, sample.a);
}

//-----------------------------------------------------------------------------
// Pixel Shader: HBloomPixelShader
// Desc: Blooms the image horizontally
//-----------------------------------------------------------------------------
float4
HBloomPixelShader(VertexShaderOutput input) : COLOR0
{
	//static const float BloomScale = 1.5f;
	float4 Color = 0;

	for(int i = 0; i < cKernelSize20; i++) {    
		Color += tex2D(TextureSampler, input.Tex + HorizontalKernel20[i].xy) * BlurWeights20[i];
	}

	return Color* BloomScale;
}

//-----------------------------------------------------------------------------
// Pixel Shader: VBloomPixelShader
// Desc: Blooms the image vertically
//-----------------------------------------------------------------------------
float4
VBloomPixelShader(VertexShaderOutput input) : COLOR0
{
	//static const float BloomScale = 1.5f;
	float4 Color = 0;

	for(int i = 0; i < cKernelSize20; i++) {    
		Color += tex2D(TextureSampler, input.Tex + VerticalKernel20[i].xy ) * BlurWeights20[i] * BloomScale;
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: InversePixelShader
// Desc: Post effect shader that takes invers of the pixel color.
//-----------------------------------------------------------------------------
float4
InversePixelShader(VertexShaderOutput input) : COLOR0
{
	float4 ColorOut = tex2D( TextureSampler, input.Tex );

	return 1.0f - ColorOut;
}

//-----------------------------------------------------------------------------
// Pixel Shader: ToneMapPixelShader
// Desc: Post effect shader for ps 2.0 that filter using a tone map filter.
//-----------------------------------------------------------------------------
float4
ToneMapPixelShader(VertexShaderOutput input) : COLOR0
{
	// sample color
	float4 color = tex2D(TextureSampler, input.Tex);

	//	
	color *= Exposure / (Luminance + 0.001f);
	color /= (1.0f + color);

	return color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: FinalHDR_PixelShader
// Desc: Post effect shader for ps 2.0 that constructs final image for HDR.
// For average luminance the 1x1 LuminanceTexture is assumed to contain the average 
// luminance
//-----------------------------------------------------------------------------
float4
FinalHDR_PixelShader(VertexShaderOutput input) : COLOR0
{
	// sample color
	const bool linearBlend = true;
	float4 color = tex2D(TextureSampler, input.Tex);
	float4 bloom;
	
	if(linearBlend) {
		// If the hardware supports linear filtering of a
		// floating point render target that this step can be skipped.
		float xWeight = frac(input.Tex.x / TextureDimensions.x) - 0.5;
		float xDir = xWeight;
		xWeight = abs(xWeight);
		xDir /= xWeight;
		xDir *= TextureDimensions.x;

		float yWeight = frac(input.Tex.y / TextureDimensions.y) - 0.5;
		float yDir = yWeight;
		yWeight = abs(yWeight);
		yDir /= yWeight;
		yDir *= TextureDimensions.y;

		// sample the blur texture for the 4 relevant pixels, weighted accordingly
		bloom =  ((1.0f - xWeight) * (1.0f - yWeight))	* tex2D(TextureSampler, input.Tex);
		bloom += (xWeight * (1.0f - yWeight)) * tex2D(TextureSampler, input.Tex + float2(xDir, 0.0f));
		bloom += (yWeight * (1.0f - xWeight)) * tex2D(TextureSampler, input.Tex + float2(0.0f, yDir));
		bloom += (xWeight * yWeight) * tex2D(TextureSampler, input.Tex + float2(xDir, yDir));

	} else {
		bloom = tex2D(TextureSampler, input.Tex);
	}

	float4 luminance = tex2D(TextureSampler, float2(0,0));

	// Map to LDR
	color *= Exposure / (Luminance.r + 0.001f);
	// Tone map
	color /= (1.0f + color);

	// Add bloom
	color += bloom;

	return color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: HDRLuminanceGreyDS
// Desc: First step of HDR luminance downsampling, includes conversion to greyscale
//-----------------------------------------------------------------------------
float4
HDRLuminanceGreyDSPS(VertexShaderOutput input) : COLOR0
{
	const float3 LuminanceWeights = float3(0.33f, 0.34f, 0.33f);
	
	float4 sample = tex2D( TextureSampler, input.Tex );
	float average = dot(sample.rgb, LuminanceWeights);
	return float4(average, average, average, sample.a);
}

//-----------------------------------------------------------------------------
// Pixel Shader: HDRLuminanceDS
// Desc: Consecutive steps of HDR luminance downsampling
//-----------------------------------------------------------------------------
float4
HDRLuminanceDSPS(VertexShaderOutput input) : COLOR0
{
	float4 sample = tex2D( TextureSampler, input.Tex );

	return sample;
}

/***********************************************************************************
 * ------------------
 * Techniques
 * ------------------
***********************************************************************************/

technique Rings
{
	pass p0 //<float Scale = 1.0f;>
	{
		PixelShader = compile PS_SHADERMODEL RingsPS();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique Wave
{
	pass p0 //<float Scale = 1.0f;>
	{
		PixelShader = compile PS_SHADERMODEL WavePS();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique ZoomAdd
{
	pass p0 //<float Scale = 1.0f;>
	{
		PixelShader = compile PS_SHADERMODEL ZoomAddPS();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}


technique DownSample4x
{
	pass p0 //<float Scale = 0.25f;>
	{
		PixelShader = compile PS_SHADERMODEL DownSample4xPS();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique UpSample4x
{
	pass p0 //<float Scale = 4.0f;>
	{
		PixelShader = compile PS_SHADERMODEL CopyPS();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique ToneMap
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL ToneMapPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique FinalHDR
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL FinalHDR_PixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique InverseColor
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL InversePixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique HorizontalBloom
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL HBloomPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique VerticalBloom
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL VBloomPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique Brighten
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL SimpleBrightenPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique AdvancedBrighten
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL BrightenPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique HDRLuminanceGreyDS
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL HDRLuminanceGreyDSPS();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique HDRLuminanceDS
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL HDRLuminanceDSPS();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique Monochrome
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL MonoPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique HorizontalSmear
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL HSmearPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique VerticalSmear
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL VSmearPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique HorizontalBlur
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL HBlurPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique VerticalBlur
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL VBlurPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique Copy
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL CopyPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique TextureAlpha
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL TextureAlphaPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique Blend
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL BlendPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique DepthOfFieldVertical
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL DoFPixelShaderVertical();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique DepthOfFieldHorizontal
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL DoFPixelShaderHorizontal();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique DepthOfField
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL DoFPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique SolidColor
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL ColorPixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}

technique Noise
{
	pass BasePass //<float Scale = 1.0f;>
	{
		PixelShader				= compile PS_SHADERMODEL NoisePixelShader();
		ZEnable						= false;
		StencilEnable			= false;
		CullMode = None;
	}
}



