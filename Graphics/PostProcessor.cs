using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public class PostProcessor : IPostProcessor
    {
        private class TextureContainer
        {
            private IRenderTarget2D renderTarget;
            private ITexture2D texture;

            public IRenderTarget2D RenderTarget
            {
                get { return renderTarget; }
                set { renderTarget = value; }
            }
            public ITexture2D Texture
            {
                get { if (texture == null) return renderTarget.GetTexture(); else return texture; }
                set { texture = value; }
            }
            public float scale;
            public bool allocated;
            public TextureContainer(IRenderTarget2D texture)
            {
                this.renderTarget = texture;
                scale = 1.0f;
            }
        }
        private IGraphicsDevice device;
        private ITextureFactory textureFactory;
        private IRenderTarget2D lastUsedTexture;
        private TextureContainer inputTextureContainer = new TextureContainer(null);
        private TextureContainer sourceTextureContainer = new TextureContainer(null);
        private ISpriteBatch spriteBatch;
        private IEffect effect;
        private BlendFunction blendOperation = BlendFunction.Add;
        private Blend sourceBlend = Blend.One;
        private Blend destinatonBlend = Blend.Zero;
        private Color blendFactor = Color.Black;
        //private bool shouldClear;
        private List<TextureContainer> textures = new List<TextureContainer>();

        public PostProcessor()
        {
        }

        public void Initialize(IGraphicsDevice device, IGraphicsFactory graphicsFactory, ITextureFactory textureFactory, IEffectFactory effectFactory)
        {
            this.device = device;
            effect = effectFactory.CreateFromFile("Content\\effects\\PostEffects");
            spriteBatch = graphicsFactory.CreateSpriteBatch();
            this.textureFactory = textureFactory;

            HandleAnnotations();
        }

        private void HandleAnnotations()
        {
            foreach (IEffectParameter parameterTo in effect.Parameters)
            {
                IEffectAnnotation annotation = parameterTo.Annotations["ConvertPixelsToTexels"];
                if (annotation != null)
                {
                    IEffectParameter parameterFrom = effect.Parameters[annotation.GetValueString()];
                    int numElements = parameterFrom.Elements.Count;
                    float[] values = parameterFrom.GetValueSingleArray(numElements * 2);
                    for (int j = 0; j < numElements; j++)
                    {
                        values[j * 2 + 0] /= device.PresentationParameters.BackBufferWidth;
                        values[j * 2 + 1] /= device.PresentationParameters.BackBufferHeight;
                    }
                    parameterTo.SetValue(values);
                }
            }
        }

        public IRenderTarget2D OutputTexture
        {
            get { return lastUsedTexture; }
        }

        public void StartFrame(IRenderTarget2D startTexture)
        {
            inputTextureContainer.RenderTarget = startTexture;
            inputTextureContainer.scale = 1.0f;
            lastUsedTexture = startTexture;
        }

        public void SetBlendParameters(BlendFunction blendOperation, Blend sourceBlend, Blend destinatonBlend, Color blendFactor)
        {
            this.blendOperation = blendOperation;
            this.sourceBlend = sourceBlend;
            this.destinatonBlend = destinatonBlend;
            this.blendFactor = blendFactor;
        }

        public void Process(string technique, IRenderTarget2D source, IRenderTarget2D destination)
        {
            effect.Parameters["Time2D"].SetValue(new float[] { 
                (1.23f * Time.CurrentTime * Time.CurrentTime) % 1,
                (2.495f * Time.CurrentTime) % 1
            });
            TextureContainer sourceContainer = GetContainer(source, true);
            TextureContainer destinationContainer = GetContainer(destination, false);
            SetupProcessParameters(technique, destinationContainer);

            ProcessPasses(technique, sourceContainer, destinationContainer);

            device.SetRenderTarget(0, null);//.ResolveRenderTarget(0);
            lastUsedTexture = destination;

            //sourceContainer.Texture.GetTexture().Save("source.jpg", ImageFileFormat.Jpg);
            //destinationContainer.Texture.GetTexture().Save("destination.jpg", ImageFileFormat.Jpg);
        }

        public void Process(string technique, ITexture2D source, IRenderTarget2D destination)
        {

            sourceTextureContainer.Texture = source;
            Process(technique, (IRenderTarget2D)null, destination);
        }

        private TextureContainer GetContainer(IRenderTarget2D source, bool useSourceIfNotFound)
        {
            if (source == inputTextureContainer.RenderTarget)
                return inputTextureContainer;
            foreach (TextureContainer container in textures)
                if (container.RenderTarget == source)
                    return container;
            if (useSourceIfNotFound)
            {
                return sourceTextureContainer;
            }
            throw new DDXXException("Unknown texture");
        }

        private void SetupProcessParameters(string technique, TextureContainer destination)
        {
            device.SetRenderTarget(0, destination.RenderTarget);
            effect.CurrentTechnique = effect.Techniques[technique];
        }

        private void ProcessPasses(string technique, TextureContainer source, TextureContainer destination)
        {
            spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
            effect.Begin();
            SetupRenderState();
            foreach (IEffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                float fromScale = source.scale;
                float toScale;
#if (XBOX)
                // Workaround for bug in annotations
                if (technique == "DownSample4x")
                    toScale = fromScale * 0.25f;
                else if (technique == "UpSample4x")
                    toScale = fromScale * 4.0f;
                else
                    toScale = fromScale;
#else
                toScale = fromScale * pass.Annotations["Scale"].GetValueSingle();
#endif
                if (toScale > 1.0f)
                    throw new DDXXException("Can not scale larger than back buffer size");
                int destHeight = device.PresentationParameters.BackBufferHeight;
                int destWidth = device.PresentationParameters.BackBufferWidth;
                int sourceHeight = source.Texture.Height;
                int sourceWidth = source.Texture.Width;
                if (destination.scale > toScale)
                    device.Clear(ClearOptions.Target, Color.Black, 0, 0);
                destination.scale = toScale;
                spriteBatch.Draw(source.Texture,
                        new Rectangle(0, 0, (int)(destWidth * toScale), (int)(destHeight * toScale)),
                        new Rectangle(0, 0, (int)(sourceWidth * fromScale), (int)(sourceHeight * fromScale)),
                        Color.White);
                spriteBatch.End();
                pass.End();
            }
            effect.End();
        }

        private void SetupRenderState()
        {
            if (BlendFunction.Add == blendOperation &&
                Blend.One == sourceBlend &&
                Blend.Zero == destinatonBlend)
            {
                device.RenderState.AlphaBlendEnable = false;
            }
            else
            {
                device.RenderState.AlphaBlendEnable = true;
                device.RenderState.BlendFunction = blendOperation;
                device.RenderState.SourceBlend = sourceBlend;
                device.RenderState.DestinationBlend = destinatonBlend;
                device.RenderState.BlendFactor = blendFactor;
            }
        }

        //private VertexPositionTexture[] CreateVertexStruct(string technique, TextureContainer source, TextureContainer destination, IEffectPass pass)
        //{
        //    VertexPositionTexture[] vertices = new VertexPositionTexture[4];
        //    float fromScale = source.scale;
        //    float toScale = fromScale * pass.Annotations["Scale"].GetValueSingle();
        //    if (toScale > 1.0f)
        //        throw new DDXXException("Can not scale larger than back buffer size");
        //    int height = device.PresentationParameters.BackBufferHeight;
        //    int width = device.PresentationParameters.BackBufferWidth;
        //    vertices = new VertexPositionTexture[4];
        //    vertices[0] = new VertexPositionTexture(new Vector3(0, 0, 0.0f), new Vector2(0, 0));
        //    vertices[1] = new VertexPositionTexture(new Vector3(width * toScale - 0.5f, -0.5f, 1.0f, 1.0f), new Vector2(fromScale, 0));
        //    vertices[2] = new VertexPositionTexture(new Vector3(-0.5f, height * toScale - 0.5f, 1.0f, 1.0f), new Vector2(0, fromScale));
        //    vertices[3] = new VertexPositionTexture(new Vector3(width * toScale - 0.5f, height * toScale - 0.5f, 1.0f, 1.0f), new Vector2(fromScale, fromScale));
        //    if (destination.scale > toScale)
        //        shouldClear = true;
        //    else
        //        shouldClear = false;
        //    destination.scale = toScale;
        //    return vertices;
        //}

        public void DebugWriteAllTextures()
        {
#if (!XBOX)
            for (int i = 0; i < textures.Count; i++)
                textures[i].RenderTarget.GetTexture().Save("Container" + i + ".jpg", ImageFileFormat.Jpg);
#endif
        }

        public void SetValue(string name, float value)
        {
            effect.Parameters[name].SetValue(value);
        }

        public void SetValue(string name, float[] value)
        {
            effect.Parameters[name].SetValue(value);
        }

        public void SetValue(string name, Vector2 value)
        {
            float[] values = new float[] { value.X, value.Y };
            effect.Parameters[name].SetValue(values);
        }

        public void SetValue(string name, Vector4 value)
        {
            effect.Parameters[name].SetValue(value);
        }

        public List<IRenderTarget2D> GetTemporaryTextures(int num, bool skipOutput)
        {
            List<IRenderTarget2D> tempTextures = new List<IRenderTarget2D>();
            foreach (TextureContainer container in textures)
            {
                if (!container.allocated)
                {
                    if (container.RenderTarget == lastUsedTexture)
                    {
                        if (num != 1 && !skipOutput)
                        {
                            tempTextures.Add(container.RenderTarget);
                        }
                    }
                    else
                    {
                        tempTextures.Add(container.RenderTarget);
                    }
                }
            }
            int numToAdd = num - tempTextures.Count;
            for (int i = 0; i < numToAdd; i++)
            {
                IRenderTarget2D newTexture = textureFactory.CreateFullsizeRenderTarget();
                textures.Add(new TextureContainer(newTexture));
                tempTextures.Add(newTexture);
            }
            if (tempTextures[0] == lastUsedTexture)
            {
                IRenderTarget2D temp = tempTextures[0];
                tempTextures[0] = tempTextures[1];
                tempTextures[1] = temp;
            }
            return tempTextures;
        }

        public void AllocateTexture(IRenderTarget2D texture)
        {
            foreach (TextureContainer container in textures)
            {
                if (container.RenderTarget == texture)
                {
                    if (container.allocated)
                        throw new DDXXException("Same texture allocated twice.");
                    container.allocated = true;
                    return;
                }
            }
            throw new DDXXException("Texture not found.");
        }

        public void FreeTexture(IRenderTarget2D texture)
        {
            foreach (TextureContainer container in textures)
            {
                if (container.RenderTarget == texture)
                {
                    container.allocated = false;
                    return;
                }
            }
            throw new DDXXException("Unknown texture");
        }
    }
}
