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
            private RenderTarget2D renderTarget;
            private Texture2D texture;

            public RenderTarget2D RenderTarget
            {
                get { return renderTarget; }
                set { renderTarget = value; }
            }
            public Texture2D Texture
            {
                get { if (texture == null) return renderTarget as Texture2D; else return texture; }
                set { texture = value; }
            }
            public float scale;
            public bool allocated;
            public TextureContainer(RenderTarget2D texture)
            {
                this.renderTarget = texture;
                scale = 1.0f;
            }
        }
        private IGraphicsFactory graphicsFactory;
        private RenderTarget2D lastUsedTexture;
        private TextureContainer inputTextureContainer = new TextureContainer(null);
        private TextureContainer sourceTextureContainer = new TextureContainer(null);
        private SpriteBatch spriteBatch;
        private Effect effect;
        //private bool shouldClear;
        private List<TextureContainer> textures = new List<TextureContainer>();

        public BlendState BlendState { set; private get; }

        public PostProcessor()
        {
            BlendState = BlendState.Opaque;
        }

        public void Initialize(IGraphicsFactory graphicsFactory)
        {
            this.graphicsFactory = graphicsFactory;
            effect = graphicsFactory.EffectFactory.CreateFromFile("Content\\effects\\PostEffects");
            spriteBatch = new SpriteBatch(graphicsFactory.GraphicsDevice);

            HandleAnnotations();
        }

        private void HandleAnnotations()
        {
            foreach (EffectParameter parameterTo in effect.Parameters)
            {
                EffectAnnotation annotation = parameterTo.Annotations["ConvertPixelsToTexels"];
                if (annotation != null)
                {
                    EffectParameter parameterFrom = effect.Parameters[annotation.GetValueString()];
                    int numElements = parameterFrom.Elements.Count;
                    float[] values = parameterFrom.GetValueSingleArray(numElements * 2);
                    for (int j = 0; j < numElements; j++)
                    {
                        values[j * 2 + 0] /= graphicsFactory.GraphicsDevice.PresentationParameters.BackBufferWidth;
                        values[j * 2 + 1] /= graphicsFactory.GraphicsDevice.PresentationParameters.BackBufferHeight;
                    }
                    parameterTo.SetValue(values);
                }
            }
        }

        public RenderTarget2D OutputTexture
        {
            get { return lastUsedTexture; }
        }

        public void StartFrame(RenderTarget2D startTexture)
        {
            inputTextureContainer.RenderTarget = startTexture;
            inputTextureContainer.scale = 1.0f;
            lastUsedTexture = startTexture;
        }

        public void Process(string technique, RenderTarget2D source, RenderTarget2D destination)
        {
            effect.Parameters["Time2D"].SetValue(new float[] { 
                (1.23f * Time.CurrentTime * Time.CurrentTime) % 1,
                (2.495f * Time.CurrentTime) % 1
            });
            TextureContainer sourceContainer = GetContainer(source, true);
            TextureContainer destinationContainer = GetContainer(destination, false);
            SetupProcessParameters(technique, destinationContainer);

            //graphicsFactory.GraphicsDevice.SetRenderTarget(null);
            //System.IO.FileStream stream1 = System.IO.File.OpenWrite("before.jpg");
            //destinationContainer.Texture.SaveAsJpeg(stream1, 1280, 720);
            //stream1.Close();
            ProcessPasses(technique, sourceContainer, destinationContainer);
            //System.IO.FileStream stream2 = System.IO.File.OpenWrite("after.jpg");
            //destinationContainer.Texture.SaveAsJpeg(stream2, 1280, 720);
            //stream2.Close();

            graphicsFactory.GraphicsDevice.SetRenderTarget(null);
            lastUsedTexture = destination;

            //sourceContainer.Texture.GetTexture().Save("source.jpg", ImageFileFormat.Jpg);
            //destinationContainer.Texture.GetTexture().Save("destination.jpg", ImageFileFormat.Jpg);
        }

        public void Process(string technique, Texture2D source, RenderTarget2D destination)
        {

            sourceTextureContainer.Texture = source;
            Process(technique, (RenderTarget2D)null, destination);
        }

        private TextureContainer GetContainer(RenderTarget2D source, bool useSourceIfNotFound)
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
            graphicsFactory.GraphicsDevice.SetRenderTarget(destination.RenderTarget);
            effect.CurrentTechnique = effect.Techniques[technique];
        }

        private void ProcessPasses(string technique, TextureContainer source, TextureContainer destination)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
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
                int destHeight = graphicsFactory.GraphicsDevice.PresentationParameters.BackBufferHeight;
                int destWidth = graphicsFactory.GraphicsDevice.PresentationParameters.BackBufferWidth;
                int sourceHeight = source.Texture.Height;
                int sourceWidth = source.Texture.Width;
                if (destination.scale > toScale)
                    graphicsFactory.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
                destination.scale = toScale;
                spriteBatch.Draw(source.Texture,
                        new Rectangle(0, 0, (int)(destWidth * toScale), (int)(destHeight * toScale)),
                        new Rectangle(0, 0, (int)(sourceWidth * fromScale), (int)(sourceHeight * fromScale)),
                        Color.White);
                spriteBatch.End();
            }
        }

        //private VertexPositionTexture[] CreateVertexStruct(string technique, TextureContainer source, TextureContainer destination, EffectPass pass)
        //{
        //    VertexPositionTexture[] vertices = new VertexPositionTexture[4];
        //    float fromScale = source.scale;
        //    float toScale = fromScale * pass.Annotations["Scale"].GetValueSingle();
        //    if (toScale > 1.0f)
        //        throw new DDXXException("Can not scale larger than back buffer size");
        //    int height = graphicsFactory.GraphicsDevice.PresentationParameters.BackBufferHeight;
        //    int width = graphicsFactory.GraphicsDevice.PresentationParameters.BackBufferWidth;
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
            //for (int i = 0; i < textures.Count; i++) {
            //    xxx(textures[i].RenderTarget as Texture2D).SaveAsJpeg("Container" + i + ".jpg");
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

        public void SetValue(string name, Texture2D value)
        {
            effect.Parameters[name].SetValue(value);
        }

        public List<RenderTarget2D> GetTemporaryTextures(int num, bool skipOutput)
        {
            List<RenderTarget2D> tempTextures = new List<RenderTarget2D>();
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
                RenderTarget2D newTexture = graphicsFactory.TextureFactory.CreateFullsizeRenderTarget();
                textures.Add(new TextureContainer(newTexture));
                tempTextures.Add(newTexture);
            }
            if (tempTextures[0] == lastUsedTexture)
            {
                RenderTarget2D temp = tempTextures[0];
                tempTextures[0] = tempTextures[1];
                tempTextures[1] = temp;
            }
            return tempTextures;
        }

        public void AllocateTexture(RenderTarget2D texture)
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

        public void FreeTexture(RenderTarget2D texture)
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
