using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.UserInterface
{
    public class ScrollListControl : WindowControl, IScrollListControl
    {
        private int maxNum;
        private int numShown;
        private int charsInWidth;
        private List<string> allText;
        private List<string> shownText;

        public ScrollListControl(Vector2 position, float widthPercent,
            Positioning positioning, byte alpha, IDrawResources resources, 
            BaseControl parent, int maxNum, int numShown, FontSize fontSize)
            : base(position, positioning, alpha, resources, parent, fontSize)
        {
            this.numShown = numShown;
            this.maxNum = maxNum;
            allText = new List<string>();
            shownText = new List<string>();
            int width = (int)(widthPercent * resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth);
            int height = (int)(textFont.MeasureString("Tg").Y) * numShown + 10;
            charsInWidth = width / (int)(textFont.MeasureString("D").X);
            DrawSize = new Vector2(width, height);
        }

        public virtual void AddText(string text)
        {
            if (text.Length > charsInWidth)
            {
                AddText(text.Substring(0, charsInWidth));
                AddText(text.Substring(charsInWidth, text.Length - charsInWidth));
            }
            else
            {
                allText.Add(text);
                shownText.Add(text);
                if (shownText.Count > numShown)
                    shownText.RemoveAt(0);
            }
        }

        public override void Draw(IDrawResources resources)
        {
            base.Draw(resources);
            Vector2 pos = new Vector2(GetDrawX1(resources) + 5, GetDrawY1(resources) + 5);

            resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            foreach (string text in shownText)
            {
                resources.SpriteBatch.DrawString(textFont, text, pos + new Vector2(1, 1), shadowColor);
                resources.SpriteBatch.DrawString(textFont, text, pos, TextColor);
                pos.Y += textFont.MeasureString(text).Y;
            }
            resources.SpriteBatch.End();
        }

    }
}
