using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EngineTest
{
    public class TextureEffect : BaseDemoEffect
    {
        private ITexture2D texture;
        private ISpriteBatch spriteBatch;

        public TextureEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            TextureDirector director = new TextureDirector(TextureFactory);
            // Base circle
            director.CreateCircle(0.0f, 1.5f);
            // Add brush noise
            director.CreateBrushNoise(3);
            director.ModulateColor(new Vector4(0.05f));
            director.Add();
            // Add perlin noise
            director.CreatePerlinNoise(512, 6, 0.5f);
            director.Madd(0.04f, 0);
            director.Add();
            // Create border
            director.CreateSquare(0.9f);
            director.GaussianBlur();
            director.Modulate();
            texture = director.Generate("Cirvle64", 64, 64, 1, SurfaceFormat.Color);
            //texture = director.GenerateChain(512, 512);
#if !(XBOX360)
            texture.Save("square-my.dds", ImageFileFormat.Dds);
#endif

            spriteBatch = GraphicsFactory.CreateSpriteBatch();
        }

        public override void Step()
        {
            Mixer.SetClearColor(0, Color.SteelBlue);
        }

        public override void Render()
        {
            spriteBatch.Begin(SpriteBlendMode.None);
            for (int i = 0; i < 8; i++)
            {
                GraphicsDevice.SamplerStates[i].MagFilter = TextureFilter.Point;
                GraphicsDevice.SamplerStates[i].MinFilter = TextureFilter.Point;
                GraphicsDevice.SamplerStates[i].MipFilter = TextureFilter.Point;
            }
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
