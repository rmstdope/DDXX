using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Dope.DDXX.ParticleSystems;
using Dope.DDXX.Graphics.Skinning;
using Dope.DDXX.MeshBuilder;

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

        private ITexture circleTexture;
        private ISprite circleSprite;
        private BlitCircle[] circles;

        public SunEffect(float startTime, float endTime)
            : base(startTime, endTime)
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
            circleTexture = TextureFactory.CreateFromFunction(512, 512, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, circleCallback);
            circleSprite = GraphicsFactory.CreateSprite(Device);
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

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            Viewport viewport = Device.Viewport;
            Vector2 point;
            Vector2 center = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            Vector2 size = new Vector2(200, 200);
            circleSprite.Begin(SpriteFlags.AlphaBlend);
            point = center - size * 0.5f;
            circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(size.X, size.Y),
                new PointF(point.X, point.Y), Color.White);
            for (int i = 0; i < NUM_CIRCLES; i++)
            {
                float x = circles[i].Offset.X + Time.CurrentTime / circles[i].Period.X;
                float y = circles[i].Offset.Y + Time.CurrentTime / circles[i].Period.Y;
                Vector2 distortion =
                    new Vector2(circles[i].Scale.X * (float)Math.Sin(x),
                                circles[i].Scale.Y * (float)Math.Cos(y));
                size = new Vector2(circles[i].Size, circles[i].Size);
                point = center - size * 0.5f + distortion;
                circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(size.X, size.Y),
                    new PointF(point.X, point.Y), Color.Black);
            }
            circleSprite.End();
        }

    }
}
