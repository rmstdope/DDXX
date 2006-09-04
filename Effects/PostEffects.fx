/**
 *
 * Effects for PostProcessing
 *
*/

#include "EffectConstants.h"

#define INCLUDE_PS_1_4
#define INCLUDE_PS_2_0

////////////////////////////////////
// Texture Variables
///////////////////////////////////

//DWORD SourceBlend = ONE;
//DWORD DestBlend = ZERO;
//DWORD BlendFactor = 0xFFFFFFFF;

texture SourceTexture;
texture SourceTexture2;

// 1x1 floating point texture for average luminance
texture LuminanceTexture;

float Luminance = 0.06;
float Exposure = 0.18;
float WhiteCutoff = 0.9f;
float BloomScale = 1.0f;//5f;
float4 Color = float4(1,1,1,1);
float2 Offset = float2(0,0);

sampler2D LinearTextureSampler = sampler_state
{
	texture = <SourceTexture>;
	MinFilter = Point;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D LuminanceTextureSampler = sampler_state
{
	texture = <LuminanceTexture>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D LinearTextureSampler2 = sampler_state
{
	texture = <SourceTexture2>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D PointTextureSampler = sampler_state
{
	texture = <SourceTexture>;
	AddressU = Clamp;
	AddressV = Clamp;
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = Point;
};

sampler2D PointTextureSampler2 = sampler_state
{
	texture = <SourceTexture2>;
	AddressU = Wrap;
	AddressV = Wrap;
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = Point;
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

float2 HorizontalKernel20[cKernelSize20]
<
	string ConvertPixelsToTexels = "PreScaledHorizontalKernel20";
>;

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
float2 DofKernel20[12] = 
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

///////////////////////////////////////////////////
// Other stuff
///////////////////////////////////////////////////

float2 TextureDimensions
<
	string ConvertPixelsToTexels = "PreScaledTextureDimensions";
>;

float2 PreScaledTextureDimensions = { 1, 1 };


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

float4
Copy(float2 Tex : TEXCOORD0) : COLOR0
{
	return tex2D(LinearTextureSampler, Tex);
}

//-----------------------------------------------------------------------------
// Pixel Shader: DownSample4x
// Desc: Downsamples the texture to one fourth the size (in each dimension).
//-----------------------------------------------------------------------------
float4
DownSample4x(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 Color = 0;

	for (int i = 0; i < 16; i++) {
		Color += tex2D(LinearTextureSampler, Tex + DownSample4xKernel[i].xy);
	}

	return Color / 16;
}

//-----------------------------------------------------------------------------
// Pixel Shader: HSmearPixelShader14
// Desc: Smear the image horizontally.
//-----------------------------------------------------------------------------
float4
HSmearPixelShader14(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 Color = 0;

	for(int i = 0; i < cKernelSize14; i++) {    
		if(i == 2) {
			Color += tex2D(PointTextureSampler,  Tex + HorizontalKernel14[i].xy ) * SmearWeight14;
		} else {
			Color += tex2D(LinearTextureSampler,  Tex + HorizontalKernel14[i].xy ) * SmearWeight14;
		}
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: HSmearPixelShader20
// Desc: Smear the image horizontally.
//-----------------------------------------------------------------------------
float4
HSmearPixelShader20(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 Color = 0;

	for(int i = 0; i < cKernelSize14; i++) {    
		Color += tex2D(PointTextureSampler,  Tex + HorizontalKernel20[i].xy ) * SmearWeight20;
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: VSmearPixelShader14
// Desc: Smear the image vertically.
//-----------------------------------------------------------------------------
float4
VSmearPixelShader14(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 Color = 0;

	for(int i = 0; i < cKernelSize14; i++) {    
		if(i == 2) {
			Color += tex2D(PointTextureSampler,  Tex + VerticalKernel14[i].xy ) * SmearWeight14;
		} else {
			Color += tex2D(LinearTextureSampler,  Tex + VerticalKernel14[i].xy ) * SmearWeight14;
		}
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: VSmearPixelShader20
// Desc: Smear the image vertically.
//-----------------------------------------------------------------------------
float4
VSmearPixelShader20(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 Color = 0;

	for(int i = 0; i < cKernelSize20; i++) {    
		Color += tex2D(PointTextureSampler,  Tex + VerticalKernel20[i].xy ) * SmearWeight20;
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: HBlurPixelShader14
// Desc: Blurs the image horizontally
//-----------------------------------------------------------------------------
float4
HBlurPixelShader14(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 Color = 0;

	for(int i = 0; i < cKernelSize14; i++) {    
		if(i == 2) {
			Color += tex2D(PointTextureSampler,  Tex + HorizontalKernel14[i].xy ) * BlurWeights14[i];
		} else {
			Color += tex2D(LinearTextureSampler,  Tex + HorizontalKernel14[i].xy ) * BlurWeights14[i];
		}
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: HBlurPixelShader20
// Desc: Blurs the image horizontally
//-----------------------------------------------------------------------------
float4
HBlurPixelShader20(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 Color = 0;

	for(int i = 0; i < cKernelSize20; i++) {    
		Color += tex2D(PointTextureSampler,  Tex + HorizontalKernel20[i].xy ) * BlurWeights20[i];
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: VBlurPixelShader14
// Desc: Blurs the image vertically
//-----------------------------------------------------------------------------
float4
VBlurPixelShader14(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 Color = 0;

	for(int i = 0; i < cKernelSize14; i++) {    
		if(i == 2) {
			Color += tex2D(PointTextureSampler,  Tex + VerticalKernel14[i].xy ) * BlurWeights14[i];
		} else {
			Color += tex2D(LinearTextureSampler,  Tex + VerticalKernel14[i].xy ) * BlurWeights14[i];
		}
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: VBlurPixelShader20
// Desc: Blurs the image vertically
//-----------------------------------------------------------------------------
float4
VBlurPixelShader20(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 Color = 0;

	for(int i = 0; i < cKernelSize20; i++) {    
		Color += tex2D(PointTextureSampler,  Tex + VerticalKernel20[i].xy ) * BlurWeights20[i];
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: CopyPixelShader
// Desc: Copies the image
//-----------------------------------------------------------------------------
float4
CopyPixelShader(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 color = float4(tex2D(LinearTextureSampler, Tex.xy).rgb, 1.0);
	return color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: BlendPixelShader
// Desc: Copies the image
//-----------------------------------------------------------------------------
float4
BlendPixelShader(float2 Tex : TEXCOORD0,
								 uniform bool usePoint) : COLOR0
{
	// Sample
	float4 tex;
	if(usePoint)
		tex = float4(tex2D(PointTextureSampler, Tex.xy + Offset).rgb, 1.0);
	else
		tex = float4(tex2D(LinearTextureSampler, Tex.xy + Offset).rgb, 1.0);
	// Modulate
	return tex * Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: MonoPixelShader
// Desc: Copies a monochrome version of the image
//-----------------------------------------------------------------------------
float4
MonoPixelShader(float2 Tex : TEXCOORD0) : COLOR0
{
	const float3 LuminanceConv = { 0.2125f, 0.7154f, 0.0721f };
	return dot((float3)tex2D(LinearTextureSampler, Tex), LuminanceConv);
}

//-----------------------------------------------------------------------------
// Pixel Shader: DoFPixelShader20
// Desc: Post effect shader for ps 2.0 that creates a depth of field feel.
//       Alpha channel needs to be filled with focus values [0.5..1] where
//       0.5 is in focus and 1 is not.
//-----------------------------------------------------------------------------
float4
DoFPixelShader20(float2 Tex					: TEXCOORD0,
								 uniform float2 kernelArray[12],
								 uniform int numSamples) : COLOR0
{
	float4 original = tex2D(LinearTextureSampler, Tex);
	float3 blurred = 0;
	
	for(int i = 0; i < numSamples; i++) {
		// Lookup into the rendertarget based by offsetting the 
		// original UV by KernelArray[i].
		float4 current = tex2D(LinearTextureSampler, Tex + kernelArray[i]);

		// Lerp between original rgb and the jitter rgb based on the alpha value
		blurred += lerp(original.rgb, current.rgb, saturate(original.a * current.a));
	}

	return float4(blurred / numSamples, 1.0f);
}

//-----------------------------------------------------------------------------
// Pixel Shader: DoFPixelShader14
// Desc: Post effect shader for ps 1.4 that creates a depth of field feel.
//       Alpha channel needs to be filled with focus values [0.5..1] where
//       0.5 is in focus and 1 is not.
//-----------------------------------------------------------------------------
float4
DoFPixelShader14(float2 Tex					: TEXCOORD0,
								 float2 JitterUV[3]	: TEXCOORD1) : COLOR0
{
	float4 Original = tex2D(LinearTextureSampler, Tex);
	float4 Jitter[3];
	float3 Blurred;
	
	for(int i = 0; i < 3; i++) {
		// Lookup into the rendertarget based on a texture coord.
		Jitter[i] = tex2D(LinearTextureSampler, JitterUV[i]);
        
		// Lerp between original rgb and the jitter rgb based on the alpha value
		//Jitter[i].rgb = lerp(Original.rgb, Jitter[i].rgb, Original.a);
		Jitter[i].rgb = lerp(Original.rgb, Jitter[i].rgb, saturate(Original.a*Jitter[i].a));
	}
        
	// Average the first two jitter samples
	Blurred = lerp(Jitter[0].rgb, Jitter[1].rgb, 0.5);
    
	// Equally weight all three jitter samples
	Blurred = lerp(Jitter[2].rgb, Blurred, 0.66666);
    
	return float4(Blurred, Original.a);
}


//-----------------------------------------------------------------------------
// Pixel Shader: TestPixelShader
// Desc: ...
//-----------------------------------------------------------------------------
float4
ColorPixelShader() : COLOR0
{
	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: TestPixelShader
// Desc: ...
//-----------------------------------------------------------------------------
float4
TestPixelShader(float2 Tex : TEXCOORD0) : COLOR0
{
	float alpha = tex2D(LinearTextureSampler, Tex).a;
	//alpha = alpha - 0.5f;  
	return alpha;//float4(alpha,0,0,0);//, alpha, alpha, alpha);
}

//-----------------------------------------------------------------------------
// Pixel Shader: BrightenPixelShader14
// Desc: Post effect shader for ps 1.4 that filters out the brightest pixels
//-----------------------------------------------------------------------------
float4
BrightenPixelShader14(float2 Tex : TEXCOORD0) : COLOR0
{
	//static const float middleGray = 0.18f;
	//static float luminance = 0.08f;
	//static const float whiteCutoff = 0.5f;
	float4 ColorOut = tex2D( LinearTextureSampler, Tex );

	//ColorOut = saturate(ColorOut - whiteCutoff);
	//if(ColorOut.r > 0)
	//	ColorOut.r += whiteCutoff;
	//if(ColorOut.g > 0)
	//	ColorOut.g += whiteCutoff;
	//if(ColorOut.b > 0)
	//	ColorOut.b += whiteCutoff;

	//ColorOut *= middleGray / (luminance + 0.001f);									// 2.222f
	//ColorOut *= (1.0f + (ColorOut / (whiteCutoff * whiteCutoff)));	//	1 + (c / 0.64f)
	//ColorOut -= 5.0f;

	//ColorOut = max(ColorOut, 0.0f);

	//ColorOut /= (10.0f + ColorOut);

	//return float4(1,1,1,1);
	return ColorOut;
}

//-----------------------------------------------------------------------------
// Pixel Shader: BrightenPixelShader20
// Desc: Post effect shader for ps 2.0 that filters out the brightest pixels
//-----------------------------------------------------------------------------
float4
BrightenPixelShader20(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 sample = tex2D( PointTextureSampler, Tex );
	float3 ColorOut = sample.rgb;

	// Map to LDR
	ColorOut *= Exposure/ (Luminance + 0.001f);

	// Subtract out dark pixels
	ColorOut -= WhiteCutoff;

	// Clamp to 0
	ColorOut = max(ColorOut, 0.0f);

	// Map the resulting value into the 0 to 1 range. Higher values than
	// 10 will isolate lights from illuminated scene 
	// objects.
	ColorOut /= (10.0f + ColorOut);

	/*ColorOut *= middleGray / (Luminance + 0.001f);
	ColorOut *= (1.0f + (ColorOut / (WhiteCutoff * WhiteCutoff)));
	ColorOut -= 5.0f;

	ColorOut = max(ColorOut, 0.0f);

	ColorOut /= (10.0f + ColorOut);*/

	return float4(ColorOut, sample.a);
}

//-----------------------------------------------------------------------------
// Pixel Shader: HBloomPixelShader14
// Desc: Blooms the image horizontally
//-----------------------------------------------------------------------------
float4
HBloomPixelShader14(float2 Tex : TEXCOORD0) : COLOR0
{
	static const float BloomScale = 1.4f;
	float4 Color = 0;

	for(int i = 0; i < cKernelSize14; i++) {    
		if(i == 2) {
			Color += tex2D(PointTextureSampler,  Tex + HorizontalKernel14[i].xy ) * BlurWeights14[i] * BloomScale;
		} else {
			Color += tex2D(LinearTextureSampler,  Tex + HorizontalKernel14[i].xy ) * BlurWeights14[i] * BloomScale;
		}
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: HBloomPixelShader20
// Desc: Blooms the image horizontally
//-----------------------------------------------------------------------------
float4
HBloomPixelShader20(float2 Tex : TEXCOORD0) : COLOR0
{
	//static const float BloomScale = 1.5f;
	float4 Color = 0;

	for(int i = 0; i < cKernelSize20; i++) {    
		Color += tex2D(PointTextureSampler,  Tex + HorizontalKernel20[i].xy ) * BlurWeights20[i];
	}

	return Color * BloomScale;
}

//-----------------------------------------------------------------------------
// Pixel Shader: VBloomPixelShader14
// Desc: Blooms the image vertically
//-----------------------------------------------------------------------------
float4
VBloomPixelShader14(float2 Tex : TEXCOORD0) : COLOR0
{
	static const float BloomScale = 1.4f;
	float4 Color = 0;

	for(int i = 0; i < cKernelSize14; i++) {    
		if(i == 2) {
			Color += tex2D(PointTextureSampler,  Tex + VerticalKernel14[i].xy ) * BlurWeights14[i] * BloomScale;
		} else {
			Color += tex2D(LinearTextureSampler,  Tex + VerticalKernel14[i].xy ) * BlurWeights14[i] * BloomScale;
		}
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: VBloomPixelShader20
// Desc: Blooms the image vertically
//-----------------------------------------------------------------------------
float4
VBloomPixelShader20(float2 Tex : TEXCOORD0) : COLOR0
{
	//static const float BloomScale = 1.5f;
	float4 Color = 0;

	for(int i = 0; i < cKernelSize20; i++) {    
		Color += tex2D(PointTextureSampler,  Tex + VerticalKernel20[i].xy ) * BlurWeights20[i] * BloomScale;
	}

	return Color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: InversePixelShader
// Desc: Post effect shader that takes invers of the pixel color.
//-----------------------------------------------------------------------------
float4
InversePixelShader(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 ColorOut = tex2D( PointTextureSampler, Tex );

	return 1.0f - ColorOut;
}

//-----------------------------------------------------------------------------
// Pixel Shader: ToneMapPixelShader20
// Desc: Post effect shader for ps 2.0 that filter using a tone map filter.
//-----------------------------------------------------------------------------
float4
ToneMapPixelShader20(float2 Tex : TEXCOORD0) : COLOR0
{
	// sample color
	float4 color = tex2D(PointTextureSampler, Tex);

	//	
	color *= Exposure / (Luminance + 0.001f);
	color /= (1.0f + color);

	return color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: FinalHDR_PixelShader20
// Desc: Post effect shader for ps 2.0 that constructs final image for HDR.
// For average luminance the 1x1 LuminanceTexture is assumed to contain the average 
// luminance
//-----------------------------------------------------------------------------
float4
FinalHDR_PixelShader20(float2 Tex : TEXCOORD0,
											 uniform bool linearBlend) : COLOR0
{
	// sample color
	float4 color = tex2D(PointTextureSampler, Tex);
	float4 bloom;
	
	if(linearBlend) {
		// If the hardware supports linear filtering of a
		// floating point render target that this step can be skipped.
		float xWeight = frac(Tex.x / TextureDimensions.x) - 0.5;
		float xDir = xWeight;
		xWeight = abs(xWeight);
		xDir /= xWeight;
		xDir *= TextureDimensions.x;

		float yWeight = frac(Tex.y / TextureDimensions.y) - 0.5;
		float yDir = yWeight;
		yWeight = abs(yWeight);
		yDir /= yWeight;
		yDir *= TextureDimensions.y;

		// sample the blur texture for the 4 relevant pixels, weighted accordingly
		bloom =  ((1.0f - xWeight) * (1.0f - yWeight))	* tex2D(PointTextureSampler2, Tex);
		bloom += (xWeight * (1.0f - yWeight))						* tex2D(PointTextureSampler2, Tex + float2(xDir, 0.0f));
		bloom += (yWeight * (1.0f - xWeight))						* tex2D(PointTextureSampler2, Tex + float2(0.0f, yDir));
		bloom += (xWeight * yWeight)										* tex2D(PointTextureSampler2, Tex + float2(xDir, yDir));

	} else {
		bloom = tex2D(LinearTextureSampler2, Tex);
	}

	float4 luminance = tex2D(LuminanceTextureSampler, float2(0,0));

	// Map to LDR
	color *= Exposure / (Luminance.r + 0.001f);
	// Tone map
	color /= (1.0f + color);

	// Add bloom
	color += bloom;

	return color;
}

//-----------------------------------------------------------------------------
// Pixel Shader: FinalHDR_PixelShader14
// Desc: Post effect shader for ps 1.4 that constructs final image for HDR.
//-----------------------------------------------------------------------------
float4
FinalHDR_PixelShader14(float2 Tex : TEXCOORD0) : COLOR0
{
	// sample color
	float4 color = tex2D(PointTextureSampler, Tex);
	float4 bloom = tex2D(LinearTextureSampler2, Tex);

	// Map to LDR
	//color *= Exposure / (Luminance + 0.001f);
	// Tone map
	//color /= (1.0f + color);

	// Add bloom
	color += bloom;

	return color;
	//return bloom;
}
//-----------------------------------------------------------------------------
// Pixel Shader: ToneMapPixelShader14
// Desc: Post effect shader for ps 1.4 that filter using a tone map filter.
//-----------------------------------------------------------------------------
float4
ToneMapPixelShader14(float2 Tex : TEXCOORD0) : COLOR0
{
	return tex2D(PointTextureSampler, Tex);
}


//-----------------------------------------------------------------------------
// Pixel Shader: HDRLuminanceGreyDS20
// Desc: First step of HDR luminance downsampling, includes conversion to greyscale
//-----------------------------------------------------------------------------
float4
HDRLuminanceGreyDS20(float2 Tex : TEXCOORD0) : COLOR0
{
	const float3 LuminanceWeights = float3(0.33f, 0.34f, 0.33f);
	
	float4 sample = tex2D( LinearTextureSampler, Tex );
	float average = dot(sample.rgb, LuminanceWeights);
	return float4(average, average, average, sample.a);
}

//-----------------------------------------------------------------------------
// Pixel Shader: HDRLuminanceDS20
// Desc: Consecutive steps of HDR luminance downsampling
//-----------------------------------------------------------------------------
float4
HDRLuminanceDS20(float2 Tex : TEXCOORD0) : COLOR0
{
	float4 sample = tex2D( LinearTextureSampler, Tex );

	return sample;
}

/***********************************************************************************
 * ------------------
 * Techniques
 * ------------------
***********************************************************************************/

#ifdef INCLUDE_PS_2_0
technique DownSample4x
{
	pass p0
	<
		float ScaleX = 0.25f;
		float ScaleY = 0.25f;
	>
	{
		VertexShader = null;
		PixelShader = compile ps_2_0 DownSample4x();
		ZEnable = false;
	}
}

technique UpSample4x
{
	pass p0
	<
		float ScaleX = 4.0f;
		float ScaleY = 4.0f;
	>
	{
		VertexShader = null;
		PixelShader = compile ps_2_0 Copy();
		ZEnable = false;
	}
}

technique ToneMap_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 ToneMapPixelShader20();
		ZEnable						= false;
	}
}

technique FinalHDR_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 FinalHDR_PixelShader20(true);
		ZEnable						= false;
	}
}

technique InverseColor_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 InversePixelShader();
		ZEnable						= false;
	}
}

technique HorizontalBloom_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 HBloomPixelShader20();
		ZEnable						= false;
	}
}

technique VerticalBloom_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 VBloomPixelShader20();
		ZEnable						= false;
	}
}

technique Brighten_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 BrightenPixelShader20();
		ZEnable						= false;
		//AlphaBlendEnable	= false;
		//BlendOp						= Add;
		//SrcBlend					= <SourceBlend>;
		//DestBlend					= <DestBlend>;
		//BlendFactor				= <BlendFactor>;
	}
}

technique HDRLuminanceGreyDS_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 HDRLuminanceGreyDS20();
		ZEnable						= false;
		//AlphaBlendEnable	= false;
		//BlendOp						= Add;
		//SrcBlend					= <SourceBlend>;
		//DestBlend					= <DestBlend>;
		//BlendFactor				= <BlendFactor>;
	}
}

technique HDRLuminanceDS_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 HDRLuminanceDS20();
		ZEnable						= false;
		//AlphaBlendEnable	= false;
		//BlendOp						= Add;
		//SrcBlend					= <SourceBlend>;
		//DestBlend					= <DestBlend>;
		//BlendFactor				= <BlendFactor>;
	}
}

technique Monochrome
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 MonoPixelShader();
		ZEnable						= false;
	}
}

technique HorizontalSmear_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 HSmearPixelShader20();
		ZEnable						= false;
	}
}

technique VerticalSmear_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 VSmearPixelShader20();
		ZEnable						= false;
	}
}

technique HorizontalBlur_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 HBlurPixelShader20();
		ZEnable						= false;
	}
}

technique VerticalBlur_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 VBlurPixelShader20();
		ZEnable						= false;
	}
}

technique Copy_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 CopyPixelShader();
		//AlphaBlendEnable	= false;
		ZEnable						= false;
	}
}

technique Blend_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 BlendPixelShader(false);
		//AlphaBlendEnable	= true;
		//BlendOp						= Add;
		//SrcBlend					= One;
		//DestBlend					= One;
		//SrcBlend					= <SourceBlend>;
		//DestBlend					= <DestBlend>;
		//BlendFactor					= <BlendFactor>;
		ZEnable						= false;
	}
}

