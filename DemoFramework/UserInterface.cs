using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.DemoFramework
{
    public struct BoxControl
    {
        public float X1;
        public float Y1;
        public float X2;
        public float Y2;
        public float Alpha;
        public Color Color;
        public BoxControl(float x1, float y1, float x2, float y2, float alpha, Color color)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Alpha = alpha;
            Color = color;
        }
    }

    public struct TextControl
    {
        public string Text;
        public float X1;
        public float Y1;
        public float Alpha;
        public Color Color;
        public TextControl(string text, float x1, float y1, float alpha, Color color)
        {
            Text = text;
            X1 = x1;
            Y1 = y1;
            Alpha = alpha;
            Color = color;
        }
    }

    public class UserInterface
    {
        private ILine line;
        private ISprite sprite;
        private ITexture whiteTexture;
        private IFont font;

        internal UserInterface()
        {
        }

        internal void Initialize()
        {
            IDevice device = D3DDriver.GetInstance().GetDevice();

            line = D3DDriver.Factory.CreateLine(device);
            line.Width = 1.0f;
            line.Antialias = true;
            
            sprite = D3DDriver.Factory.CreateSprite(device);

            font = D3DDriver.Factory.CreateFont(device, 14, 0, FontWeight.Normal, 1, false, CharacterSet.Default, Precision.Default, FontQuality.Default, PitchAndFamily.DefaultPitch | PitchAndFamily.FamilyDoNotCare, "Courier");
            
            whiteTexture = D3DDriver.Factory.CreateTexture(device, 1, 1, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
            device.ColorFill(whiteTexture.GetSurfaceLevel(0), new Rectangle(0, 0, 1, 1), Color.White);
        }

        internal void DrawControl(BoxControl box)
        {
            float width = D3DDriver.GetInstance().GetDevice().PresentationParameters.BackBufferWidth;
            float height = D3DDriver.GetInstance().GetDevice().PresentationParameters.BackBufferHeight;

            sprite.Begin(SpriteFlags.AlphaBlend);
            sprite.Draw2D(whiteTexture, Rectangle.Empty, new SizeF(width * (box.X2 - box.X1), height * (box.Y2 - box.Y1)), new PointF(width * box.X1, height * box.Y1), Color.FromArgb((int)(255 * box.Alpha), box.Color));
            sprite.End();

            line.Begin();
            line.Draw(new Vector2[] { new Vector2(width * box.X1, height * box.Y1), new Vector2(width * box.X2, height * box.Y1), new Vector2(width * box.X2, height * box.Y2), new Vector2(width * box.X1, height * box.Y2), new Vector2(width * box.X1, height * box.Y1) }, Color.White);
            line.End();
        }

        internal void DrawControl(TextControl text)
        {
            float width = D3DDriver.GetInstance().GetDevice().PresentationParameters.BackBufferWidth;
            float height = D3DDriver.GetInstance().GetDevice().PresentationParameters.BackBufferHeight;

            font.DrawText(null, text.Text, (int)(width * text.X1), (int)(height * text.Y1), Color.FromArgb((int)(255 * text.Alpha), text.Color));
        }
    }
}
