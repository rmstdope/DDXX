using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoFramework
{
    public class UserInterface
    {
        private ILine line;
        private ISprite sprite;
        private ITexture whiteTexture;

        internal UserInterface()
        {
        }

        internal void Initialize()
        {
            IDevice device = D3DDriver.GetInstance().GetDevice();
            line = D3DDriver.Factory.CreateLine(device);
            sprite = D3DDriver.Factory.CreateSprite(device);
            whiteTexture = D3DDriver.Factory.CreateTexture(device, 1, 1, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
            //device.ColorFill(whiteTexture.GetSurfaceLevel(0), new Rectangle(0, 0, 1, 1), Color.White);
        }


        internal void DrawBox(float x1, float y1, float x2, float y2, float alpha, Color color)
        {
            //float width = D3DDriver.GetInstance().GetDevice().PresentationParameters.BackBufferWidth;
            //float height = D3DDriver.GetInstance().GetDevice().PresentationParameters.BackBufferHeight;

            sprite.Begin(SpriteFlags.AlphaBlend);
            //sprite.Draw2D(whiteTexture, Rectangle.Empty, new SizeF(width * (x2 - x1), height * (y2 - y1)), new PointF(x1, y1), Color.FromArgb((int)(255 * alpha), color));
            sprite.End();
        }
    }
}
