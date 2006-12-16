using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.DirectX;
using System.Drawing;
using System.Collections;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public enum TweakableType
    {
        Unknown = 0, Integer, Float, String, Vector3, Color
    }

    public struct Parameter
    {
        public string name;
        public object value;
        public TweakableType Type;

        #region Value access
        public int IntValue
        {
            get { return (int)value; }
        }

        public string StringValue
        {
            get { return (string)value; }
        }

        public float FloatValue
        {
            get { return (float)value; }
        }

        public Vector3 Vector3Value
        {
            get { return (Vector3)value; }
        }

        #endregion

        #region ctors
        public Parameter(string name, TweakableType type, object value)
        {
            this.name = name;
            this.Type = type;
            this.value = value;
        }
        #endregion
    }

    public class DemoXMLReader : IEffectChangeListener
    {
        private IDemoEffectBuilder effectBuilder;
        private XmlDocument doc;

        public DemoXMLReader(IDemoEffectBuilder builder)
        {
            this.effectBuilder = builder;
        }

        public void Read(string filename)
        {
            doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            using (Stream inputStream = new FileStream(FileUtility.FilePath(filename), FileMode.Open))
            {
                doc.Load(inputStream);
                Parse(doc);
            }
        }

        public void ReadString(string xmlString)
        {
            doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(xmlString);
            Parse(doc);
        }

        private void Parse(XmlDocument doc)
        {
            XmlNode root = doc.DocumentElement;
            XmlNode effectsNode = root;
            while (effectsNode != null && effectsNode.Name != "Effects")
            {
                effectsNode = effectsNode.NextSibling;
            }
            if (effectsNode == null)
                throw new DDXXException("No effects found");
            foreach (XmlNode node in effectsNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element && node.Name == "Effect")
                {
                    ReadEffect(node);
                }
                else if (node.NodeType == XmlNodeType.Element && node.Name == "PostEffect")
                {
                    ReadPostEffect(node);
                }
                else if (node.NodeType == XmlNodeType.Element && node.Name == "Transition")
                {
                    ReadTransition(node);
                }
            }
        }

        private void ReadEffect(XmlNode node)
        {
            string effectName;
            int effectTrack;
            float startTime;
            float endTime;
            ReadNameTrack(node, out effectName, out effectTrack, out startTime, out endTime);
            effectBuilder.AddEffect(effectName, effectTrack, startTime, endTime);
            ReadParameters(node);
        }

        private void ReadPostEffect(XmlNode node)
        {
            string effectName;
            int effectTrack;
            float startTime;
            float endTime;
            ReadNameTrack(node, out effectName, out effectTrack, out startTime, out endTime);
            effectBuilder.AddPostEffect(effectName, effectTrack, startTime, endTime);
            ReadParameters(node);
        }

        private void ReadTransition(XmlNode node)
        {
            XmlAttribute effectName = (XmlAttribute)node.Attributes.GetNamedItem("name");
            XmlAttribute track = (XmlAttribute)node.Attributes.GetNamedItem("destinationTrack");
            int destinationTrack;
            if (track != null)
            {
                destinationTrack = int.Parse(track.Value);
            }
            else
            {
                destinationTrack = 0;
            }
            effectBuilder.AddTransition(effectName.Value, destinationTrack);
            ReadParameters(node);
        }

        private void ReadNameTrack(XmlNode node,
            out string effectName,
            out int effectTrack, out 
            float startTime,
            out float endTime)
        {
            XmlAttributeCollection attrs = node.Attributes;
            XmlAttribute attr = (XmlAttribute)attrs.GetNamedItem("name");
            if (attr == null)
                throw new DDXXException("name attr not found");
            effectName = attr.Value;
            XmlAttribute track = (XmlAttribute)attrs.GetNamedItem("track");
            XmlAttribute start = (XmlAttribute)attrs.GetNamedItem("startTime");
            XmlAttribute end = (XmlAttribute)attrs.GetNamedItem("endTime");
            effectTrack = 0;
            startTime = 0.0F;
            endTime = 0.0F;
            if (track != null)
            {
                effectTrack = int.Parse(track.Value);
            }
            if (start != null)
            {
                startTime = ParseFloat(start.Value);
            }
            if (end != null)
            {
                endTime = ParseFloat(end.Value);
            }
        }

        public void ReadParameters(XmlNode effectNode)
        {
            foreach (XmlNode node in effectNode.ChildNodes)
            {
                if (CommentOrWhitespace(node))
                    continue;
                if (node.NodeType == XmlNodeType.Element && node.Name == "Parameter")
                {
                    ReadParameter(node);
                }
                else if (node.NodeType == XmlNodeType.Element && node.Name == "SetupCall")
                {
                    ReadSetupCall(node);
                }
                else
                {
                    throw new DDXXException("Unknown tag in XML file.");
                }
            }
        }

        private static bool CommentOrWhitespace(XmlNode node)
        {
            return node.NodeType == XmlNodeType.Whitespace || node.NodeType == XmlNodeType.Comment;
        }

        private void ReadParameter(XmlNode node)
        {
            TweakableType parameterType = TweakableType.Unknown;
            string parameterName = null;
            string parameterValue = "";
            foreach (XmlAttribute attr in node.Attributes)
            {
                string name = attr.Name;
                string value = attr.Value;
                switch (name)
                {
                    case "name":
                        parameterName = value;
                        break;
                    default:
                        parameterType = GetParameterType(name);
                        if (parameterType != TweakableType.Unknown)
                        {
                            parameterValue = value;
                        }
                        break;
                }
            }
            if (parameterName != null && parameterType != TweakableType.Unknown)
            {
                AddParameter(parameterName, parameterType, parameterValue);
            }
            else
            {
                throw new DDXXException("Failed to parse parameter");
            }
        }

        private void ReadSetupCall(XmlNode setupNode)
        {
            XmlAttribute nameAttr = (XmlAttribute)setupNode.Attributes.GetNamedItem("name");
            if (nameAttr == null)
                throw new DDXXException("Could not parse setup call.");
            string name = nameAttr.Value;
            List<Object> parameters = new List<Object>();

            foreach (XmlNode node in setupNode.ChildNodes)
            {
                if (CommentOrWhitespace(node))
                    continue;
                if (node.NodeType == XmlNodeType.Element && node.Name == "Parameter")
                {
                    IEnumerator e = node.Attributes.GetEnumerator();
                    if (!e.MoveNext())
                        throw new DDXXException("Failed to parse parameters for setup call.");
                    XmlAttribute attr = (XmlAttribute)e.Current;
                    switch (GetParameterType(attr.Name))
                    {
                        case TweakableType.Float:
                            parameters.Add(ParseFloat(attr.Value));
                            break;
                        case TweakableType.Integer:
                            parameters.Add(int.Parse(attr.Value));
                            break;
                        case TweakableType.String:
                            parameters.Add(attr.Value);
                            break;
                        case TweakableType.Vector3:
                            string[] s = attr.Value.Split(new char[] { ',' }, 3);
                            Vector3 v = new Vector3(ParseFloat(s[0]), ParseFloat(s[1]), ParseFloat(s[2]));
                            parameters.Add(v);
                            break;
                        case TweakableType.Color:
                            parameters.Add(Color.FromName(attr.Value));
                            break;
                        default:
                            throw new DDXXException("Unknown internal parameter type");
                    }
                }
                else
                {
                    throw new DDXXException("Unknown tag in XML file.");
                }
            }
            effectBuilder.AddSetupCall(name, parameters);
        }

        private void AddParameter(string parameterName, TweakableType parameterType, string parameterValue)
        {
            switch (parameterType)
            {
                case TweakableType.Float:
                    effectBuilder.AddFloatParameter(parameterName, ParseFloat(parameterValue));
                    break;
                case TweakableType.Integer:
                    effectBuilder.AddIntParameter(parameterName, int.Parse(parameterValue));
                    break;
                case TweakableType.String:
                    effectBuilder.AddStringParameter(parameterName, parameterValue);
                    break;
                case TweakableType.Vector3:
                    string[] s = parameterValue.Split(new char[] { ',' }, 3);
                    Vector3 v = new Vector3(ParseFloat(s[0]), ParseFloat(s[1]), ParseFloat(s[2]));
                    effectBuilder.AddVector3Parameter(parameterName, v);
                    break;
                case TweakableType.Color:
                    effectBuilder.AddColorParameter(parameterName, Color.FromName(parameterValue));
                    break;
                default:
                    throw new DDXXException("Unknown internal parameter type");
            }
        }

        private float ParseFloat(string s)
        {
            return float.Parse(s, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        private TweakableType GetParameterType(string name)
        {
            switch (name)
            {
                case "int": return TweakableType.Integer;
                case "float": return TweakableType.Float;
                case "string": return TweakableType.String;
                case "Vector3": return TweakableType.Vector3;
                case "Color": return TweakableType.Color;
                default: return TweakableType.Unknown;
            }
        }

        private string GetParameterTypeString(TweakableType ty)
        {
            switch (ty)
            {
                case TweakableType.Integer: return "int";
                case TweakableType.Float: return "float";
                case TweakableType.String: return "string";
                case TweakableType.Vector3: return "Vector3";
                case TweakableType.Color: return "Color";
                default: return null;
            }
        }

        public void Write(string filename)
        {
            //doc.CreateXmlDeclaration("1.0", null, null);
            doc.Save(filename);
        }

        public void Write(TextWriter writer)
        {
            doc.Save(writer);
        }

        private XmlAttribute GetAttribute(XmlNode node, string attrName)
        {
            return (XmlAttribute)node.Attributes.GetNamedItem(attrName);
        }

        private string GetAttributeValue(XmlNode node, string attrName)
        {
            XmlAttribute attr = GetAttribute(node, attrName);
            if (attr != null)
                return attr.Value;
            else
                throw new DDXXException("Attribute " + attrName + " not found in node: " + node.ToString());
        }

        private XmlNode FindParameterContainer(string effectName)
        {
            XmlNode root = doc.DocumentElement;
            while (root != null && root.Name != "Effects")
                root = root.NextSibling;
            if (root != null)
            {
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (IsParameterContainer(node) && GetAttributeValue(node, "name") == effectName)
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        private static bool IsParameterContainer(XmlNode node)
        {
            return node.NodeType == XmlNodeType.Element && 
                (node.Name == "Effect" || node.Name == "PostEffect" || 
                node.Name == "Transition" || node.Name == "SetupCall");
        }

        private XmlNode FindParamNode(string effect, string paramName)
        {
            XmlNode effectNode = FindParameterContainer(effect);
            if (effectNode == null)
                throw new DDXXException("Named node " + effect + " not found");
            foreach (XmlNode node in effectNode.ChildNodes)
            {
                if (IsParamNode(node) && GetAttributeValue(node, "name") == paramName)
                {
                    return node;
                }
            }
            return null;
        }

        private static bool IsParamNode(XmlNode node)
        {
            return node.NodeType == XmlNodeType.Element && node.Name == "Parameter";
        }

        private XmlAttribute GetParamValueAttr(string effect, string param, TweakableType ty)
        {
            XmlNode paramNode = FindParamNode(effect, param);
            XmlAttribute attr = GetAttribute(paramNode, GetParameterTypeString(ty));
            if (attr == null)
                throw new DDXXException("XmlNode " + param + " in effect " + effect +
                    " is not a float parameter: " + paramNode.ToString());
            return attr;
        }

        public void FloatParamChanged(string effect, string param, string value)
        {
            XmlAttribute attr = GetParamValueAttr(effect, param, TweakableType.Float);
            attr.Value = value;
        }

        public void IntParamChanged(string effect, string param, string value)
        {
            XmlAttribute attr = GetParamValueAttr(effect, param, TweakableType.Integer);
            attr.Value = value;
        }

        public void StringParamChanged(string effect, string param, string value)
        {
            XmlAttribute attr = GetParamValueAttr(effect, param, TweakableType.String);
            attr.Value = value;
        }

        public void Vector3ParamChanged(string effect, string param, string value)
        {
            XmlAttribute attr = GetParamValueAttr(effect, param, TweakableType.Vector3);
            attr.Value = value;
        }

        public void ColorParamChanged(string effect, string param, string value)
        {
            XmlAttribute attr = GetParamValueAttr(effect, param, TweakableType.Color);
            attr.Value = value;
        }

        private void ChangeOrCreateAttribute(string effectName, string attrName, string value)
        {
            XmlNode effectNode = FindParameterContainer(effectName);
            XmlAttribute startAttr = GetAttribute(effectNode, attrName);
            if (startAttr == null)
            {
                XmlElement element = (XmlElement)effectNode;
                element.SetAttribute(attrName, value);
            }
            else
            {
                startAttr.Value = value;
            }
        }

        public void StartTimeChanged(string effectName, float value)
        {
            ChangeOrCreateAttribute(effectName, "startTime", value.ToString());
        }

        public void EndTimeChanged(string effectName, float value)
        {
            ChangeOrCreateAttribute(effectName, "endTime", value.ToString());
        }


        #region debugging
        private void PrintElement(XmlNode node)
        {
            Console.WriteLine("{0}: {1} = {2}",
                node.NodeType.ToString(), node.Name, node.Value);
            foreach (XmlAttribute attr in node.Attributes)
            {
                Console.WriteLine("  {0} = {1} ", attr.Name, attr.Value);
            }
        }
        #endregion
    }
}
