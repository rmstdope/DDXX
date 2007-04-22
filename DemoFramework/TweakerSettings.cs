using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{
    public class TweakerSettings : ITweakerSettings
    {
        private float alpha;
        private float textAlpha;
        private Color titleColor;
        private Color timeColor;
        private Color selectedColor;
        private Color unselectedColor;
        private ColorSchema colorSchema;

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

        public float TextAlpha
        {
            get { return textAlpha; }
        }
        public float Alpha
        {
            get { return alpha; }
        }

        public TweakerSettings()
        {
            SetTransparency(Transparency.High);
            SetColorSchema(ColorSchema.Blue);
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
                    titleColor = Color.FromArgb(50, 50, 50);
                    timeColor = Color.FromArgb(70, 70, 70);
                    selectedColor = Color.FromArgb(100, 100, 100);
                    unselectedColor = Color.FromArgb(50, 50, 50);
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
                    alpha = 0.15f;
                    textAlpha = 0.3f;
                    break;
                case Transparency.Medium:
                    alpha = 0.3f;
                    textAlpha = 0.5f;
                    break;
                case Transparency.Low:
                    alpha = 0.5f;
                    textAlpha = 0.8f;
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
