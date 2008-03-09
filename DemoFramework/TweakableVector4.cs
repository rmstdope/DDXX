using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableVector4 : TweakableValueBase<Vector4>
    {
        public TweakableVector4(PropertyInfo property, object target, ITweakableFactory factory)
            : base(property, target, factory)
        {
        }

        protected override void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h, ITweakerSettings settings)
        {
            new TextControl("X: " + Value.X.ToString("N3", System.Globalization.CultureInfo.InvariantCulture),
                new Vector4(x + 0 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter,
                settings.TextAlpha, GetTextColor(status, index, 0), status.RootControl);
            new TextControl("Y: " + Value.Y.ToString("N3", System.Globalization.CultureInfo.InvariantCulture),
                new Vector4(x + 1 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter,
                settings.TextAlpha, GetTextColor(status, index, 1), status.RootControl);
            new TextControl("Z: " + Value.Z.ToString("N3", System.Globalization.CultureInfo.InvariantCulture),
                new Vector4(x + 2 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter,
                settings.TextAlpha, GetTextColor(status, index, 2), status.RootControl);
            new TextControl("W: " + Value.W.ToString("N3", System.Globalization.CultureInfo.InvariantCulture),
                new Vector4(x + 3 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter,
                settings.TextAlpha, GetTextColor(status, index, 3), status.RootControl);
        }

        public override int Dimension
        {
            get { return 4; }
        }

        protected override void ChangeValue(int index, float delta)
        {
            Value += new Vector4(index == 0 ? delta : 0, index == 1 ? delta : 0, index == 2 ? delta : 0, index == 3 ? delta : 0);
        }

        public override void SetFromString(int index, string value)
        {
            float floatValue = float.Parse(value, System.Globalization.NumberFormatInfo.InvariantInfo);
            Value = new Vector4(index == 0 ? floatValue : Value.X,
                                index == 1 ? floatValue : Value.Y,
                                index == 2 ? floatValue : Value.Z,
                                index == 3 ? floatValue : Value.W);
        }

        protected override string GetToString(int index)
        {
            switch (index)
            {
                case 0:
                    return Value.X.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
                case 1:
                    return Value.Y.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
                case 2:
                    return Value.Z.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
                case 3:
                    return Value.W.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            throw new DDXXException("Invalid index.");
        }
    }
}
