using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.UserInterface
{
    public class ListControl : WindowControl
    {
        private List<string> optionTexts;

        public ListControl(Vector2 position, Positioning positioning, byte alpha, IDrawResources resources, BaseControl parent)
            : base(position, positioning, alpha, resources, parent)
        {
            this.optionTexts = new List<string>();
            DrawSize = new Vector2(0, 10);
        }

        public void AddText(string text)
        {
            optionTexts.Add(text);
            Vector2 textSize = textFont.MeasureString(text);
            DrawSize = new Vector2(Math.Max(DrawSize.X, textSize.X + 10), DrawSize.Y + textSize.Y);
        }

        public void ClearText()
        {
            optionTexts.Clear();
            DrawSize = new Vector2(0, 10);
        }

        public override void Draw(IDrawResources resources)
        {
            base.Draw(resources);
            Vector2 pos = new Vector2(GetDrawX1(resources) + 5, GetDrawY1(resources) + 5);

            resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            foreach (string text in optionTexts)
            {
                resources.SpriteBatch.DrawString(textFont, text, pos + new Vector2(1, 1), shadowColor);
                resources.SpriteBatch.DrawString(textFont, text, pos, TextColor);
                pos.Y += textFont.MeasureString(text).Y;
            }
            resources.SpriteBatch.End();
        }
    }
}
