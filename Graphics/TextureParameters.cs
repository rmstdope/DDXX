using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public class TextureParameters
    {
        private string name;

        public TextureParameters(string fileName)
        {
            this.name = fileName;
        }

        public string Name
        {
            get { return name; }
        }

    }

    public class Texture2DParameters : TextureParameters
    {
        private Texture2D texture;
        private GraphicsDevice graphicsDevice;
        public ITextureGenerator Generator;
        public int Width { get; set; }
        public int Height { get; set; }
        public bool MipMap { get; set; }
        public SurfaceFormat Format { get; set; }

        public Texture2DParameters(GraphicsDevice graphicsDevice, string fileName, Texture2D texture)
            : base(fileName)
        {
            this.graphicsDevice = graphicsDevice;
            this.texture = texture;
        }

        public Texture2DParameters(GraphicsDevice graphicsDevice, string name, Texture2D texture, ITextureGenerator generator)
            : base(name)
        {
            this.graphicsDevice = graphicsDevice;
            this.texture = null;
            Generator = generator;
            Width = texture.Width;
            Height = texture.Height;
            MipMap = texture.LevelCount > 1;
            Format = texture.Format;
            Regenerate();
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public bool IsGenerated
        {
            get { return Generator != null; }
        }

        public void Regenerate()
        {
            if (!IsGenerated)
                return;

            Vector4[,] pixels = Generator.GenerateTexture(Width, Height);
            Color[] data = new Color[Width * Height];
            int i = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    data[i++] = new Color(pixels[x, y]);
                }
            }

            VerifyTexture();
            texture.SetData<Color>(data);

            int levelWidth = Width;
            int levelHeight = Height;
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
        }

        private void VerifyTexture()
        {
            if (texture == null || texture.Width != Width || texture.Height != Height || 
                (texture.LevelCount == 1 && MipMap) || (texture.LevelCount > 1 && !MipMap) ||
                texture.Format != Format)
            {
                texture = new Texture2D(graphicsDevice, Width, Height, MipMap, Format);
            }
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
    }

    public class TextureCubeParameters : TextureParameters
    {
        private TextureCube texture;

        public TextureCubeParameters(string fileName, TextureCube texture)
            : base(fileName)
        {
            this.texture = texture;
        }

        public TextureCube Texture
        {
            get { return texture; }
        }

    }
}
