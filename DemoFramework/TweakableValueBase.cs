using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public abstract class TweakableValueBase<T> : TweakableProperty<T>
    {
        protected abstract void ChangeValue(int index, float delta);
        protected abstract string GetToString(int index);

        protected TweakableValueBase(PropertyInfo property, object target)
            : base(property, target)
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

        private float TweakStep
        {
            get 
            { 
                foreach (TweakStepAttribute attribute in Property.GetCustomAttributes(false))
                {
                    if (attribute != null)
                        return attribute.Step;
                }
                return 1.0f;
            }
        }

        public override int NumVisableVariables
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override int NumVariables
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override ITweakable GetTweakableChild(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsObject()
        {
            return false;
        }

        public override void CreateBaseControls(TweakerStatus status, ITweakerSettings settings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void NextIndex(TweakerStatus status)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void IncreaseValue(TweakerStatus status)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DecreaseValue(TweakerStatus status)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetValue(TweakerStatus status)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ReadFromXmlFile(System.Xml.XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void WriteToXmlFile(System.Xml.XmlDocument xmlDocument, System.Xml.XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
