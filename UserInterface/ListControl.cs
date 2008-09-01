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
        private readonly ISpriteFont font;
        private List<string> optionTexts;

        public ListControl(Vector2 position, byte alpha, IDrawResources resources, BaseControl parent)
            : base(position, alpha, resources, parent)
        {
            optionTexts = new List<string>();
            font = resources.GetSpriteFont(FontSize.Medium);
            DrawSize = new Vector2(0, 10);
        }

        public void AddText(string text)
        {
            optionTexts.Add(text);
            Vector2 textSize = font.MeasureString(text);
            DrawSize = new Vector2(Math.Max(DrawSize.X, textSize.X + 10), DrawSize.Y + textSize.Y);
        }

        public override void Draw(IDrawResources resources)
        {
            base.Draw(resources);
            Vector2 pos = new Vector2(GetDrawX1(resources) + 5, GetDrawY1(resources) + 5);

            resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            int num = 0;
            foreach (string text in optionTexts)
            {
                resources.SpriteBatch.DrawString(font, text, pos + new Vector2(1, 1), shadowColor);
                resources.SpriteBatch.DrawString(font, text, pos, textColor);
                pos.Y += font.MeasureString(text).Y;
            }
            resources.SpriteBatch.End();
        }
    }
}
