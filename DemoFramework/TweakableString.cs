using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableString : TweakableValueBase<string>
    {
        public TweakableString(PropertyInfo property, object target)
            : base(property, target)
        {
        }

        protected override void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h)
        {
            new TextControl(Value, new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter, 
                GetAlpha(status, index), GetTextColor(status, index, 0), status.RootControl);
        }

        public override int Dimension
        {
            get { return 1; }
        }

        protected override void ChangeValue(int index, float delta)
        {
            return;
        }

        public override void SetFromString(int index, string value)
        {
            Value = value;
        }

        protected override string GetToString(int index)
        {
            return Value;
        }
    }
}
