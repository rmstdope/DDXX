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
    public class WhiteFlare : BaseDemoEffect
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
        private float majorScale = 0.5f;

        public WhiteFlare(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        private Vector4 circleCallback(Vector2 texCoord, Vector2 texelSize)
        {
            Vector2 centered = texCoord - new Vector2(0.5f, 0.5f);
            float distance = centered.Length();
            if (distance < 0.4f)
                return new Vector4(1, 1, 1, 1);
            else if (distance < 0.5f)
            {
                float scaled = (0.5f - distance) / 0.1f;
                return new Vector4(scaled, scaled, scaled, 1);
            }
            return new Vector4(0, 0, 0, 0);
        }

        protected override void Initialize()
        {
            circleTexture = TextureFactory.CreateFromFunction(256, 256, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, circleCallback);
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
                circles[i].Scale = new Vector2(20 + 70 * (float)(rand.NextDouble()),
                    20 + 70 * (float)(rand.NextDouble()));
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
            float s = (float)Math.Sin((Time.StepTime - StartTime) / 10.0f);
            s = s * s * s;
            majorScale = s * 30;
            Viewport viewport = Device.Viewport;
            Vector2 point;
            Vector2 center = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            Vector2 size = new Vector2(200, 200);
            size *= majorScale;
            circleSprite.Begin(SpriteFlags.AlphaBlend);
            point = center - size * 0.5f;
            Device.RenderState.BlendOperation = BlendOperation.Add;
            Device.RenderState.SourceBlend = Blend.One;
            Device.RenderState.DestinationBlend = Blend.One;
            DrawCircle(circleTexture, new SizeF(size.X, size.Y),
                new PointF(point.X, point.Y), -1, Color.White);
            circleSprite.End();
            circleSprite.Begin(SpriteFlags.AlphaBlend);
            Device.RenderState.BlendOperation = BlendOperation.RevSubtract;
            Device.RenderState.SourceBlend = Blend.DestinationColor;
            Device.RenderState.DestinationBlend = Blend.One;
            Device.RenderState.AlphaTestEnable = false;
            for (int i = 0; i < NUM_CIRCLES; i++)
            {
                float x = circles[i].Offset.X + Time.CurrentTime / circles[i].Period.X;
                float y = circles[i].Offset.Y + Time.CurrentTime / circles[i].Period.Y;
                Vector2 distortion =
                    new Vector2(circles[i].Scale.X * (float)Math.Sin(x),
                                circles[i].Scale.Y * (float)Math.Cos(y));
                distortion *= majorScale;
                size = new Vector2(circles[i].Size, circles[i].Size);
                size *= majorScale;
                point = center - size * 0.5f + distortion;
                DrawCircle(circleTexture, new SizeF(size.X, size.Y), new PointF(point.X, point.Y), -1, Color.FromArgb(0, 40, 40, 40));
                //circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(size.X, size.Y),
                //    new PointF(point.X, point.Y), Color.Black);
            }
            circleSprite.End();
        }

        private void DrawCircle(ITexture texture, SizeF size, PointF pos, float z, Color color)
        {
            Matrix m = Matrix.Identity;
            m.M11 = size.Width / texture.GetLevelDescription(0).Width;
            m.M22 = size.Height / texture.GetLevelDescription(0).Height;
            circleSprite.Transform = m;
            float x = -pos.X / m.M11;
            float y = -pos.Y / m.M22;
            circleSprite.Draw(texture, new Vector3(x, y, -1.0f), new Vector3(0, 0, 0), color.ToArgb());
        }

    }
}
