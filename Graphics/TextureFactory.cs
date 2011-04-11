using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class TextureFactory : ITextureFactory
    {
        private Texture2D whiteTexture;

        private IGraphicsFactory factory;
        private List<Texture2DParameters> files = new List<Texture2DParameters>();
        private List<TextureCubeParameters> cubeFiles = new List<TextureCubeParameters>();


        public GraphicsDevice GraphicsDevice
        {
            get { return factory.GraphicsDevice; }
        }

        public TextureFactory(IGraphicsFactory factory)
        {
            this.factory = factory;
        }

        public bool TextureExists(string name)
        {
            return files.Find(delegate(Texture2DParameters item) { return name == item.Name; }) != null;
        }

        public Texture2D CreateFromName(string file)
        {
            Texture2DParameters result = files.Find(delegate(Texture2DParameters item) { return file == item.Name; });
            if (result != null)
            {
                return result.Texture;
            }
            Texture2D texture = factory.Texture2DFromFile(file);
            files.Add(new Texture2DParameters(file, texture));
            return texture;
        }

        public TextureCube CreateCubeFromFile(string file)
        {
            TextureCubeParameters result = cubeFiles.Find(delegate(TextureCubeParameters item) { return file == item.Name; });
            if (result != null)
            {
                return result.Texture;
            }
            TextureCube texture = factory.TextureCubeFromFile(file);
            cubeFiles.Add(new TextureCubeParameters(file, texture));
            return texture;
        }

        public RenderTarget2D CreateFullsizeRenderTarget()
        {
            return new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public RenderTarget2D CreateFullsizeRenderTarget(SurfaceFormat format, DepthFormat depthformat, int preferredMultiSampleCount)
        {
            return new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight,
                false, format, depthformat, preferredMultiSampleCount, RenderTargetUsage.DiscardContents);
        }

        public Texture2D CreateFromGenerator(string name, int width, int height, bool mipMap, SurfaceFormat format, ITextureGenerator generator)
        {
            if (files.Find(delegate(Texture2DParameters item) { return name == item.Name; }) != null)
                throw new DDXXException("Texture with name " + name + " already created.");
            Texture2D texture = CreateFromFunction(width, height, mipMap, format, generator.GenerateTexture);
            if (name != null && name != "")
                files.Add(new Texture2DParameters(name, texture, generator));
            return texture;
        }

        public void Update(Texture2DParameters target)
        {
            target.Regenerate();
        }

        public Texture2D CreateFromFunction(int width, int height, bool mipMap, SurfaceFormat format, Generate2DTextureCallback callbackFunction)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, width, height, mipMap, format);

            SetFromFunction(texture, callbackFunction);

            return texture;
        }

        private void SetFromFunction(Texture2D texture, Generate2DTextureCallback callbackFunction)
        {
            int i = 0;
            Color[] data = new Color[texture.Width * texture.Height];
            Vector4[,] vectorData = callbackFunction(texture.Width, texture.Height);
            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    data[i++] = new Color(vectorData[x, y]);
                }
            }
            texture.SetData<Color>(data);

            int levelWidth = texture.Width;
            int levelHeight = texture.Height;
            //texture.Save("before.dds", ImageFileFormat.Dds);
            for (int j = 0; j < texture.LevelCount - 1; j++)
            {
                levelHeight = levelHeight / 2;
                levelWidth = levelWidth / 2;
                Color[] newData = new Color[levelWidth * levelHeight];
                i = 0;
                for (int y = 0; y < levelHeight; y++)
                {
                    for (int x = 0; x < levelWidth; x++)
                    {
                        int w2 = levelWidth * 2;
                        int h2 = levelHeight * 2;
                        int x2 = x * 2;
                        int y2 = y * 2;
                        newData[y * levelWidth + x] = new Color(
                            (GetData(data, x2 - 1, y2, w2, h2) +
                            GetData(data, x2 + 1, y2, w2, h2) +
                            GetData(data, x2, y2 - 1, w2, h2) +
                            GetData(data, x2, y2 + 1, w2, h2) +
                            GetData(data, x2, y2, w2, h2)) / 5.0f);
                        i++;
                    }
                }
                texture.SetData<Color>(j + 1, null, newData, 0, newData.Length);
                data = newData;
            }
            //texture.Save("after.dds", ImageFileFormat.Dds);
        }

        private Vector4 GetData(Color[] data, int x, int y, int width, int height)
        {
            if (y < 0)
                y = 0;
            if (x < 0)
                x = 0;
            if (x >= width)
                x = width - 1;
            if (y >= height)
                y = height - 1;
            return data[y * width + x].ToVector4();
        }

        public Texture2D WhiteTexture
        {
            get
            {
                if (whiteTexture == null)
                    whiteTexture = CreateFromFunction(1, 1, false, SurfaceFormat.Color, delegate(int w, int h) { Vector4[,] array = new Vector4[1, 1]; array[0, 0] = Vector4.One; return array; });
                return whiteTexture;
            }
        }

        public List<Texture2DParameters> Texture2DParameters
        {
            get
            {
                return files;
            }
        }

    }
}
