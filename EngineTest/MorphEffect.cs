using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace EngineTest
{
    public class MorphEffect : BaseDemoEffect
    {
        private class PixelMorph
        {
            private Vector3 startPos;
            private Vector3 endPos;
            private Color startColor;
            private Color endColor;
            private float delta;

            public PixelMorph(Vector3 startPos, Color startColor)
            {
                this.startPos = startPos;
                this.startColor = startColor;
                this.delta = 0;
            }

            public void SetEnd(Vector3 pos, Color color)
            {
                this.endPos = pos;
                this.endColor = color;
            }

            public void SetDelta(float delta)
            {
                this.delta = delta;
            }

            public Color GetColor()
            {
                return new Color(startColor.ToVector3() + (endColor.ToVector3() - startColor.ToVector3()) * delta);
            }

            public Vector3 GetPosition()
            {
                return startPos + (endPos - startPos) * delta;
            }
        }

        private ITexture2D[] textures;
        private List<PixelMorph> pixels = new List<PixelMorph>();
        private IVertexDeclaration vertexDeclaration;
        private VertexPositionColorPoint[] vertices;
        private IMaterialHandler material;
        
        public MorphEffect(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            textures = new ITexture2D[2];
            textures[0] = GraphicsFactory.Texture2DFromFile("Content/textures/boni");
            textures[1] = GraphicsFactory.Texture2DFromFile("Content/textures/dope");

            Color[] data = new Color[textures[0].Height * textures[0].Width];
            textures[0].GetData<Color>(data);
            for (int y = 0; y < textures[0].Height; y++)
            {
                for (int x = 0; x < textures[0].Width; x++)
                {
                    Color color = data[y * textures[0].Width + x];
                    if (color != Color.Black)
                    {
                        pixels.Add(new PixelMorph(new Vector3(x, y, 0), color));
                    }
                }
            }
            data = new Color[textures[1].Height * textures[1].Width];
            textures[1].GetData<Color>(data);
            int p = 0;// pixels.Count - 1;
            for (int y = 0; y < textures[1].Height; y++)
            {
                for (int x = 0; x < textures[1].Width; x++)
                {
                    Color color = data[y * textures[1].Width + x];
                    if (color != Color.Black)
                    {
                        if (p < pixels.Count)
                            pixels[p++].SetEnd(new Vector3(x, y, 0), color);
                    }
                }
            }
            vertexDeclaration = GraphicsFactory.CreateVertexDeclaration(VertexPositionColorPoint.VertexElements);
            vertices = new VertexPositionColorPoint[pixels.Count];
            for (int i = 0; i < pixels.Count; i++)
            {
                vertices[i] = new VertexPositionColorPoint();
            }

            material = new MaterialHandler(GraphicsFactory.EffectFromFile("Content\\effects\\Point"), new EffectConverter());
            material.BlendFunction = BlendFunction.Add;
            material.SourceBlend = Blend.One;
            material.DestinationBlend = Blend.InverseSourceColor;
            TextureDirector.CreateCircle(0.4f, 0.45f, 0.5f, 0.5f, new Vector2(0.5f, 0.5f));
            material.DiffuseTexture = TextureDirector.Generate("Circle64", 64, 64, 4, SurfaceFormat.Color);
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            float delta = Math.Min(1, EffectTime / 2.0f);
            float size = 1 + (float)Math.Sin(delta * MathHelper.Pi) * 8.0f;
            for (int i = 0; i < pixels.Count; i++)
            {
                pixels[i].SetDelta(delta);
                float x = (float)pixels[i].GetPosition().X;
                float y = (float)pixels[i].GetPosition().Y;
                vertices[i].Position = new Vector3(-1 + 2 * x / GraphicsDevice.PresentationParameters.BackBufferWidth,
                            1 - 2 * y / GraphicsDevice.PresentationParameters.BackBufferHeight, 0);
                vertices[i].Color = pixels[i].GetColor();
                vertices[i].PointSize = size;
            }
            material.SetupRendering(new Matrix[] { Matrix.Identity }, Matrix.Identity, Matrix.Identity, Color.White, new LightState());
            GraphicsDevice.VertexDeclaration = vertexDeclaration;
            material.Effect.Begin();
            foreach (IEffectPass pass in material.Effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColorPoint>(PrimitiveType.PointList, vertices, 0, pixels.Count);
                pass.End();
            }
            material.Effect.End();
        }
    }
}
