using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoEffects
{
    public class SpinningBackgroundEffect : BaseDemoEffect
    {
        public class TextureLayer
        {
            public string TextureName;
            public Texture2D Texture;
            public float Period;
            public Color ColorModulation;
            public TextureLayer(string texture, float period, Color colorModulation)
            {
                TextureName = texture;
                Texture = null;
                Period = period;
                ColorModulation = colorModulation;
            }
        }

        private List<TextureLayer> textureLayers;
        private SpriteBatch sprite;

        public SpinningBackgroundEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            textureLayers = new List<TextureLayer>();
        }

        protected override void Initialize()
        {
            sprite = new SpriteBatch(GraphicsDevice);

            foreach (TextureLayer layer in textureLayers)
            {
                layer.Texture = TextureFactory.CreateFromName(layer.TextureName);
            }
        }

        public void AddTextureLayer(string texture, float period, Color colorModulation)
        {
            AddTextureLayer(new TextureLayer(texture, period, colorModulation));
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
            sprite.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            GraphicsDevice.DepthStencilState.DepthBufferEnable = false;
            foreach (TextureLayer layer in textureLayers)
            {
                float angle;
                int sWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
                int sHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
                int tWidth = layer.Texture.Width;
                int tHeight = layer.Texture.Height;
                angle = (2.0f * (float)Math.PI * Time.CurrentTime) / (layer.Period);
                sprite.Draw(layer.Texture, new Rectangle(0, 0, sWidth * 2, sHeight * 2), new Rectangle(0, 0, tWidth, tHeight),
                    layer.ColorModulation, angle, new Vector2(tWidth / 2.0f, tHeight / 2.0f), SpriteEffects.None, 1.0f);
            }            
            sprite.End();
        }

    }
}
