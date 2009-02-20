using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoTweaker
{
    public abstract class TweakableValueBase<T> : TweakableProperty<T>
    {
        protected abstract void ChangeValue(int index, float delta);
        protected abstract string GetToString(int index);

        protected TweakableValueBase(PropertyInfo property, object target, ITweakableFactory factory)
            : base(property, target, factory)
        {
        }

        public override void IncreaseValue(int index)
        {
            ChangeValue(index, TweakStep);
        }

        public override void DecreaseValue(int index)
        {
            ChangeValue(index, -TweakStep);
        }

        public override void SetFromString(string value)
        {
            string[] values = value.Split(',');
            for (int i = 0; i < values.Length; i++)
                SetFromString(i, values[i]);
        }

        public override string GetToString()
        {
            string value = "";
            for (int i = 0; i < Dimension; i++)
            {
                if (i != 0)
                    value += ", ";
                value += GetToString(i);
            }
            return value;
        }

    }
}
