using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public enum TweakableType
    {
        Unknown = 0, 
        Integer, 
        Float, 
        String, 
        Vector2, 
        Vector3, 
        Vector4, 
        Color, 
        Bool,
        IScene
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

        public Vector2 Vector2Value
        {
            get { return (Vector2)value; }
        }

        public Vector3 Vector3Value
        {
            get { return (Vector3)value; }
        }

        public Vector4 Vector4Value
        {
            get { return (Vector4)value; }
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
            this.filename = FileUtility.FilePath(filename);
            doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            using (Stream inputStream = new FileStream(this.filename, FileMode.Open))
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

        private void HandleDemoChildren(XmlNode demoNode)
        {
            foreach (XmlNode node in demoNode.ChildNodes)
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
                    case "Texture":
                        ReadTexture(node);
                        break;
                }
            }
        }

        private void ReadTexture(XmlNode node)
        {
            int width = 128;
            int height = 128;
            int mipLevels = 0;
            if (node.Attributes.GetNamedItem("width") != null)
                width = int.Parse(node.Attributes.GetNamedItem("width").Value);
            if (node.Attributes.GetNamedItem("height") != null)
                height = int.Parse(node.Attributes.GetNamedItem("height").Value);
            if (node.Attributes.GetNamedItem("miplevels") != null)
                mipLevels = int.Parse(node.Attributes.GetNamedItem("miplevels").Value);

            effectBuilder.AddTexture(node.Attributes.GetNamedItem("name").Value,
                node.Attributes.GetNamedItem("generator").Value, width, height, mipLevels);
        }

        private void HandleDemoAttributes(XmlDocument doc, XmlNode demoNode)
        {
            foreach (XmlAttribute node in demoNode.Attributes)
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
            XmlNode demoNode = doc.DocumentElement;
            while (demoNode != null && demoNode.Name != "Demo")
            {
                demoNode = demoNode.NextSibling;
            }
            if (demoNode == null)
                throw new DDXXException("No <Demo> tag found");
            return demoNode;
        }

        private void ReadGenerator(XmlNode node)
        {
            effectBuilder.AddGenerator(node.Attributes.GetNamedItem("name").Value, 
                node.Attributes.GetNamedItem("class").Value);
            ReadParameters(node, true);
        }

        private void ReadEffect(XmlNode node)
        {
            string className;
            string effectName;
            int effectTrack;
            float startTime;
            float endTime;
            ReadNameTrack(node, out className, out effectName, out effectTrack, out startTime, out endTime);
            effectBuilder.AddEffect(className, effectName, effectTrack, startTime, endTime);
            ReadParameters(node, false);
        }

        private void ReadPostEffect(XmlNode node)
        {
            string className;
            string effectName;
            int effectTrack;
            float startTime;
            float endTime;
            ReadNameTrack(node, out className, out effectName, out effectTrack, out startTime, out endTime);
            effectBuilder.AddPostEffect(className, effectName, effectTrack, startTime, endTime);
            ReadParameters(node, false);
        }

        private void ReadTransition(XmlNode node)
        {
            string className;
            string effectName;
            int effectTrack;
            float startTime;
            float endTime;
            ReadNameTrack(node, out className, out effectName, out effectTrack, out startTime, out endTime);
            effectBuilder.AddTransition(className, effectName, effectTrack, startTime, endTime);
            ReadParameters(node, false);
        }

        private void ReadNameTrack(XmlNode node,
            out string className,
            out string effectName,
            out int effectTrack, out 
            float startTime,
            out float endTime)
        {
            XmlAttributeCollection attrs = node.Attributes;
            className = GetClassName(attrs);
            effectName = GetEffectName(className, attrs);
            effectTrack = GetTrackNumber(attrs);
            startTime = GetStartTime(attrs);
            endTime = GetEndTime(attrs);
        }

        private static float GetEndTime(XmlAttributeCollection attrs)
        {
            float endTime;
            if (attrs.GetNamedItem("endTime") != null)
                endTime = ParseFloat(attrs.GetNamedItem("endTime").Value);
            else
                endTime = 0;
            return endTime;
        }

        private static float GetStartTime(XmlAttributeCollection attrs)
        {
            float startTime;
            if (attrs.GetNamedItem("startTime") != null)
                startTime = ParseFloat(attrs.GetNamedItem("startTime").Value);
            else
                startTime = 0;
            return startTime;
        }

        private static int GetTrackNumber(XmlAttributeCollection attrs)
        {
            int effectTrack;
            if (attrs.GetNamedItem("track") != null)
                effectTrack = int.Parse(attrs.GetNamedItem("track").Value);
            else
                effectTrack = 0;
            return effectTrack;
        }

        private static string GetEffectName(string className, XmlAttributeCollection attrs)
        {
            string effectName;
            if (attrs.GetNamedItem("name") != null)
                effectName = attrs.GetNamedItem("name").Value;
            else
                effectName = className;
            return effectName;
        }

        private static string GetClassName(XmlAttributeCollection attrs)
        {
            string className;
            if (attrs.GetNamedItem("class") == null)
                throw new DDXXException("class attr not found");
            className = attrs.GetNamedItem("class").Value;
            return className;
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
                    throw new DDXXException("Unknown tag '" + node.Name + "' in XML file.");
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
                        case TweakableType.Vector2:
                            parameters.Add(ParseVector2(attr.Value));
                            break;
                        case TweakableType.Vector3:
                            parameters.Add(ParseVector3(attr.Value));
                            break;
                        case TweakableType.Vector4:
                            parameters.Add(ParseVector4(attr.Value));
                            break;
                        case TweakableType.Color:
                            parameters.Add(ParseColor(attr.Value));
                            break;
                        case TweakableType.Bool:
                            parameters.Add(ParseBool(attr.Value));
                            break;
                        default:
                            throw new DDXXException("Unknown public parameter type");
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

        private static Vector2 ParseVector2(string strval)
        {
            string[] s = strval.Split(new char[] { ',' });
            Vector2 v = new Vector2(ParseFloat(s[0]), ParseFloat(s[1]));
            return v;
        }

        private static Vector3 ParseVector3(string strval)
        {
            string[] s = strval.Split(new char[] { ',' });
            Vector3 v = new Vector3(ParseFloat(s[0]), ParseFloat(s[1]), ParseFloat(s[2]));
            return v;
        }

        private static Vector4 ParseVector4(string strval)
        {
            string[] s = strval.Split(new char[] { ',' });
            Vector4 v = new Vector4(ParseFloat(s[0]), ParseFloat(s[1]), ParseFloat(s[2]), ParseFloat(s[3]));
            return v;
        }

        private static Color ParseColor(string strval)
        {
            Color color = new Color(0, 0, 0, 0);//.FromName(strval);
            if (color.A == 0 && color.R == 0 && color.G == 0 && color.B == 0)
            {
                string[] s = strval.Split(new char[] { ',' });
                int i = 0;
                if (s.Length == 4)
                    color = new Color(byte.Parse(s[i++]), byte.Parse(s[i++]), byte.Parse(s[i++]), byte.Parse(s[i++]));
                else
                    color = new Color(byte.Parse(s[i++]), byte.Parse(s[i++]), byte.Parse(s[i++]), 255);
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
                case TweakableType.Vector2:
                    effectBuilder.AddVector2Parameter(parameterName, ParseVector2(parameterValue), parameterStep);
                    break;
                case TweakableType.Vector3:
                    effectBuilder.AddVector3Parameter(parameterName, ParseVector3(parameterValue), parameterStep);
                    break;
                case TweakableType.Vector4:
                    effectBuilder.AddVector4Parameter(parameterName, ParseVector4(parameterValue), parameterStep);
                    break;
                case TweakableType.Color:
                    effectBuilder.AddColorParameter(parameterName, ParseColor(parameterValue));
                    break;
                case TweakableType.Bool:
                    effectBuilder.AddBoolParameter(parameterName, ParseBool(parameterValue));
                    break;
                default:
                    throw new DDXXException("Unknown public parameter type");
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
                case "Vector2": return TweakableType.Vector2;
                case "Vector3": return TweakableType.Vector3;
                case "Vector4": return TweakableType.Vector4;
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
                case TweakableType.Vector2: return "Vector2";
                case TweakableType.Vector3: return "Vector3";
                case TweakableType.Vector4: return "Vector4";
                case TweakableType.Color: return "Color";
                case TweakableType.Bool: return "bool";
                default: return null;
            }
        }


        public void Write()
        {
            using (Stream outStream = new FileStream(FileUtility.FilePath("..\\..\\..\\" + filename), FileMode.Create))
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

        private XmlNode FindParameterContainer(string className, string effectName)
        {
            XmlNode root = doc.DocumentElement;
            while (root != null && root.Name != "Demo")
                root = root.NextSibling;
            if (root != null)
            {
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (IsParameterContainer(node))
                    {
                        string nameAttributeString = "name";
                        if (GetAttribute(node, "name") == null)
                            nameAttributeString = "class";
                        if (GetAttributeValue(node, "class") == className &&
                            GetAttributeValue(node, nameAttributeString) == effectName)
                        {
                            return node;
                        }
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

        private XmlNode FindParamNode(string className, string effect, string paramName)
        {
            XmlNode effectNode = FindParameterContainer(className, effect);
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

        private XmlAttribute GetParamValueAttr(string className, string effect, string param, TweakableType ty)
        {
            XmlNode paramNode = FindParamNode(className, effect, param);
            if (paramNode == null)
                paramNode = CreateParameterNode(className, effect, param, ty);
            XmlAttribute attr = GetAttribute(paramNode, GetParameterTypeString(ty));
            if (attr == null)
                throw new DDXXException("XmlNode " + param + " in effect " + effect +
                    " is not a float parameter: " + paramNode.ToString());
            return attr;
        }

        private XmlNode CreateParameterNode(string className, string effect, string param, TweakableType ty)
        {
            XmlNode effectNode = FindParameterContainer(className, effect);
            if (effectNode.ChildNodes.Count == 0)
                effectNode.AppendChild(doc.CreateWhitespace("\r\n  "));
            XmlNode lastWhitespace = effectNode.LastChild;
            XmlElement newElement = doc.CreateElement("Parameter");
            effectNode.AppendChild(newElement);
            XmlWhitespace newWhitespace = doc.CreateWhitespace(lastWhitespace.Value);
            effectNode.AppendChild(newWhitespace);
            newElement.SetAttribute("name", param);
            newElement.SetAttribute(GetParameterTypeString(ty), "");
            lastWhitespace.Value += "\t";
            return newElement;
        }

        private string FloatToString(float value)
        {
            return value.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        public void SetFloatParam(string className, string effect, string param, float value)
        {
            XmlAttribute attr = GetParamValueAttr(className, effect, param, TweakableType.Float);
            attr.Value = FloatToString(value);
        }

        public void SetIntParam(string className, string effect, string param, int value)
        {
            XmlAttribute attr = GetParamValueAttr(className, effect, param, TweakableType.Integer);
            attr.Value = value.ToString();
        }

        public void SetStringParam(string className, string effect, string param, string value)
        {
            XmlAttribute attr = GetParamValueAttr(className, effect, param, TweakableType.String);
            attr.Value = value;
        }

        public void SetVector2Param(string className, string effect, string param, Vector2 value)
        {
            XmlAttribute attr = GetParamValueAttr(className, effect, param, TweakableType.Vector2);
            string strval = FloatToString(value.X) + ", " +
                FloatToString(value.Y);
            attr.Value = strval;
        }

        public void SetVector3Param(string className, string effect, string param, Vector3 value)
        {
            XmlAttribute attr = GetParamValueAttr(className, effect, param, TweakableType.Vector3);
            string strval = FloatToString(value.X) + ", " +
                FloatToString(value.Y) + ", " +
                FloatToString(value.Z);
            attr.Value = strval;
        }

        public void SetVector4Param(string className, string effect, string param, Vector4 value)
        {
            XmlAttribute attr = GetParamValueAttr(className, effect, param, TweakableType.Vector4);
            string strval = FloatToString(value.X) + ", " +
                FloatToString(value.Y) + ", " +
                FloatToString(value.Z) + ", " +
                FloatToString(value.W);
            attr.Value = strval;
        }

        public void SetColorParam(string className, string effect, string param, Color value)
        {
            XmlAttribute attr = GetParamValueAttr(className, effect, param, TweakableType.Color);
            string strval;
            //if (value.IsNamedColor)
            //{
            //    strval = value.Name;
            //}
            //else
            {
                strval = FloatToString(value.R) + ", " +
                    FloatToString(value.G) + ", " +
                    FloatToString(value.B) + ", " +
                    FloatToString(value.A);
            }
            attr.Value = strval;
        }

        public void SetBoolParam(string className, string effect, string param, bool value)
        {
            XmlAttribute attr = GetParamValueAttr(className, effect, param, TweakableType.Bool);
            if (value)
                attr.Value = "true";
            else
                attr.Value = "false";
        }

        private void ChangeOrCreateAttribute(string className, string effectName, string attrName, string value)
        {
            XmlNode effectNode = FindParameterContainer(className, effectName);
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

        public void SetStartTime(string className, string effectName, float value)
        {
            ChangeOrCreateAttribute(className, effectName, "startTime", FloatToString(value));
        }

        public void SetEndTime(string className, string effectName, float value)
        {
            ChangeOrCreateAttribute(className, effectName, "endTime", FloatToString(value));
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
