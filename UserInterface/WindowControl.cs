using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.UserInterface
{
    public class WindowControl : BaseControl
    {
        private readonly Positioning positioning;
        private Color boxColor;
        protected readonly Color shadowColor = Color.Black;
        protected readonly Color outlineColor = Color.White;
        private Color textColor;
        private Color selectedTextColor;
        private readonly float screenWidth;
        private readonly float screenHeight;
        private readonly ISpriteFont titleFont;
        private readonly ISpriteFont subTitleFont;
        protected readonly ISpriteFont textFont;
        private string title;
        private string subTitle;
        private Vector2 titleTextSize;
        private Vector2 subTitleTextSize;
        private Vector2 combinedTitleTextSize;
        private Vector2 drawSize;
        private ITexture2D texture;
        private bool fixedWindow;

        public Color SelectedTextColor
        {
            get { return selectedTextColor; }
            set { selectedTextColor = value; Alpha = Alpha; }
        }

        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; Alpha = Alpha; }

        }
        protected Vector2 DrawSize
        {
            get { return drawSize; }
            set { drawSize = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; CalculateTitleSize(); }
        }

        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = value; CalculateTitleSize(); }
        }

        public byte Alpha
        {
            get { return boxColor.A; }
            set 
            {
                boxColor = new Color(boxColor.R, boxColor.G, boxColor.B, value);
                textColor = new Color(textColor.R, textColor.G, textColor.B, value);
                selectedTextColor = new Color(selectedTextColor.R, selectedTextColor.G, selectedTextColor.B, value);
            }
        }

        public ITexture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public WindowControl(Vector2 position, Positioning positioning, byte alpha, 
            IDrawResources resources, BaseControl parent, FontSize fontSize)
            : base(new Vector4(position, 0, 0), parent)
        {
            this.textColor = Color.White;
            this.selectedTextColor = Color.Red;
            this.positioning = positioning;
            this.fixedWindow = false;
            boxColor = new Color(Color.Teal.R, Color.Teal.G, Color.Teal.B, alpha);
            screenWidth = resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenHeight = resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;
            titleFont = resources.GetSpriteFont(FontSize.Large);
            subTitleFont = resources.GetSpriteFont(FontSize.Medium);
            textFont = resources.GetSpriteFont(fontSize);
            title = "";
            subTitle = "";
            CalculateTitleSize();
        }

        public WindowControl(Vector4 rectangle, byte alpha,
            IDrawResources resources, BaseControl parent, FontSize fontSize)
            : base(rectangle, parent)
        {
            this.textColor = Color.White;
            this.selectedTextColor = Color.Red;
            this.fixedWindow = true;
            boxColor = new Color(Color.Teal.R, Color.Teal.G, Color.Teal.B, alpha);
            screenWidth = resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenHeight = resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;
            titleFont = resources.GetSpriteFont(FontSize.Large);
            subTitleFont = resources.GetSpriteFont(FontSize.Medium);
            textFont = resources.GetSpriteFont(fontSize);
            title = "";
            subTitle = "";
            CalculateTitleSize();
        }

        private void CalculateTitleSize()
        {
            combinedTitleTextSize = Vector2.Zero;
            if (title != "")
                titleTextSize = titleFont.MeasureString(title);
            else
                titleTextSize = Vector2.Zero;
            if (subTitle != "")
                subTitleTextSize = subTitleFont.MeasureString(subTitle);
            else
                subTitleTextSize = Vector2.Zero;
            if (subTitleTextSize != Vector2.Zero || titleTextSize != Vector2.Zero)
            {
                combinedTitleTextSize.X = Math.Max(titleTextSize.X, subTitleTextSize.X) + 10;
                combinedTitleTextSize.Y = titleTextSize.Y + subTitleTextSize.Y + 10;
            }
        }

        public override void Draw(IDrawResources resources)
        {

            int x1;
            int y1;
            int width;
            int height;
            if (fixedWindow)
            {
                x1 = (int)(screenWidth * GetX1(resources));
                y1 = (int)(screenHeight * GetY1(resources));
                width = (int)(screenWidth * GetWidth(resources));
                height = (int)(screenHeight * GetHeight(resources));
            }
            else
            {
                x1 = GetWindowX1(resources);
                y1 = GetWindowY1(resources);
                width = GetWindowWidth();
                height = GetWindowHeight();
            }

            resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            if (Texture != null)
                resources.SpriteBatch.Draw(Texture, new Rectangle(x1, y1, width, height), new Color(255, 255, 255, 255));
            else
                resources.SpriteBatch.Draw(resources.WhiteTexture, new Rectangle(x1, y1, width, height), boxColor);
            resources.SpriteBatch.End();

            resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            DrawHorizontalLine(resources.SpriteBatch, resources.WhiteTexture, x1 + 1, y1 + 1, width, shadowColor);
            DrawHorizontalLine(resources.SpriteBatch, resources.WhiteTexture, x1 + 1, y1 + 1 + height, width, shadowColor);
            DrawVerticalLine(resources.SpriteBatch, resources.WhiteTexture, x1 + 1, y1 + 1, height, shadowColor);
            DrawVerticalLine(resources.SpriteBatch, resources.WhiteTexture, x1 + 1 + width, y1 + 1, height, shadowColor);

            DrawHorizontalLine(resources.SpriteBatch, resources.WhiteTexture, x1, y1 + GetTitleHeight(), width, outlineColor);
            DrawHorizontalLine(resources.SpriteBatch, resources.WhiteTexture, x1, y1, width, outlineColor);
            DrawHorizontalLine(resources.SpriteBatch, resources.WhiteTexture, x1, y1 + height, width, outlineColor);
            DrawVerticalLine(resources.SpriteBatch, resources.WhiteTexture, x1, y1, height, outlineColor);
            DrawVerticalLine(resources.SpriteBatch, resources.WhiteTexture, x1 + width, y1, height, outlineColor);
            resources.SpriteBatch.End();

            Vector2 pos = new Vector2(x1 + (width - titleTextSize.X) / 2, 
                                      y1 + 10 / 2);
            if (title != "")
            {
                resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
                resources.SpriteBatch.DrawString(titleFont, title, pos + new Vector2(1, 1), shadowColor);
                resources.SpriteBatch.DrawString(titleFont, title, pos, textColor);
                resources.SpriteBatch.End();
            }
            if (subTitle != "")
            {
                pos.X = x1 + (width - subTitleTextSize.X) / 2;
                pos.Y += titleTextSize.Y;
                resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
                resources.SpriteBatch.DrawString(subTitleFont, subTitle, pos + new Vector2(1, 1), shadowColor);
                resources.SpriteBatch.DrawString(subTitleFont, subTitle, pos, textColor);
                resources.SpriteBatch.End();
            }
        }

        private int GetWindowY1(IDrawResources resources)
        {
            if (fixedWindow)
            {
                return (int)(screenHeight * GetY1(resources));
            }
            else
            {
                if ((positioning & Positioning.VerticalCenter) == Positioning.VerticalCenter)
                    return (int)(screenHeight * GetY1(resources)) - GetWindowHeight() / 2;
                if ((positioning & Positioning.Top) == Positioning.Top)
                    return (int)(screenHeight * GetY1(resources));
                return (int)(screenHeight * GetY1(resources)) - GetWindowHeight();
            }
        }

        private int GetWindowX1(IDrawResources resources)
        {
            if (fixedWindow)
            {
                return (int)(screenWidth * GetX1(resources));
            }
            else
            {
                if ((positioning & Positioning.Center) == Positioning.Center)
                    return (int)(screenWidth * GetX1(resources)) - GetWindowWidth() / 2;
                if ((positioning & Positioning.Left) == Positioning.Left)
                    return (int)(screenWidth * GetX1(resources));
                return (int)(screenWidth * GetX1(resources)) - GetWindowWidth();
            }
        }

        private int GetWindowHeight()
        {
            return (int)(combinedTitleTextSize.Y + drawSize.Y);
        }

        private int GetWindowWidth()
        {
            return (int)(Math.Max(combinedTitleTextSize.X, drawSize.X));
        }

        private int GetTitleHeight()
        {
            return (int)(combinedTitleTextSize.Y);
        }

        protected int GetDrawX1(IDrawResources resources)
        {
            return GetWindowX1(resources);
        }

        protected int GetDrawY1(IDrawResources resources)
        {
            return GetWindowY1(resources) + GetTitleHeight();
        }

    }
}
