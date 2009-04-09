using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Dope.DDXX.UserInterface;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoTweaker
{
    public abstract class TweakableObjectBase<T> : ITweakable
    {
        private List<ITweakableProperty> propertyHandlers;
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

        public ITweakable GetChild(int index)
        {
            if (index < NumSpecificVariables)
                return GetSpecificVariable(index);
            return null;
        }

        public void CreateChildControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            if (index < NumSpecificVariables)
                GetChild(index).CreateControl(status, index, y, settings);
            else
                propertyHandlers[index - NumSpecificVariables].CreateControl(status, index, y, settings);
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

        public virtual void CreateBaseControls(TweakerStatus status, ITweakerSettings settings)
        {
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
                else if (child is XmlElement)
                    ParseSpecficXmlNode(child);
            }
        }

        public void WriteToXmlFile(XmlDocument xmlDocument, XmlNode node)
        {
            foreach (ITweakableProperty tweakable in propertyHandlers)
            {
                bool found = false;
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == tweakable.Name)
                        found = true;
                }
                if (!found && tweakable.Name != "StartTime" && tweakable.Name != "EndTime")
                {
                    AddNewChild(xmlDocument, node, tweakable.Name);
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
            WriteNewNodes(xmlDocument, node);
        }

        protected XmlElement AddNewChild(XmlDocument xmlDocument, XmlNode parentNode, string name)
        {
            if (parentNode.FirstChild == null && parentNode.PreviousSibling is XmlWhitespace)
                return AddNodeWhitespaceFromParent(xmlDocument, parentNode, name);
            if (parentNode.FirstChild is XmlWhitespace)
                return AddNodeWhitespaceFromChild(xmlDocument, parentNode, name);
            return AddNodeWithoutWhitespace(xmlDocument, parentNode, name);
        }

        protected void AddAttribute(XmlDocument xmlDocument, XmlElement node, string name, string value)
        {
            XmlAttribute newAttribute = xmlDocument.CreateAttribute(name);
            newAttribute.Value = value;
            node.Attributes.Append(newAttribute);
        }

        private XmlElement AddNodeWithoutWhitespace(XmlDocument xmlDocument, XmlNode parentNode, string name)
        {
            XmlElement newElement = xmlDocument.CreateElement(name);
            parentNode.InsertBefore(newElement, parentNode.LastChild);
            return newElement;
        }

        private XmlElement AddNodeWhitespaceFromChild(XmlDocument xmlDocument, XmlNode parentNode, string name)
        {
            parentNode.InsertBefore(xmlDocument.CreateWhitespace(parentNode.FirstChild.Value), parentNode.LastChild);
            return AddNodeWithoutWhitespace(xmlDocument, parentNode, name);
        }

        private XmlElement AddNodeWhitespaceFromParent(XmlDocument xmlDocument, XmlNode parentNode, string name)
        {
            parentNode.InsertBefore(xmlDocument.CreateWhitespace(parentNode.PreviousSibling.Value + "	"), parentNode.LastChild);
            XmlElement newElement = xmlDocument.CreateElement(name);
            parentNode.InsertAfter(newElement, parentNode.LastChild);
            parentNode.InsertAfter(xmlDocument.CreateWhitespace(parentNode.PreviousSibling.Value), parentNode.LastChild);
            return newElement;
        }

        protected virtual void WriteNewNodes(XmlDocument xmlDocument, XmlNode node)
        {
        }

        public virtual void Regenerate(TweakerStatus status)
        {
        }

        public virtual IMenuControl InsertNew(TweakerStatus status, IDrawResources drawResources, bool after)
        {
            return null;
        }

        public virtual void ChoiceMade(TweakerStatus status, int index)
        {
        }

        private bool HasProperty(string name)
        {
            return propertyHandlers.Exists(delegate(ITweakableProperty a) { return a.Name == name; });
        }

        private ITweakableProperty GetProperty(string name)
        {
            return propertyHandlers.Find(delegate(ITweakableProperty a) { return a.Name == name; });
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
            propertyHandlers = new List<ITweakableProperty>();
            foreach (PropertyInfo property in array)
            {
                if (property.CanRead && property.CanWrite)
                {
                    ITweakableProperty tweakable = factory.CreateTweakableValue(property, Target);
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

        protected List<ITweakableProperty> PropertyHandlers
        {
            get { return propertyHandlers; }
        }

    }
}
