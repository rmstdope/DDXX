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
        Unknown = 0, Integer, Float, String, Vector3, Color, Bool
    }

    public struct Parameter
    {
        public string name;
        public object value;
        public TweakableType Type;
        public float StepSize;

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

        public Color ColorValue
        {
            get { return (Color)value; }
        }

        public bool BoolValue
        {
            get { return (bool)value; }
        }

        #endregion

        #region ctors
        public Parameter(string name, TweakableType type, object value)
        {
            this.name = name;
            this.Type = type;
            this.value = value;
            this.StepSize = 1;
        }
        public Parameter(string name, TweakableType type, object value, float stepSize)
        {
            this.name = name;
            this.Type = type;
            this.value = value;
            this.StepSize = stepSize;
        }
        #endregion
    }

    public class DemoXMLReader : IEffectChangeListener
    {
        private IDemoEffectBuilder effectBuilder;
        private XmlDocument doc;
        private string filename;

        public DemoXMLReader(IDemoEffectBuilder builder)
        {
            this.effectBuilder = builder;
        }

        public void Read(string filename)
        {
            this.filename = filename;
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
            XmlNode demoNode = GetDemoNode(doc);
            HandleDemoAttributes(doc, demoNode);
            HandleDemoChildren(demoNode);
        }

        private void HandleDemoChildren(XmlNode effectsNode)
        {
            foreach (XmlNode node in effectsNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Effect":
                        ReadEffect(node);
                        break;
                    case "PostEffect":
                        ReadPostEffect(node);
                        break;
                    case "Transition":
                        ReadTransition(node);
                        break;
                    case "Generator":
                        ReadGenerator(node);
                        break;
                }
            }
        }

        private void HandleDemoAttributes(XmlDocument doc, XmlNode effectsNode)
        {
            foreach (XmlAttribute node in effectsNode.Attributes)
            {
                switch (node.Name)
                {
                    case "song":
                        effectBuilder.SetSong(node.InnerText);
                        break;
                    default:
                        throw new DDXXException("Unknown attribute '" + node.Name + "' in xml file " + doc.Name);
                }
            }
        }

        private static XmlNode GetDemoNode(XmlDocument doc)
        {
            XmlNode effectsNode = doc.DocumentElement;
            // TODO: Rename "Effects" to "Demo"
            while (effectsNode != null && effectsNode.Name != "Effects")
            {
                effectsNode = effectsNode.NextSibling;
            }
            if (effectsNode == null)
                throw new DDXXException("No effects found");
            return effectsNode;
        }

        private void ReadGenerator(XmlNode node)
        {
            XmlAttribute name = (XmlAttribute)node.Attributes.GetNamedItem("name");
            XmlAttribute className = (XmlAttribute)node.Attributes.GetNamedItem("class");
            effectBuilder.AddGenerator(name.Value, className.Value);
            ReadParameters(node, true);
        }

        private void ReadEffect(XmlNode node)
        {
            string effectName;
            int effectTrack;
            float startTime;
            float endTime;
            ReadNameTrack(node, out effectName, out effectTrack, out startTime, out endTime);
            effectBuilder.AddEffect(effectName, effectTrack, startTime, endTime);
            ReadParameters(node, false);
        }

        private void ReadPostEffect(XmlNode node)
        {
            string effectName;
            int effectTrack;
            float startTime;
            float endTime;
            ReadNameTrack(node, out effectName, out effectTrack, out startTime, out endTime);
            effectBuilder.AddPostEffect(effectName, effectTrack, startTime, endTime);
            ReadParameters(node, false);
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
            ReadParameters(node, false);
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

        public void ReadParameters(XmlNode effectNode, bool readInputs)
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
                else if (node.NodeType == XmlNodeType.Element && node.Name == "Input" && readInputs)
                {
                    ReadInput(node);
                }
                else
                {
                    throw new DDXXException("Unknown tag in XML file.");
                }
            }
        }

        private void ReadInput(XmlNode node)
        {
            int number = -1;
            string generator = "";
            foreach (XmlAttribute attr in node.Attributes)
            {
                switch (attr.Name)
                {
                    case "number":
                        number = int.Parse(attr.Value);
                        break;
                    case "generator":
                        generator = attr.Value;
                        break;
                }
            }
            if (number == -1)
                throw new DDXXException("Found generator input without name in " + doc.Name);
            if (generator == "")
                throw new DDXXException("Found generator input without generator in " + doc.Name);
            effectBuilder.AddGeneratorInput(number, generator);
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
            float parameterStep = -1;
            foreach (XmlAttribute attr in node.Attributes)
            {
                string name = attr.Name;
                string value = attr.Value;
                switch (name)
                {
                    case "name":
                        parameterName = value;
                        break;
                    case "step":
                        parameterStep = ParseFloat(value);
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
                AddParameter(parameterName, parameterType, parameterValue, parameterStep);
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
                            parameters.Add(ParseVector3(attr.Value));
                            break;
                        case TweakableType.Color:
                            parameters.Add(ParseColor(attr.Value));
                            break;
                        case TweakableType.Bool:
                            parameters.Add(ParseBool(attr.Value));
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

        private static bool ParseBool(string strval)
        {
            if (strval.ToLower() == "true")
                return true;
            return false;
        }

        private static Vector3 ParseVector3(string strval)
        {
            string[] s = strval.Split(new char[] { ',' }, 3);
            Vector3 v = new Vector3(ParseFloat(s[0]), ParseFloat(s[1]), ParseFloat(s[2]));
            return v;
        }

        private static Color ParseColor(string strval)
        {
            Color color = Color.FromName(strval);
            if (color.A == 0 && color.R == 0 && color.G == 0 && color.B == 0)
            {
                string[] s = strval.Split(new char[] { ',' });
                int a = 255;
                int i = 0;
                if (s.Length == 4)
                {
                    a = int.Parse(s[i++]);
                }
                color = Color.FromArgb(a, int.Parse(s[i++]), int.Parse(s[i++]), int.Parse(s[i++]));
            }
            return color;
        }

        private void AddParameter(string parameterName, TweakableType parameterType, string parameterValue, float parameterStep)
        {
            switch (parameterType)
            {
                case TweakableType.Float:
                    effectBuilder.AddFloatParameter(parameterName, ParseFloat(parameterValue), parameterStep);
                    break;
                case TweakableType.Integer:
                    effectBuilder.AddIntParameter(parameterName, int.Parse(parameterValue), parameterStep);
                    break;
                case TweakableType.String:
                    effectBuilder.AddStringParameter(parameterName, parameterValue);
                    break;
                case TweakableType.Vector3:
                    effectBuilder.AddVector3Parameter(parameterName, ParseVector3(parameterValue), parameterStep);
                    break;
                case TweakableType.Color:
                    effectBuilder.AddColorParameter(parameterName, ParseColor(parameterValue));
                    break;
                case TweakableType.Bool:
                    effectBuilder.AddBoolParameter(parameterName, ParseBool(parameterValue));
                    break;
                default:
                    throw new DDXXException("Unknown internal parameter type");
            }
        }

        private static float ParseFloat(string s)
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
                case "bool": return TweakableType.Bool;
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
                case TweakableType.Bool: return "bool";
                default: return null;
            }
        }


        public void Write()
        {
            using (Stream outStream = new FileStream(FileUtility.FilePath(filename), FileMode.Create))
                doc.Save(outStream);
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
            if (paramNode == null)
                paramNode = CreateParameterNode(effect, param, ty);
            XmlAttribute attr = GetAttribute(paramNode, GetParameterTypeString(ty));
            if (attr == null)
                throw new DDXXException("XmlNode " + param + " in effect " + effect +
                    " is not a float parameter: " + paramNode.ToString());
            return attr;
        }

        private XmlNode CreateParameterNode(string effect, string param, TweakableType ty)
        {
            XmlNode effectNode = FindParameterContainer(effect);
            if (effectNode.ChildNodes.Count == 0)
                effectNode.AppendChild(doc.CreateWhitespace("\r\n  "));
            XmlNode lastWhitespace = effectNode.LastChild;
            XmlElement newElement = doc.CreateElement("Parameter");
            effectNode.AppendChild(newElement);
            XmlWhitespace newWhitespace = doc.CreateWhitespace(lastWhitespace.Value);
            effectNode.AppendChild(newWhitespace);
            newElement.SetAttribute("name", param);
            newElement.SetAttribute(GetParameterTypeString(ty), "");
            lastWhitespace.Value += "  ";
            return newElement;
        }

        private string FloatToString(float value)
        {
            return value.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        public void SetFloatParam(string effect, string param, float value)
        {
            XmlAttribute attr = GetParamValueAttr(effect, param, TweakableType.Float);
            attr.Value = FloatToString(value);
        }

        public void SetIntParam(string effect, string param, int value)
        {
            XmlAttribute attr = GetParamValueAttr(effect, param, TweakableType.Integer);
            attr.Value = value.ToString();
        }

        public void SetStringParam(string effect, string param, string value)
        {
            XmlAttribute attr = GetParamValueAttr(effect, param, TweakableType.String);
            attr.Value = value;
        }

        public void SetVector3Param(string effect, string param, Vector3 value)
        {
            XmlAttribute attr = GetParamValueAttr(effect, param, TweakableType.Vector3);
            string strval = FloatToString(value.X) + ", " +
                FloatToString(value.Y) + ", " +
                FloatToString(value.Z);
            attr.Value = strval;
        }

        public void SetColorParam(string effect, string param, Color value)
        {
            XmlAttribute attr = GetParamValueAttr(effect, param, TweakableType.Color);
            string strval;
            if (value.IsNamedColor)
            {
                strval = value.Name;
            }
            else
            {
                strval = FloatToString(value.A) + ", " +
                    FloatToString(value.R) + ", " +
                    FloatToString(value.G) + ", " +
                    FloatToString(value.B);
            }
            attr.Value = strval;
        }

        public void SetBoolParam(string effect, string param, bool value)
        {
            XmlAttribute attr = GetParamValueAttr(effect, param, TweakableType.Bool);
            if (value)
                attr.Value = "true";
            else
                attr.Value = "false";
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

        public void SetStartTime(string effectName, float value)
        {
            ChangeOrCreateAttribute(effectName, "startTime", value.ToString());
        }

        public void SetEndTime(string effectName, float value)
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
