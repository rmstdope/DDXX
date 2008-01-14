using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableColor : TweakableValueBase<Color>
    {
        public TweakableColor(PropertyInfo property, object target)
            : base(property, target)
        {
        }

        protected override void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h)
        {
            new TextControl("R: " + Value.R.ToString(), new Vector4(x + 0 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, GetAlpha(status, index), GetTextColor(status, index, 0), status.RootControl);
            new TextControl("G: " + Value.G.ToString(), new Vector4(x + 1 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, GetAlpha(status, index), GetTextColor(status, index, 1), status.RootControl);
            new TextControl("B: " + Value.B.ToString(), new Vector4(x + 2 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, GetAlpha(status, index), GetTextColor(status, index, 2), status.RootControl);
            new TextControl("A: " + Value.A.ToString(), new Vector4(x + 3 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, GetAlpha(status, index), GetTextColor(status, index, 3), status.RootControl);
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

        public override void SetFromInputString(TweakerStatus status)
        {
            byte byteValue = byte.Parse(status.InputString, System.Globalization.NumberFormatInfo.InvariantInfo);
            Value = new Color(status.Index == 0 ? byteValue : Value.R,
                              status.Index == 1 ? byteValue : Value.G,
                              status.Index == 2 ? byteValue : Value.B,
                              status.Index == 3 ? byteValue : Value.A);
        }
    }
}
