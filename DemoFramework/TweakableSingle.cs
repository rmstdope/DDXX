using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableSingle : TweakableValueBase<float>
    {
        public TweakableSingle(PropertyInfo property, object target)
            : base(property, target)
        {
        }

        protected override void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h)
        {
            new TextControl(Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter,
                GetAlpha(status, index), GetTextColor(status, index, 0), status.RootControl);
        }

        public override int Dimension
        {
            get { return 1; }
        }

        protected override void ChangeValue(int index, float delta)
        {
            Value += delta;
        }

        public override void SetFromString(int index, string value)
        {
            Value = float.Parse(value, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
    }
}
