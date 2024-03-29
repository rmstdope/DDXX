using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Dope.DDXX.DemoFramework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoTweaker
{
    public class TweakerSettings : ITweakerSettings
    {
        private byte alpha;
        private byte textAlpha;
        private Color titleColor;
        private Color timeColor;
        private Color selectedColor;
        private Color unselectedColor;
        private ColorSchema colorSchema;
        private Keys screenshotKey;
        private Keys regenerateKey;

        public Keys RegenerateKey
        {
            get { return regenerateKey; }
            set { regenerateKey = value; }
        }

        public Keys ScreenshotKey
        {
            get { return screenshotKey; }
            set { screenshotKey = value; }
        }

        public Color UnselectedColor
        {
            get { return unselectedColor; }
            set { unselectedColor = value; }
        }

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        public Color TimeColor
        {
            get { return timeColor; }
            set { timeColor = value; }
        }

        public Color TitleColor
        {
            get { return titleColor; }
            set { titleColor = value; }
        }

        public byte TextAlpha
        {
            get { return textAlpha; }
        }
        public byte Alpha
        {
            get { return alpha; }
        }

        public TweakerSettings()
        {
            SetTransparency(Transparency.High);
            SetColorSchema(ColorSchema.Blue);
            ScreenshotKey = Keys.F11;
            RegenerateKey = Keys.F12;
        }

        private void SetColorSchema(ColorSchema colorSchema)
        {
            this.colorSchema = colorSchema;
            switch (colorSchema)
            {
                case ColorSchema.Blue:
                    titleColor = Color.Blue;
                    timeColor = Color.BlueViolet;
                    selectedColor = Color.LightBlue;
                    selectedColor = Color.DarkBlue;
                    break;
                case ColorSchema.Gray:
                    titleColor = new Color(50, 50, 50);
                    timeColor = new Color(70, 70, 70);
                    selectedColor = new Color(100, 100, 100);
                    unselectedColor = new Color(50, 50, 50);
                    break;
                case ColorSchema.Original:
                    titleColor = Color.Aquamarine;
                    timeColor = Color.BurlyWood;
                    selectedColor = Color.Crimson;
                    unselectedColor = Color.DarkBlue;
                    break;
            }
        }

        public void SetTransparency(Transparency transparency)
        {
            switch (transparency)
            {
                case Transparency.High:
                    alpha = 40;
                    textAlpha = 120;
                    break;
                case Transparency.Medium:
                    alpha = 80;
                    textAlpha = 200;
                    break;
                case Transparency.Low:
                    alpha = 128;
                    textAlpha = 255;
                    break;
            }
        }

        public void NextColorSchema()
        {
            ColorSchema nextColorSchema = colorSchema;
            switch (colorSchema)
            {
                case ColorSchema.Blue:
                    nextColorSchema = ColorSchema.Gray;
                    break;
                case ColorSchema.Gray:
                    nextColorSchema = ColorSchema.Original;
                    break;
                case ColorSchema.Original:
                    nextColorSchema = ColorSchema.Blue;
                    break;
            }
            SetColorSchema(nextColorSchema);
        }

        public void PreviousColorSchema()
        {
            ColorSchema nextColorSchema = colorSchema;
            switch (colorSchema)
            {
                case ColorSchema.Blue:
                    nextColorSchema = ColorSchema.Original;
                    break;
                case ColorSchema.Original:
                    nextColorSchema = ColorSchema.Gray;
                    break;
                case ColorSchema.Gray:
                    nextColorSchema = ColorSchema.Blue;
                    break;
            }
            SetColorSchema(nextColorSchema);
        }

    }
}
