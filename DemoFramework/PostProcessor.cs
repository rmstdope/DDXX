using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{
    public class PostProcessor : IPostProcessor
    {
        private class TextureContainer
        {
            private ITexture texture;
            public ITexture Texture
            {
                get { return texture; }
                set { texture = value; }
            }
            public float scale;
            public TextureContainer(ITexture texture)
            {
                this.texture = texture;
                scale = 1.0f;
            }
        }
        private IDevice device;
        private TextureID lastUsedTexture;
        private ITexture inputTexture;
        private TextureContainer[] textures;
        private IEffect effect;
        private EffectHandle sourceTextureParameter;
        private BlendOperation blendOperation = BlendOperation.Add;
        private Blend sourceBlend = Blend.One;
        private Blend destinatonBlend = Blend.Zero;
        private Color blendFactor = Color.Black;

        public PostProcessor()
        {
        }

        public void Initialize(IDevice device)
        {
            this.device = device;
            effect = D3DDriver.EffectFactory.CreateFromFile("../../../Effects/PostEffects.fxo");

            sourceTextureParameter = effect.GetParameter(null, "SourceTexture");

            textures = new TextureContainer[(int)TextureID.NUM_TEXTURES];
            for (int i = 0; i < (int)TextureID.NUM_TEXTURES; i++)
            {
                if (i == (int)TextureID.INPUT_TEXTURE)
                    textures[i] = new TextureContainer(null);
                else
                    textures[i] = new TextureContainer(D3DDriver.TextureFactory.CreateFullsizeRenderTarget());
            }

            HandleAnnotations();
        }

        private void HandleAnnotations()
        {
            for (int i = 0; i < effect.Description_Parameters; i++)
            {
                EffectHandle parameterTo = effect.GetParameter(null, i);
                EffectHandle annotation = effect.GetAnnotation(parameterTo, "ConvertPixelsToTexels");
                if (annotation != null)
                {
                    EffectHandle parameterFrom = effect.GetParameter(null, effect.GetValueString(annotation));
                    int numElements = effect.GetParameterDescription_Elements(parameterFrom);
                    float[] values = effect.GetValueFloatArray(parameterFrom, numElements * 2);
                    for (int j = 0; j < numElements; j++)
                    {
                        values[j * 2 + 0] /= device.PresentationParameters.BackBufferWidth;
                        values[j * 2 + 1] /= device.PresentationParameters.BackBufferHeight;
                    }
                    effect.SetValue(parameterTo, values);
                }
            }
        }

        public ITexture OutputTexture
        {
            get
            {
                return textures[(int)lastUsedTexture].Texture;
            }
        }

        public TextureID OutputTextureID
        {
            get { return lastUsedTexture; }
        }

        public void StartFrame(ITexture startTexture)
        {
            inputTexture = startTexture;
            textures[(int)TextureID.INPUT_TEXTURE].Texture = startTexture;
            SetScale(TextureID.INPUT_TEXTURE, 1.0f);
            lastUsedTexture = TextureID.INPUT_TEXTURE;
        }

        private ITexture GetTexture(TextureID texture)
        {
            if (texture == TextureID.INPUT_TEXTURE)
                return inputTexture;
            return textures[(int)texture].Texture;
        }

        public void SetBlendParameters(BlendOperation blendOperation, Blend sourceBlend, Blend destinatonBlend, Color blendFactor)
        {
            this.blendOperation = blendOperation;
            this.sourceBlend = sourceBlend;
            this.destinatonBlend = destinatonBlend;
            this.blendFactor = blendFactor;
        }

        public void Process(string technique, TextureID source, TextureID destination)
        {
            //if (destination == TextureID.INPUT_TEXTURE)
            //    throw new DDXXException("Input texture not valid as output!");

            SetupProcessParameters(technique, source, destination);

            device.BeginScene();
            ProcessPasses(technique, source, destination);
            device.EndScene();

            lastUsedTexture = destination;
        }

        private void SetupProcessParameters(string technique, TextureID source, TextureID destination)
        {
            device.SetRenderTarget(0, GetTexture(destination).GetSurfaceLevel(0));
            effect.SetValue(sourceTextureParameter, GetTexture(source));
            effect.Technique = technique;
            device.VertexFormat = CustomVertex.TransformedTextured.Format;
        }

        private void ProcessPasses(string technique, TextureID source, TextureID destination)
        {
            int passes = effect.Begin(FX.None);
            SetupRenderState();
            for (int pass = 0; pass < passes; pass++)
            {
                CustomVertex.TransformedTextured[] vertices = CreateVertexStruct(technique, source, destination, pass);
                effect.BeginPass(pass);
                device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, vertices);
                effect.EndPass();
            }
            effect.End();
        }

        private void SetupRenderState()
        {
            if (BlendOperation.Add == blendOperation &&
                Blend.One == sourceBlend &&
                Blend.Zero == destinatonBlend)
            {
                device.RenderState.AlphaBlendEnable = false;
            }
            else
            {
                device.RenderState.AlphaBlendEnable = true;
                device.RenderState.BlendOperation = blendOperation;
                device.RenderState.SourceBlend = sourceBlend;
                device.RenderState.DestinationBlend = destinatonBlend;
                device.RenderState.BlendFactor = blendFactor;
        }
    }

        private CustomVertex.TransformedTextured[] CreateVertexStruct(string technique, TextureID source, TextureID destination, int pass)
        {
            CustomVertex.TransformedTextured[] vertices = new CustomVertex.TransformedTextured[4];
            float fromScale = GetScale(source);
            float toScale = fromScale * effect.GetValueFloat(effect.GetAnnotation(effect.GetPass(technique, pass), "Scale"));
            if (toScale > 1.0f)
                throw new DDXXException("Can not scale larger than back buffer size");
            int height = device.PresentationParameters.BackBufferHeight;
            int width = device.PresentationParameters.BackBufferWidth;
            vertices = new CustomVertex.TransformedTextured[4];
            vertices[0] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, -0.5f, 1.0f, 1.0f), 0, 0);
            vertices[1] = new CustomVertex.TransformedTextured(new Vector4(width * toScale - 0.5f, -0.5f, 1.0f, 1.0f), fromScale, 0);
            vertices[2] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, height * toScale - 0.5f, 1.0f, 1.0f), 0, fromScale);
            vertices[3] = new CustomVertex.TransformedTextured(new Vector4(width * toScale - 0.5f, height * toScale - 0.5f, 1.0f, 1.0f), fromScale, fromScale);
            SetScale(destination, toScale);
            return vertices;
        }

        private void SetScale(TextureID destination, float toScale)
        {
            textures[(int)destination].scale = toScale;
        }

        private float GetScale(TextureID source)
        {
            if (source == TextureID.INPUT_TEXTURE)
                return 1.0f;
            else
                return textures[(int)source].scale;
        }

        internal void DebugWriteAllTextures()
        {
            TextureLoader.Save("INPUT.jpg", ImageFileFormat.Jpg, ((TextureAdapter)inputTexture).TextureDX);
            TextureLoader.Save("FULLSCREEN_1.jpg", ImageFileFormat.Jpg, ((TextureAdapter)textures[(int)TextureID.FULLSIZE_TEXTURE_1].Texture).TextureDX);
            TextureLoader.Save("FULLSCREEN_2.jpg", ImageFileFormat.Jpg, ((TextureAdapter)textures[(int)TextureID.FULLSIZE_TEXTURE_2].Texture).TextureDX);
            TextureLoader.Save("FULLSCREEN_3.jpg", ImageFileFormat.Jpg, ((TextureAdapter)textures[(int)TextureID.FULLSIZE_TEXTURE_3].Texture).TextureDX);
        }

        public void SetValue(string name, float value)
        {
            effect.SetValue(name, value);
        }
    }
}