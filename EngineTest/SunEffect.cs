using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Dope.DDXX.ParticleSystems;
using Dope.DDXX.ModelBuilder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineTest
{
    public class SunEffect : BaseDemoEffect
    {
        private const int NUM_CIRCLES = 60;
        private struct BlitCircle
        {
            public Vector2 Offset;
            public Vector2 Period;
            public float Size;
            public Vector2 Scale;
        }

        private ITexture2D circleTexture;
        private ISpriteBatch circleSprite;
        private BlitCircle[] circles;

        public SunEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        private Vector4 circleCallback(Vector2 texCoord, Vector2 texelSize)
        {
            Vector2 centered = texCoord - new Vector2(0.5f, 0.5f);
            float distance = centered.Length();
            if (distance < 0.3f)
                return new Vector4(1, 1, 1, 1);
            else if (distance < 0.5f)
            {
                float scaled = (0.5f - distance) / 0.2f;
                return new Vector4(scaled, scaled, scaled, scaled);
            }
            return new Vector4(0, 0, 0, 0);
        }

        protected override void Initialize()
        {
            //circleTexture = TextureFactory.CreateFromFunction(512, 512, 0, TextureUsage.None, SurfaceFormat.Color, circleCallback);
            circleSprite = GraphicsFactory.CreateSpriteBatch();
            circles = new BlitCircle[NUM_CIRCLES];
            Random rand = new Random();
            for (int i = 0; i < NUM_CIRCLES; i++)
            {
                circles[i] = new BlitCircle();
                circles[i].Offset = new Vector2((float)(2 * Math.PI * rand.NextDouble()),
                    (float)(2 * Math.PI * rand.NextDouble()));
                circles[i].Period = new Vector2((float)(0.5f + 3 * rand.NextDouble()),
                    (float)(0.5f + 3 * rand.NextDouble()));
                circles[i].Size = 10 + rand.Next(20);
                circles[i].Scale = new Vector2(50 + 80 * (float)(rand.NextDouble()),
                    50 + 80 * (float)(rand.NextDouble()));
            }

            //circleTexture.Save("test.jpg", ImageFileFormat.Jpg);
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            Viewport viewport = GraphicsDevice.Viewport;
            Vector2 point;
            Vector2 center = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            Vector2 size = new Vector2(200, 200);
            circleSprite.Begin();
            point = center - size * 0.5f;
            circleSprite.Draw(circleTexture, new Rectangle((int)point.X, (int)point.Y, (int)size.X, (int)size.Y), Color.White);
            for (int i = 0; i < NUM_CIRCLES; i++)
            {
                float x = circles[i].Offset.X + Time.CurrentTime / circles[i].Period.X;
                float y = circles[i].Offset.Y + Time.CurrentTime / circles[i].Period.Y;
                Vector2 distortion =
                    new Vector2(circles[i].Scale.X * (float)Math.Sin(x),
                                circles[i].Scale.Y * (float)Math.Cos(y));
                size = new Vector2(circles[i].Size, circles[i].Size);
                point = center - size * 0.5f + distortion;
                circleSprite.Draw(circleTexture, new Rectangle((int)point.X, (int)point.Y, (int)size.X, (int)size.Y), Color.Black);
            }
            circleSprite.End();
        }

    }
}
