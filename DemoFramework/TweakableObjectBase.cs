using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

// ITweakableValue
//  - TweakableVector2
//  - TweakableVector3
//  - TweakableVector4
//  - TweakableSingle
//  - TweakableInt32
//  - TweakableBoolean
//  - TweakableString
// ITweakableObject
//  - TweakableDemo
//  - TweakableTrack
//  - TweakableRegitrable
//  - TweakableScene
//  - TweakableLight
//  - TweakableModel
//  - Tweakable

namespace Dope.DDXX.DemoFramework
{
    public abstract class TweakableObjectBase<T> : ITweakableObject
    {
        private List<ITweakableValue> propertyHandlers;
        private T target;
        private ITweakableFactory factory;

        public abstract int NumVisableVariables { get; }
        protected abstract int NumSpecificVariables { get; }
        protected abstract ITweakableObject GetSpecificVariable(int index);
        protected abstract void CreateSpecificVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings);
        public abstract void ReadFromXmlFile(XmlNode node);

        protected T Target
        {
            get { return target; }
        }

        protected ITweakableFactory Factory
        {
            get { return factory; }
        }

        protected TweakableObjectBase(T target, ITweakableFactory factory)
        {
            this.target = target;
            this.factory = factory;
            GetProperties();
        }

        public int NumVariables
        {
            get { return NumSpecificVariables + propertyHandlers.Count; }
        }

        public ITweakable GetTweakableChild(int index)
        {
            if (index < NumSpecificVariables)
                return GetSpecificVariable(index);
            return propertyHandlers[index - NumSpecificVariables];
        }

        public void CreateVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            if (index < NumSpecificVariables)
                CreateSpecificVariableControl(status, index, y, settings);
            else
                propertyHandlers[index - NumSpecificVariables].CreateVariableControl(status, index, y, settings);
        }

        public void NextIndex(TweakerStatus status)
        {
            int dimension = 1;
            if (status.Selection >= NumSpecificVariables)
                dimension = propertyHandlers[status.Selection - NumSpecificVariables].Dimension;
            status.Index++;
            if (status.Index == dimension)
                status.Index = 0;
        }

        public void IncreaseValue(TweakerStatus status)
        {
            if (status.Selection >= NumSpecificVariables)
                propertyHandlers[status.Selection - NumSpecificVariables].IncreaseValue(status.Index);
        }

        public void DecreaseValue(TweakerStatus status)
        {
            if (status.Selection >= NumSpecificVariables)
                propertyHandlers[status.Selection - NumSpecificVariables].DecreaseValue(status.Index);
        }

        public void CreateBaseControls(TweakerStatus status, ITweakerSettings settings)
        {
            string displayText = "<No Input>";
            BoxControl inputBox = new BoxControl(new Vector4(0, 0.95f, 1, 0.05f),
                settings.Alpha, settings.TitleColor, status.RootControl);
            if (status.InputString != "")
                displayText = "Input: " + status.InputString;
            TextControl text = new TextControl(displayText, new Vector2(0.5f, 0.5f),
                TextFormatting.Center | TextFormatting.VerticalCenter, 255,
                Color.White, inputBox);
        }

        public void SetValue(TweakerStatus status)
        {
            if (status.Selection >= NumSpecificVariables)
                propertyHandlers[status.Selection - NumSpecificVariables].SetFromString(status.Index, status.InputString);
        }

        private void GetProperties()
        {
            List<KeyValuePair<Type, Type>> typeTweakableMapping = new List<KeyValuePair<Type, Type>>();
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(int), typeof(TweakableInt32)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(float), typeof(TweakableSingle)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(Vector2), typeof(TweakableVector2)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(Vector3), typeof(TweakableVector3)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(Vector4), typeof(TweakableVector4)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(string), typeof(TweakableString)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(Color), typeof(TweakableColor)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(bool), typeof(TweakableBoolean)));
            PropertyInfo[] array = target.GetType().GetProperties();
            propertyHandlers = new List<ITweakableValue>();
            foreach (PropertyInfo property in array)
            {
                if (property.CanRead && property.CanWrite)
                {
                    foreach (KeyValuePair<Type, Type> pair in typeTweakableMapping)
                    {
                        if (pair.Key == property.PropertyType)
                            propertyHandlers.Add(pair.Value.
                                GetConstructor(new Type[] { typeof(PropertyInfo), typeof(object) }).
                                Invoke(new object[] { property, target }) as ITweakableValue);
                    }
                }
            }            
        }

        protected Color GetTextColor(TweakerStatus status, int selection, int index)
        {
            if (selection == status.Selection && index == status.Index)
                return Color.Red;
            return Color.White;
        }

        protected Color GetBoxColor(TweakerStatus status, int selection, ITweakerSettings settings)
        {
            if (selection == status.Selection)
                return settings.SelectedColor;
            return settings.UnselectedColor;
        }

        protected string GetStringAttribute(XmlNode node, string name)
        {
            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.Name.ToLower() == name)
                    return attribute.Value;
            }
            throw new DDXXException("<" + node.Name + "> tag must have a " + name + " attribute.");
        }

        protected int GetIntAttribute(XmlNode node, string name)
        {
            return int.Parse(GetStringAttribute(node, name), System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        protected float GetFloatAttribute(XmlNode node, string name)
        {
            return float.Parse(GetStringAttribute(node, name), System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        protected List<ITweakableValue> PropertyHandlers
        {
            get { return propertyHandlers; }
        }

    }
}
