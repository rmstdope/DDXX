using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace TiVi
{
    public class TextureViewer : BaseDemoEffect
    {
        private ISprite sprite;
        private List<ITexture> textures;
        private int textureIndex;
        private int waitCount;
        private int waited;
        private float targetWidth;
        private float targetHeight;

        public float TargetWidth
        {
            get { return targetWidth; }
            set { targetWidth = value; }
        }

        public float TargetHeight
        {
            get { return targetHeight; }
            set { targetHeight = value; }
        }

        public TextureViewer(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            textures = new List<ITexture>();
            waitCount = 1;
            waited = 0;
            targetWidth = -1;
            targetHeight = -1;
        }

        public int WaitCount
        {
            get { return waitCount; }
            set { waitCount = value; }
        }

        public string Texture
        {
            get { return "Save not supported"; }
            set { AddTexture(value); }
        }


        protected override void Initialize()
        {
            sprite = GraphicsFactory.CreateSprite(Device);
            if (targetWidth < 0)
            {
                targetWidth = Device.PresentationParameters.BackBufferWidth;
            }
            if (targetHeight < 0) {
                targetHeight = Device.PresentationParameters.BackBufferHeight;
            }
        }

        public void AddTexture(string filename)
        {
            textures.Add(TextureFactory.CreateFromFile(filename));
        }

        public override void Step()
        {
            if (--waited <= 0)
            {
                waited = waitCount;
                textureIndex++;
                if (textureIndex >= textures.Count)
                    textureIndex = 0;
            }
        }

        public override void Render()
        {
            sprite.Begin(SpriteFlags.None);
            sprite.Draw2D(textures[textureIndex], Rectangle.Empty, new SizeF(TargetWidth, TargetHeight), PointF.Empty, Color.White);
            sprite.End();
        }

        public void SetTargetWidthHeight(float x, float y)
        {
            TargetWidth = x;
            TargetHeight = y;
        }
    }
}
