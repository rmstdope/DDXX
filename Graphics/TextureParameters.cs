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
        private ITexture2D texture;
        private ITextureGenerator generator;

        public Texture2DParameters(string fileName, ITexture2D texture)
            : base(fileName)
        {
            this.texture = texture;
        }

        public Texture2DParameters(string name, ITexture2D texture, ITextureGenerator generator)
            : base(name)
        {
            this.texture = texture;
            this.generator = generator;
        }

        public ITexture2D Texture
        {
            get { return texture; }
        }

        public ITextureGenerator Generator
        {
            get { return generator; }
        }

        public bool IsGenerated
        {
            get { return generator != null; }
        }

        public void Regenerate()
        {
            if (!IsGenerated)
                return;

            Color[] data = new Color[texture.Width * texture.Height];
            int i = 0;
            Vector2 texelSize = new Vector2(1 / (float)(texture.Width - 1), 1 / (float)(texture.Height - 1));
            for (int y = 0; y < texture.Height; y++)
            {
                float yPos = y * texelSize.Y;
                for (int x = 0; x < texture.Width; x++)
                {
                    float xPos = x * texelSize.X;
                    data[i++] = new Color(generator.GetPixel(new Vector2(xPos, yPos), texelSize));
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
                texture.SetData<Color>(j + 1, null, newData, 0, newData.Length, SetDataOptions.None);
                data = newData;
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
        private ITextureCube texture;

        public TextureCubeParameters(string fileName, ITextureCube texture)
            : base(fileName)
        {
            this.texture = texture;
        }

        public ITextureCube Texture
        {
            get { return texture; }
        }

    }
}
