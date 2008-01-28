using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public class TextureFactory : ITextureFactory
    {
        private ITexture2D whiteTexture;

        private IGraphicsFactory factory;
        private IGraphicsDevice device;
        private List<Texture2DParameters> files = new List<Texture2DParameters>();
        private List<TextureCubeParameters> cubeFiles = new List<TextureCubeParameters>();

        public TextureFactory(IGraphicsDevice device, IGraphicsFactory factory)
        {
            this.device = device;
            this.factory = factory;
        }

        public ITexture2D CreateFromFile(string file)
        {
            Texture2DParameters result = files.Find(delegate(Texture2DParameters item) { return file == item.Name; });
            if (result != null)
            {
                return result.Texture;
            }
            ITexture2D texture = factory.Texture2DFromFile(file);
            files.Add(new Texture2DParameters(file, texture));
            return texture;
        }

        public ITextureCube CreateCubeFromFile(string file)
        {
            TextureCubeParameters result = cubeFiles.Find(delegate(TextureCubeParameters item) { return file == item.Name; });
            if (result != null)
            {
                return result.Texture;
            }
            ITextureCube texture = factory.TextureCubeFromFile(file);
            cubeFiles.Add(new TextureCubeParameters(file, texture));
            return texture;
        }

        private IRenderTarget2D CreateRenderTarget(int width, int height)
        {
            return factory.CreateRenderTarget2D(width, height, 1, SurfaceFormat.Color, MultiSampleType.None, 0);
        }

        private IRenderTarget2D CreateRenderTarget(int width, int height, SurfaceFormat format, MultiSampleType multiSampleType, int multiSampleQuality)
        {
            return factory.CreateRenderTarget2D(width, height, 1, format, multiSampleType, multiSampleQuality);
        }

        public IRenderTarget2D CreateFullsizeRenderTarget()
        {
            return this.CreateRenderTarget(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);
        }

        public IRenderTarget2D CreateFullsizeRenderTarget(SurfaceFormat format, MultiSampleType multiSampleType, int multiSampleQuality)
        {
            return this.CreateRenderTarget(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight,
                format, multiSampleType, multiSampleQuality);
        }

        public IDepthStencilBuffer CreateFullsizeDepthStencil(DepthFormat format, MultiSampleType multiSampleType)
        {
            return this.CreateDepthStencil(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight,
                format, multiSampleType);
        }

        private IDepthStencilBuffer CreateDepthStencil(int width, int height, DepthFormat format, MultiSampleType multiSampleType)
        {
            return factory.CreateDepthStencilBuffer(width, height, format, multiSampleType);
        }

        public ITexture2D CreateFromFunction(int width, int height, int numLevels, TextureUsage usage, SurfaceFormat format, Fill2DTextureCallback callbackFunction)
        {
            Color[] data = new Color[width * height];
            int i = 0;
            Vector2 texelSize = new Vector2(1 / (float)(width - 1), 1 / (float)(height - 1));
            for (int y = 0; y < height; y++)
            {
                float yPos = y * texelSize.Y;
                for (int x = 0; x < width; x++)
                {
                    float xPos = x * texelSize.X;
                    data[i++] = new Color(callbackFunction(new Vector2(xPos, yPos), texelSize));
                }
            }

            ITexture2D texture = factory.CreateTexture2D(width, height, numLevels, usage, format);
            texture.SetData<Color>(data);

            int levelWidth = width;
            int levelHeight = height;
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
                texture.SetData<Color>(j + 1, null, newData, 0, newData.Length, SetDataOptions.None);
                data = newData;
            }

            //texture.Save("after.dds", ImageFileFormat.Dds);
            return texture;
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

        public void RegisterTexture(string name, ITexture2D texture)
        {
            files.Add(new Texture2DParameters(name, texture));
        }

        public ITexture2D WhiteTexture
        {
            get
            {
                if (whiteTexture == null)
                    whiteTexture = CreateFromFunction(1, 1, 1, TextureUsage.None, SurfaceFormat.Color, delegate(Vector2 x, Vector2 y) { return Vector4.One; });
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
