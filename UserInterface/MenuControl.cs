using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.UserInterface
{
    public class MenuControl<T> : WindowControl
    {
        private readonly ISpriteFont font;
        private List<string> optionTexts;
        private List<T> optionActions;
        private int selected;

        public int Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public T Action
        {
            get 
            {
                if (selected < optionActions.Count)
                    return optionActions[selected];
                else
                    return default(T);
            }
        }

        public MenuControl(Vector2 position, Positioning positioning, byte alpha, IDrawResources resources, BaseControl parent)
            : base(position, positioning, alpha, resources, parent)
        {
            optionTexts = new List<string>();
            optionActions = new List<T>();
            font = resources.GetSpriteFont(FontSize.Medium);
            DrawSize = new Vector2(0, 10);
            selected = 0;
        }

        public void Next()
        {
            if (selected == optionTexts.Count - 1)
                selected = 0;
            else
                selected = selected + 1;
        }

        public void Previous()
        {
            if (selected == 0)
                selected = optionTexts.Count - 1;
            else
                selected = selected - 1;
        }

        public void AddOption(string text, T action)
        {
            optionTexts.Add(text);
            optionActions.Add(action);
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
                resources.SpriteBatch.DrawString(font, text, pos, num++ == selected ? SelectedTextColor : TextColor);
                pos.Y += font.MeasureString(text).Y;
            }
            resources.SpriteBatch.End();
        }

        public void ClearOptions()
        {
            optionTexts.Clear();
            optionActions.Clear();
            selected = 0;
            DrawSize = new Vector2(0, 10);
        }

        public int NumOptions { get { return optionTexts.Count; } }

    }
}
