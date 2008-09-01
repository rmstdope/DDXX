﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.UserInterface
{
    public class WindowControl : BaseControl
    {
        private readonly Color boxColor;
        protected readonly Color shadowColor = Color.Black;
        protected readonly Color outlineColor = Color.White;
        protected readonly Color textColor = Color.Green;
        protected readonly Color selectedTextColor = Color.Red;
        private readonly float screenWidth;
        private readonly float screenHeight;
        private readonly ISpriteFont titleFont;
        private string title;
        private Vector2 titleTextSize;
        private Vector2 drawSize;

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

        public WindowControl(Vector2 position, byte alpha, IDrawResources resources, BaseControl parent)
            : base(new Vector4(position, 0, 0), parent)
        {
            boxColor = new Color(Color.Teal.R, Color.Teal.G, Color.Teal.B, alpha);
            screenWidth = resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenHeight = resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;
            titleFont = resources.GetSpriteFont(FontSize.Medium);
            title = "";
            CalculateTitleSize();
        }

        private void CalculateTitleSize()
        {
            titleTextSize = titleFont.MeasureString(title);
        }

        public override void Draw(IDrawResources resources)
        {
            int width = GetWindowWidth();
            int height = GetWindowHeight();
            int x1 = GetWindowX1(resources);
            int y1 = GetWindowY1(resources);

            resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
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

            Vector2 pos = new Vector2(x1 + (width - titleTextSize.X) / 2, y1 + (GetTitleHeight() - titleTextSize.Y) / 2);
            resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            resources.SpriteBatch.DrawString(titleFont, title, pos + new Vector2(1, 1), shadowColor);
            resources.SpriteBatch.DrawString(titleFont, title, pos, textColor);
            resources.SpriteBatch.End();
        }

        private int GetWindowY1(IDrawResources resources)
        {
            return (int)(screenHeight * GetY1(resources)) - GetWindowHeight() / 2;
        }

        private int GetWindowX1(IDrawResources resources)
        {
            return (int)(screenWidth * GetX1(resources)) - GetWindowWidth() / 2;
        }

        private int GetWindowHeight()
        {
            return (int)(titleTextSize.Y + 10 + drawSize.Y);
        }

        private int GetWindowWidth()
        {
            return (int)(Math.Max(titleTextSize.X + 10, drawSize.X));
        }

        private int GetTitleHeight()
        {
            return (int)(titleTextSize.Y + 10);
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