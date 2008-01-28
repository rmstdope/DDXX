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
    public abstract class TweakableObjectBase<T> : ITweakable
    {
        private List<ITweakable> propertyHandlers;
        private T target;
        private ITweakableFactory factory;

        public abstract int NumVisableVariables { get; }
        protected abstract int NumSpecificVariables { get; }
        protected abstract ITweakable GetSpecificVariable(int index);
        protected abstract void ParseSpecficXmlNode(XmlNode node);
        protected abstract void WriteSpecificXmlNode(XmlDocument xmlDocument, XmlNode node);
        public abstract void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings);

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

        public bool IsObject()
        {
            return true;
        }


        public void CreateVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            GetTweakableChild(index).CreateControl(status, index, y, settings);
            //if (index < NumSpecificVariables)
            //    CreateSpecificVariableControl(status, index, y, settings);
            //else
            //    propertyHandlers[index - NumSpecificVariables].CreateVariableControl(status, index, y, settings);
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

        public void ReadFromXmlFile(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (HasProperty(child.Name))
                    ParseProperty(child);
                else
                    ParseSpecficXmlNode(child);
            }
        }

        public void WriteToXmlFile(XmlDocument xmlDocument, XmlNode node)
        {
            foreach (ITweakable tweakable in propertyHandlers)
            {
                bool found = false;
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == tweakable.Property.Name)
                        found = true;
                }
                if (!found && tweakable.Property.Name != "StartTime" && tweakable.Property.Name != "EndTime")
                {
                    XmlNode whitespace = node.FirstChild;
                    if (whitespace is XmlWhitespace)
                        node.InsertBefore(xmlDocument.CreateWhitespace(whitespace.Value), node.LastChild);
                    node.InsertBefore(xmlDocument.CreateElement(tweakable.Property.Name), node.LastChild);
                    //XmlWhitespace newWhitespace = xmlDocument.CreateWhitespace(lastWhitespace.Value);
                    //node.AppendChild(newWhitespace);
                    //lastWhitespace.Value += "\t";
                }
            }
            foreach (XmlNode child in node.ChildNodes)
            {
                if (!(child is XmlElement))
                    continue;
                if (HasProperty(child.Name))
                    WriteProperty(child);
                else
                    WriteSpecificXmlNode(xmlDocument, child);
            }
        }

        private bool HasProperty(string name)
        {
            return propertyHandlers.Exists(delegate(ITweakable a) { return a.Property.Name == name; });
        }

        private ITweakable GetProperty(string name)
        {
            return propertyHandlers.Find(delegate(ITweakable a) { return a.Property.Name == name; });
        }

        private void ParseProperty(XmlNode node)
        {
            GetProperty(node.Name).SetFromString(node.InnerText);
        }

        private void WriteProperty(XmlNode node)
        {
            node.InnerText = GetProperty(node.Name).GetToString();
        }

        private void GetProperties()
        {
            PropertyInfo[] array = target.GetType().GetProperties();
            propertyHandlers = new List<ITweakable>();
            foreach (PropertyInfo property in array)
            {
                if (property.CanRead && property.CanWrite)
                {
                    ITweakable tweakable = factory.CreateTweakableValue(property, Target);
                    if (tweakable != null)
                        propertyHandlers.Add(tweakable);
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

        protected List<ITweakable> PropertyHandlers
        {
            get { return propertyHandlers; }
        }

        public int Dimension
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void IncreaseValue(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DecreaseValue(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetFromString(string value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetFromString(int index, string value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public PropertyInfo Property
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public string GetToString()
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