technique DoF_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 DoFPixelShader20(DofKernel20, 12);
		ZEnable						= false;
	}
}

technique Color_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 ColorPixelShader();
		//AlphaBlendEnable	= true;
		//BlendOp						= Add;
		//SrcBlend					= <SourceBlend>;
		//DestBlend					= <DestBlend>;
		//BlendFactor					= <BlendFactor>;
		ZEnable						= false;
	}
}

/*technique Test_2_0
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_2_0 DoFPixelShader20(DofKernel20, 12);
		ZEnable						= false;
	}
}*/

#endif

#ifdef INCLUDE_PS_1_4
technique ToneMap_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 ToneMapPixelShader14();
		ZEnable						= false;
	}
}

technique HDRLuminanceGreyDS_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 ToneMapPixelShader14();
		ZEnable						= false;
	}
}

technique HDRLuminanceDS_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 ToneMapPixelShader14();
		ZEnable						= false;
	}
}

technique FinalHDR_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 FinalHDR_PixelShader14();
		ZEnable						= false;
	}
}

technique InverseColor_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 InversePixelShader();
		ZEnable						= false;
	}
}

technique HorizontalBloom_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 HBloomPixelShader14();
		ZEnable						= false;
	}
}

technique VerticalBloom_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 VBloomPixelShader14();
		ZEnable						= false;
	}
}

