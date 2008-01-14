using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public abstract class TweakableValueBase<T> : ITweakableValue
    {
        private PropertyInfo property;
        private object target;

        protected abstract void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h);
        public abstract int Dimension { get; }
        protected abstract void ChangeValue(int index, float delta);
        public abstract void SetFromInputString(TweakerStatus status);

        protected TweakableValueBase(PropertyInfo property, object target)
        {
            this.property = property;
            this.target = target;
        }

        public void CreateVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            float height = status.VariableSpacing * 0.9f;
            if (index == status.Selection)
                new BoxControl(new Vector4(0, y, 1, height), GetAlpha(status, index), Color.Black, status.RootControl);
            new TextControl(property.Name, new Vector4(0, y, 0.45f, height), TextFormatting.Right | TextFormatting.VerticalCenter, GetAlpha(status, index), Color.White, status.RootControl);
            CreateValueControls(status, index, 0.55f, y, 0.45f, height);
        }

        public void IncreaseValue(int index)
        {
            ChangeValue(index, 1.0f);
        }

        public void DecreaseValue(int index)
        {
            ChangeValue(index, -1.0f);
        }

        protected byte GetAlpha(TweakerStatus status, int selection)
        {
            if (selection == status.Selection)
                return 200;
            return 75;
        }

        protected Color GetTextColor(TweakerStatus status, int selection, int index)
        {
            if (selection == status.Selection && index == status.Index)
                return Color.Red;
            return Color.White;
        }

        protected T Value
        {
            get { return InvokeGetter(); }
            set { InvokeSetter(value); }
        }

        private T InvokeGetter()
        {
            return (T)property.GetGetMethod().Invoke(target, null);
        }

        private void InvokeSetter(T value)
        {
            property.GetSetMethod().Invoke(target, new object[] { value });
        }

    }
}
