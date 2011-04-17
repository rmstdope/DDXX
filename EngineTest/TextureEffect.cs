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
        private Texture2D texture;
        private SpriteBatch spriteBatch;

        public TextureEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            TextureDirector director = new TextureDirector(TextureFactory);
            // Base circle
            director.CreateCircle(0.0f, 0.75f, 1.5f, 0.5f, new Vector2(0.5f, 0.5f));
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
            director.Multiply();
            texture = director.Generate("Circle64", 64, 64, false, SurfaceFormat.Color);
            //texture = director.GenerateChain(512, 512);
#if !(XBOX360)
            //texture.Save("square-my.dds", ImageFileFormat.Dds);
#endif

            spriteBatch = new SpriteBatch(GraphicsFactory.GraphicsDevice);
        }

        public override void Step()
        {
            Mixer.SetClearColor(0, Color.SteelBlue);
        }

        public override void Render()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            for (int i = 0; i < 8; i++)
            {
                GraphicsDevice.SamplerStates[i].Filter = TextureFilter.Point;
            }
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
