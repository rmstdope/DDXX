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
            public bool allocated;
            public TextureContainer(ITexture texture)
            {
                this.texture = texture;
                scale = 1.0f;
            }
        }
        private IDevice device;
        private ITextureFactory textureFactory;
        private ITexture lastUsedTexture;
        private TextureContainer inputTextureContainer = new TextureContainer(null);
        private TextureContainer sourceTextureContainer = new TextureContainer(null);
        private IEffect effect;
        private EffectHandle sourceTextureParameter;
        private BlendOperation blendOperation = BlendOperation.Add;
        private Blend sourceBlend = Blend.One;
        private Blend destinatonBlend = Blend.Zero;
        private Color blendFactor = Color.Black;
        private bool shouldClear;
        private List<TextureContainer> textures = new List<TextureContainer>();

        public PostProcessor()
        {
        }

        public void Initialize(IDevice device, ITextureFactory textureFactory, IEffectFactory effectFactory)
        {
            this.device = device;
            effect = effectFactory.CreateFromFile("PostEffects.fxo");
            this.textureFactory = textureFactory;

            sourceTextureParameter = effect.GetParameter(null, "SourceTexture");

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
            get { return lastUsedTexture; }
        }

        public void StartFrame(ITexture startTexture)
        {
            inputTextureContainer.Texture = startTexture;
            inputTextureContainer.scale = 1.0f;
            lastUsedTexture = startTexture;
        }

        public void SetBlendParameters(BlendOperation blendOperation, Blend sourceBlend, Blend destinatonBlend, Color blendFactor)
        {
            this.blendOperation = blendOperation;
            this.sourceBlend = sourceBlend;
            this.destinatonBlend = destinatonBlend;
            this.blendFactor = blendFactor;
        }

        public void Process(string technique, ITexture source, ITexture destination)
        {
            TextureContainer sourceContainer = GetContainer(source, true);
            TextureContainer destinationContainer = GetContainer(destination, false);
            SetupProcessParameters(technique, sourceContainer, destinationContainer);

            device.BeginScene();
            ProcessPasses(technique, sourceContainer, destinationContainer);
            device.EndScene();

            lastUsedTexture = destination;

            //sourceContainer.Texture.Save("source.dds", ImageFileFormat.Dds);
            //destinationContainer.Texture.Save("destination.dds", ImageFileFormat.Dds);
        }

        private TextureContainer GetContainer(ITexture source, bool useSourceIfNotFound)
        {
            if (source == inputTextureContainer.Texture)
                return inputTextureContainer;
            foreach (TextureContainer container in textures)
                if (container.Texture == source)
                    return container;
            if (useSourceIfNotFound)
            {
                sourceTextureContainer.Texture = source;
                return sourceTextureContainer;
            }
            throw new DDXXException("Unknown texture");
        }

        private void SetupProcessParameters(string technique, TextureContainer source, TextureContainer destination)
        {
            using (ISurface renderTarget = destination.Texture.GetSurfaceLevel(0))
            {
                device.SetRenderTarget(0, renderTarget);
                effect.SetValue(sourceTextureParameter, source.Texture);
                effect.Technique = technique;
                device.VertexFormat = CustomVertex.TransformedTextured.Format;
            }
        }

        private void ProcessPasses(string technique, TextureContainer source, TextureContainer destination)
        {
            int passes = effect.Begin(FX.None);
            SetupRenderState();
            for (int pass = 0; pass < passes; pass++)
            {
                CustomVertex.TransformedTextured[] vertices = CreateVertexStruct(technique, source, destination, pass);
                effect.BeginPass(pass);
                if (shouldClear)
                    device.Clear(ClearFlags.Target, Color.Black, 0, 0);
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

        private CustomVertex.TransformedTextured[] CreateVertexStruct(string technique, TextureContainer source, TextureContainer destination, int pass)
        {
            CustomVertex.TransformedTextured[] vertices = new CustomVertex.TransformedTextured[4];
            float fromScale = source.scale;
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
            if (destination.scale > toScale)
                shouldClear = true;
            else
                shouldClear = false;
            destination.scale = toScale;
            return vertices;
        }

        public void DebugWriteAllTextures()
        {
            for (int i = 0; i < textures.Count; i++)
                textures[i].Texture.Save("Container" + i + ".jpg", ImageFileFormat.Jpg);
        }

        public void SetValue(string name, float value)
        {
            effect.SetValue(name, value);
        }

        public void SetValue(string name, float[] value)
        {
            effect.SetValue(name, value);
        }

        public void SetValue(string name, Vector2 value)
        {
            float[] values = new float[] { value.X, value.Y };
            effect.SetValue(name, values);
        }

        public void SetValue(string name, Vector4 value)
        {
            effect.SetValue(name, value);
        }

        public List<ITexture> GetTemporaryTextures(int num, bool skipOutput)
        {
            List<ITexture> tempTextures = new List<ITexture>();
            foreach (TextureContainer container in textures)
            {
                if (!container.allocated)
                {
                    if (container.Texture == lastUsedTexture)
                    {
                        if (num != 1 && !skipOutput)
                        {
                            tempTextures.Add(container.Texture);
                        }
                    }
                    else
                    {
                        tempTextures.Add(container.Texture);
                    }
                }
            }
            int numToAdd = num - tempTextures.Count;
            for (int i = 0; i < numToAdd; i++)
            {
                ITexture newTexture = textureFactory.CreateFullsizeRenderTarget(Format.A8R8G8B8);
                textures.Add(new TextureContainer(newTexture));
                tempTextures.Add(newTexture);
            }
            if (tempTextures[0] == lastUsedTexture)
            {
                ITexture temp = tempTextures[0];
                tempTextures[0] = tempTextures[1];
                tempTextures[1] = temp;
            }
            return tempTextures;
        }

        public void AllocateTexture(ITexture texture)
        {
            foreach (TextureContainer container in textures)
            {
                if (container.Texture == texture)
                {
                    if (container.allocated)
                        throw new DDXXException("Same texture allocated twice.");
                    container.allocated = true;
                    return;
                }
            }
            throw new DDXXException("Texture not found.");
        }

        public void FreeTexture(ITexture texture)
        {
            foreach (TextureContainer container in textures)
            {
                if (container.Texture == texture)
                {
                    container.allocated = false;
                    return;
                }
            }
            throw new DDXXException("Unknown texture");
        }
    }
}
