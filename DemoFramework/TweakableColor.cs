using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;
using Dope.DDXX.UserInterface;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableColor : TweakableValueBase<Color>
    {
        public TweakableColor(PropertyInfo property, object target, ITweakableFactory factory)
            : base(property, target, factory)
        {
        }

        protected override void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h, ITweakerSettings settings)
        {
            new TextControl("R: " + Value.R.ToString(), new Vector4(x + 0 * w / 5, y, w / 5, h), Positioning.Center | Positioning.VerticalCenter, settings.TextAlpha, GetTextColor(status, index, 0), status.RootControl);
            new TextControl("G: " + Value.G.ToString(), new Vector4(x + 1 * w / 5, y, w / 5, h), Positioning.Center | Positioning.VerticalCenter, settings.TextAlpha, GetTextColor(status, index, 1), status.RootControl);
            new TextControl("B: " + Value.B.ToString(), new Vector4(x + 2 * w / 5, y, w / 5, h), Positioning.Center | Positioning.VerticalCenter, settings.TextAlpha, GetTextColor(status, index, 2), status.RootControl);
            new TextControl("A: " + Value.A.ToString(), new Vector4(x + 3 * w / 5, y, w / 5, h), Positioning.Center | Positioning.VerticalCenter, settings.TextAlpha, GetTextColor(status, index, 3), status.RootControl);
            Color alphaColor = new Color(Value.A, Value.A, Value.A, Value.A);
            new BoxControl(new Vector4(x + 4 * w / 5 + 0 * w / 10, y, w / 10, h), 255, Value, status.RootControl);
            new BoxControl(new Vector4(x + 4 * w / 5 + 1 * w / 10, y, w / 10, h), 255, alphaColor, status.RootControl);
        }

        public override int Dimension
        {
            get { return 4; }
        }

        protected override void ChangeValue(int index, float delta)
        {
            Value = new Color(index == 0 ? (byte)Math.Max(0, Math.Min(Value.R + delta, 255)) : Value.R,
                index == 1 ? (byte)Math.Max(0, Math.Min(Value.G + delta, 255)) : Value.G,
                index == 2 ? (byte)Math.Max(0, Math.Min(Value.B + delta, 255)) : Value.B,
                index == 3 ? (byte)Math.Max(0, Math.Min(Value.A + delta, 255)) : Value.A);
        }

        public override void SetFromString(int index, string value)
        {
            byte byteValue = 0;
            try
            {
                byteValue = byte.Parse(value, System.Globalization.NumberStyles.Any);
            }
            catch (Exception e) { }
            Value = new Color(index == 0 ? byteValue : Value.R,
                              index == 1 ? byteValue : Value.G,
                              index == 2 ? byteValue : Value.B,
                              index == 3 ? byteValue : Value.A);
        }

        protected override string GetToString(int index)
        {
            switch (index)
            {
                case 0:
                    return Value.R.ToString();
                case 1:
                    return Value.G.ToString();
                case 2:
                    return Value.B.ToString();
                case 3:
                    return Value.A.ToString();
            }
            throw new DDXXException("Invalid index.");
        }

    }
}
