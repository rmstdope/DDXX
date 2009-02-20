using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.UserInterface;

namespace Dope.DDXX.DemoTweaker
{
    public class TweakableSingle : TweakableValueBase<float>
    {
        public TweakableSingle(PropertyInfo property, object target, ITweakableFactory factory)
            : base(property, target, factory)
        {
        }

        protected override void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h, ITweakerSettings settings)
        {
            new TextControl(Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                new Vector4(x, y, w, h), Positioning.Center | Positioning.VerticalCenter,
                settings.TextAlpha, GetTextColor(status, index, 0), status.RootControl);
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

        protected override string GetToString(int index)
        {
            return Value.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
        }

    }
}
