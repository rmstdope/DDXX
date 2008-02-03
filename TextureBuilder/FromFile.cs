using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.TextureBuilder
{
    public class FromFile : Generator
    {
        private ITextureFactory textureFactory;
        private string filename;
        private Color[] data;
        private int width;
        private int height;

        public ITextureFactory TextureFactory
        {
            get { return textureFactory; }
            set { textureFactory = value; }
        }

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public FromFile()
            : base(0)
        {
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            if (data == null)
            {
                ITexture2D texture = textureFactory.CreateFromName(filename);
                width = texture.Width;
                height = texture.Height;
                data = new Color[width * height];
                texture.GetData<Color>(data);
            }
            float x = textureCoordinate.X * (width - 1);
            float y = textureCoordinate.Y * (height - 1);
            int x1 = (int)x;
            int y1 = (int)y;
            float fracX = x - x1;
            float fracY = y - y1;
            Vector4 v1 = GetData(x1, y1, width, height, data);
            Vector4 v2 = GetData(x1 + 1, y1, width, height, data);
            Vector4 v3 = GetData(x1, y1 + 1, width, height, data);
            Vector4 v4 = GetData(x1 + 1, y1 + 1, width, height, data);
            return Vector4.Lerp(Vector4.Lerp(v1, v2, fracX), Vector4.Lerp(v3, v4, fracX), fracY);
        }

        private Vector4 GetData(int x, int y, int width, int height, Color[] data)
        {
            if (x >= width)
                x = width - 1;
            if (y >= height)
                y = height - 1;
            return data[y * width + x].ToVector4();
        }
    }
}
