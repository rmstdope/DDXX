using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoEffects
{
    public class SpinningBackgroundEffect : BaseDemoEffect
    {
        public class TextureLayer
        {
            public string TextureName;
            public ITexture Texture;
            public float Period;
            public Color ColorModulation;
            public TextureLayer(string texture, float period, Color colorModulation, float alpha)
            {
                TextureName = texture;
                Texture = null;
                Period = period;
                ColorModulation = Color.FromArgb((int)(255 * alpha), colorModulation);
            }
        }

        private List<TextureLayer> textureLayers;
        private ISprite sprite;

        public SpinningBackgroundEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
            textureLayers = new List<TextureLayer>();
        }

        protected override void Initialize()
        {
            sprite = D3DDriver.GraphicsFactory.CreateSprite(Device);

            foreach (TextureLayer layer in textureLayers)
            {
                layer.Texture = D3DDriver.TextureFactory.CreateFromFile(layer.TextureName);
            }
        }

        public void AddTextureLayer(string texture, float period, Color colorModulation, float alpha)
        {
            AddTextureLayer(new TextureLayer(texture, period, colorModulation, alpha));
        }

        public void AddTextureLayer(TextureLayer textureLayer)
        {
            textureLayers.Add(textureLayer);
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            sprite.Begin(SpriteFlags.AlphaBlend);
            Device.RenderState.ZBufferEnable = false;
            foreach (TextureLayer layer in textureLayers)
            {
                float angle;
                int sWidth = Device.PresentationParameters.BackBufferWidth;
                int sHeight = Device.PresentationParameters.BackBufferHeight;
                int tWidth;
                int tHeight;
                using (ISurface surface = layer.Texture.GetSurfaceLevel(0))
                {
                    tWidth = surface.Description.Width;
                    tHeight = surface.Description.Height;
                }
                angle = (2.0f * (float)Math.PI * Time.StepTime) / (layer.Period);
                sprite.Draw2D(layer.Texture, Rectangle.Empty, new SizeF(sWidth * 2.0f, sHeight * 2.0f), new PointF(tWidth / 2, tHeight / 2), angle, new PointF(sWidth / 2, sHeight / 2), layer.ColorModulation);
            }
            sprite.End();
        }

    }
}
