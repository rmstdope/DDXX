using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
//using Microsoft.DirectX.Direct3D;
using System.Drawing;
//using Microsoft.DirectX;
using Dope.DDXX.Utility;

namespace EngineTest
{
    /*public class TextWave : BaseDemoEffect
    {
        private IFont font;
        private ISprite sprite;
        private ITexture texture;
        private Rectangle size;
        private IEffect effect;

        public TextWave(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateFont();
            effect = EffectFactory.CreateFromFile("Test.fxo");
        }

        private void CreateFont()
        {
            FontDescription description = new FontDescription();
            description.FaceName = "Copperplate Gothic Bold";
            description.Height = 200;
            description.Weight = FontWeight.ExtraBold;
            font = GraphicsFactory.CreateFont(Device, description);
            sprite = GraphicsFactory.CreateSprite(Device);
            size = font.MeasureString(sprite, "Dope", DrawTextFormat.None, Color.AntiqueWhite);
            texture = GraphicsFactory.CreateTexture(Device, size.Width, size.Height, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
            using (ISurface original = Device.GetRenderTarget(0))
            {
                using (ISurface target = texture.GetSurfaceLevel(0))
                {
                    Device.SetRenderTarget(0, target);
                    Device.Clear(ClearFlags.Target, Color.FromArgb(0, Color.Black), 0, 0);
                    Device.BeginScene();
                    sprite.Begin(SpriteFlags.AlphaBlend);
                    font.DrawText(sprite, "Dope", 0, 0, Color.FromArgb(255, Color.White));
                    sprite.End();
                    Device.EndScene();
                }
                Device.SetRenderTarget(0, original);
            }
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            //sprite.Begin(SpriteFlags.AlphaBlend);
            //sprite.Draw2D(texture, Rectangle.Empty, SizeF.Empty, new PointF(205, 205), Color.Black);
            //sprite.Draw2D(texture, Rectangle.Empty, SizeF.Empty, new PointF(198, 198), Color.LightGray);
            //sprite.Draw2D(texture, Rectangle.Empty, SizeF.Empty, new PointF(200, 200), Color.White);
            //sprite.End();

            DrawText(3, 3, BlendOperation.RevSubtract);
            DrawText(0, 0, BlendOperation.Add);
        }

        private void DrawText(int x, int y, BlendOperation op)
        {
            int height = Device.PresentationParameters.BackBufferHeight;
            int width = Device.PresentationParameters.BackBufferWidth;
            CustomVertex.TransformedTextured[] vertices = new CustomVertex.TransformedTextured[4];
            vertices[0] = new CustomVertex.TransformedTextured(new Vector4(x - 0.5f, y - 0.5f, 1.0f, 1.0f), -1, -2);
            vertices[1] = new CustomVertex.TransformedTextured(new Vector4(x + width - 0.5f, y - 0.5f, 1.0f, 1.0f), 2, -1);
            vertices[2] = new CustomVertex.TransformedTextured(new Vector4(x - 0.5f, y + height - 0.5f, 1.0f, 1.0f), -1, 2);
            vertices[3] = new CustomVertex.TransformedTextured(new Vector4(x + width - 0.5f, y + height - 0.5f, 1.0f, 1.0f), 2, 2);
            Device.VertexFormat = CustomVertex.TransformedTextured.Format;
            effect.SetValue(EffectHandle.FromString("BaseTexture"), texture);
            effect.SetValue(EffectHandle.FromString("WaveTextTime"), Time.CurrentTime * 1 + x * 0.2f);
            effect.Technique = "WaveText";
            effect.Begin(FX.None);
            effect.BeginPass(0);
            Device.RenderState.AlphaBlendEnable = true;
            Device.RenderState.BlendOperation = op;
            Device.RenderState.SourceBlend = Blend.SourceAlpha;
            Device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
            Device.SamplerState[0].AddressU = TextureAddress.Clamp;
            Device.SamplerState[0].AddressV = TextureAddress.Clamp;
            Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, vertices);
            effect.EndPass();
            effect.End();
        }
    }*/
}
