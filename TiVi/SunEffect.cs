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
            public float SizeDist;
            public Vector2 ScaleDist;
        }

        private ITexture circleTexture;
        private ISprite circleSprite;
        private BlitCircle[] circles;
        private Vector3 sunPosition;
        private int sunSize;

        public int SunSize
        {
            get { return sunSize; }
            set { sunSize = value; }
        }
        public Vector3 SunPosition
        {
            get { return sunPosition; }
            set { sunPosition = value; }
        }

        public SunEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            sunSize = 200;
            sunPosition = new Vector3(0.5f, 0.5f, 0);
            SetStepSize(GetTweakableNumber("SunPosition"), 0.01f);
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
                circles[i].SizeDist = rand.Next(1);
                circles[i].ScaleDist = new Vector2((float)(rand.NextDouble()), (float)(rand.NextDouble()));
            }
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
            Vector2 center = new Vector2(sunPosition.X * viewport.Width, sunPosition.Y * viewport.Height);
            Vector2 size = new Vector2(sunSize, sunSize);
            point = center - size * 0.5f;
            circleSprite.Begin(SpriteFlags.AlphaBlend);
            circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(size.X, size.Y),
                new PointF(point.X, point.Y), Color.White);
            circleSprite.End();

            circleSprite.Begin(SpriteFlags.AlphaBlend);
            for (int i = 0; i < NUM_CIRCLES; i++)
            {
                float x = circles[i].Offset.X + Time.CurrentTime / circles[i].Period.X;
                float y = circles[i].Offset.Y + Time.CurrentTime / circles[i].Period.Y;
                Vector2 distortion =
                    new Vector2(0.2f * sunSize * (1 + circles[i].ScaleDist.X) * (float)Math.Sin(x),
                                0.2f * sunSize * (1 + circles[i].ScaleDist.Y) * (float)Math.Cos(y));
                float s = (1 + circles[i].SizeDist) * sunSize / 10.0f;
                size = new Vector2(s, s);
                point = center - size * 0.5f + distortion;
                Device.RenderState.BlendOperation = BlendOperation.RevSubtract;
                Device.RenderState.SourceBlend = Blend.One;
                Device.RenderState.DestinationBlend = Blend.One;
                circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(size.X, size.Y),
                    new PointF(point.X, point.Y), Color.Gray);
            }
            circleSprite.End();
        }

    }
}
