using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableVector3 : TweakableValueBase<Vector3>
    {
        public TweakableVector3(PropertyInfo property, object target)
            : base(property, target)
        {
        }

        protected override void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h)
        {
            new TextControl("X: " + Value.X.ToString("N3", System.Globalization.CultureInfo.InvariantCulture),
                new Vector4(x + 0 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter,
                GetAlpha(status, index), GetTextColor(status, index, 0), status.RootControl);
            new TextControl("Y: " + Value.Y.ToString("N3", System.Globalization.CultureInfo.InvariantCulture),
                new Vector4(x + 1 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter,
                GetAlpha(status, index), GetTextColor(status, index, 1), status.RootControl);
            new TextControl("Z: " + Value.Z.ToString("N3", System.Globalization.CultureInfo.InvariantCulture),
                new Vector4(x + 2 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter,
                GetAlpha(status, index), GetTextColor(status, index, 2), status.RootControl);
        }

        public override int Dimension
        {
            get { return 3; }
        }

        protected override void ChangeValue(int index, float delta)
        {
            Value += new Vector3(index == 0 ? delta : 0, index == 1 ? delta : 0, index == 2 ? delta : 0);
        }

        public override void SetFromString(int index, string value)
        {
            float floatValue = float.Parse(value, System.Globalization.NumberFormatInfo.InvariantInfo);
            Value = new Vector3(index == 0 ? floatValue : Value.X,
                                index == 1 ? floatValue : Value.Y,
                                index == 2 ? floatValue : Value.Z);
        }
    }
}
