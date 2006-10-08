using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweaker
    {
        private ISprite sprite;
        private bool enabled;
        //private ITexture texture;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public DemoTweaker()
        {
            enabled = false;
        }

        internal void Initialize()
        {
            IDevice device = D3DDriver.GetInstance().GetDevice();
            sprite = D3DDriver.Factory.CreateSprite(device);

            //texture = D3DDriver.Factory.CreateTexture(device, 1, 1, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
            //device.ColorFill(texture.GetSurfaceLevel(0), new Rectangle(0, 0, 1, 1), Color.Aqua);

        }

        internal void Draw()
        {
            if (!Enabled)
                return;

            sprite.Begin(SpriteFlags.AlphaBlend);
            //sprite.Draw2D(texture, Rectangle.Empty, new SizeF(400.0f, 400.0f), new PointF(0, 0), Color.FromArgb(80, Color.White));
            sprite.End();
        }
    }
}
