using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public abstract class TweakableProperty<T> : ITweakableProperty
    {
        private PropertyInfo property;
        private object target;

        public TweakableProperty(PropertyInfo property, object target)
        {
            this.property = property;
            this.target = target;
        }

        public PropertyInfo Property
        {
            get { return property; } 
        }

        protected object Target
        {
            get { return target; }
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

        public void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            float height = status.VariableSpacing * 0.9f;
            if (index == status.Selection)
                new BoxControl(new Vector4(0, y, 1, height), GetAlpha(status, index), Color.Black, status.RootControl);
            new TextControl(property.Name, new Vector4(0, y, 0.45f, height), TextFormatting.Right | TextFormatting.VerticalCenter, GetAlpha(status, index), Color.White, status.RootControl);
            CreateValueControls(status, index, 0.55f, y, 0.45f, height);
        }

        protected abstract void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h);
        public abstract int Dimension { get; }
        public abstract void IncreaseValue(int index);
        public abstract void DecreaseValue(int index);
        public abstract void SetFromString(string value);
        public abstract void SetFromString(int index, string value);
        public abstract string GetToString();
    }
}
