using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Dope.DDXX.UserInterface;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoTweaker
{
    public class TweakableBoolean : TweakableValueBase<bool>
    {
        public TweakableBoolean(PropertyInfo property, object target, ITweakableFactory factory)
            : base(property, target, factory)
        {
        }

        protected override void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h, ITweakerSettings settings)
        {
            new TextControl(Value ? "true" : "false", new Vector4(x, y, w, h),
                Positioning.Center | Positioning.VerticalCenter, settings.TextAlpha, 
                GetTextColor(status, index, 0), status.RootControl);
        }

        public override int Dimension
        {
            get { return 1; }
        }

        protected override void ChangeValue(int index, float delta)
        {
            Value = !Value;
        }

        public override void SetFromString(int index, string value)
        {
            Value = bool.Parse(value);
        }

        protected override string GetToString(int index)
        {
            if (Value)
                return "true";
            return "false";
        }

    }
}
