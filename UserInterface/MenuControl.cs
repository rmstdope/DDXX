using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.UserInterface
{
    public class MenuControl : BaseControl
    {
        private int selected;
        private string[] options;
        private TextControl textControl;
        private BoxControl boxControl;

        public MenuControl(Vector4 rectangle, string[] options, byte boxAlpha, Color boxColor, Color textColor, BaseControl parent)
            : base(rectangle, parent)
        {
            boxControl = new BoxControl(new Vector4(0, 0, 1, 1), boxAlpha, boxColor, this);
            textControl = new TextControl("", new Vector2(0.03f, 0.5f), TextFormatting.Left | TextFormatting.VerticalCenter, 255, textColor, this);
            this.options = options;
            Selected = 0;
        }

        public Color BoxColor
        {
            get
            {
                return boxControl.Color;
            }
            set
            {
                boxControl.Color = value;
            }
        }

        public int Selected
        {
            set
            {
                selected = value;
                string sumString = "";
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selected)
                        sumString += ">";
                    sumString += options[i];
                    if (i != options.Length - 1)
                        sumString += "\n";
                }
                textControl.Text = sumString;
            }
            get
            {
                return selected;
            }
        }

        public void Next()
        {
            if (selected == options.Length - 1)
                Selected = 0;
            else
                Selected = selected + 1;
        }

        public void Previous()
        {
            if (selected == 0)
                Selected = options.Length - 1;
            else
                Selected = selected - 1;
        }

        public string[] Options
        {
            get
            {
                return options;
            }
            set
            {
                options = value;
                Selected = 0;
            }
        }

        public override void Draw(IDrawResources resources)
        {
            float x = resources.AspectRatio;
        }

    }
}