technique Brighten_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 BrightenPixelShader14();
		ZEnable						= false;
	}
}

technique HorizontalSmear_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 HSmearPixelShader14();
		ZEnable						= false;
		Wrap0							= 0;
		AddressU[0]				= Clamp;
		AddressV[0]				= Clamp;
		AddressW[0]				= Clamp;
	}
}

technique VerticalSmear_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 VSmearPixelShader14();
		ZEnable						= false;
	}
}

technique HorizontalBlur_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 HBlurPixelShader14();
		ZEnable						= false;
	}
}

technique VerticalBlur_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 VBlurPixelShader14();
		ZEnable						= false;
	}
}

technique Copy_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 CopyPixelShader();
		//AlphaBlendEnable	= false;
		ZEnable						= false;
	}
}

technique Blend_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 BlendPixelShader(false);
		//AlphaBlendEnable	= true;
		//BlendOp						= Add;
		//SrcBlend					= <SourceBlend>;
		//DestBlend					= <DestBlend>;
		//BlendFactor					= <BlendFactor>;
		ZEnable						= false;
	}
}

technique DoF_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 DoFPixelShader14();
		ZEnable						= false;
	}
}

technique Test_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 TestPixelShader();
		ZEnable						= false;
	}
}

technique Color_1_4
{
	pass BasePass
	{
		VertexShader			= null;
		PixelShader				= compile ps_1_4 ColorPixelShader();
		//AlphaBlendEnable	= true;
		//BlendOp						= Add;
		//SrcBlend					= <SourceBlend>;
		//DestBlend					= <DestBlend>;
		//BlendFactor					= <BlendFactor>;
		ZEnable						= false;
	}
}

#endif


