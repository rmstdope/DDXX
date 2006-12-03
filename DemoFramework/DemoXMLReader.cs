using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.DirectX;
using System.Drawing;

namespace Dope.DDXX.Utility
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

    public class DemoXMLReader
    {
        private Stream inputStream;
        private XmlReader reader;
        private IDemoEffectBuilder effectBuilder;


        public DemoXMLReader(IDemoEffectBuilder builder)
        {
            this.effectBuilder = builder;
        }

        public void Read(string filename)
        {
            Stream inputStream = new FileStream(FileUtility.FilePath(filename), FileMode.Open);
            Read(inputStream);
        }
        private void Read(Stream input)
        {
            this.inputStream = input;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            reader = XmlReader.Create(inputStream, settings);
            //reader.MoveToContent();
            try
            {
                while (reader.Read())
                {
                    // PrintElement(reader);
                    if (reader.NodeType != XmlNodeType.Element)
                        continue;
                    if (reader.Name == "Effects")
                    {
                        ReadEffects();
                    }
                    else
                    {
                        throw new XmlException("Expected <Effects>, found <" + reader.Name + ">");
                    }
                }
            }
            finally
            {
                Close();
            }
        }

        public void Close()
        {
            reader.Close();
            inputStream.Close();
        }

        public void ReadEffects()
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Effect")
                {
                    ReadEffect();
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.Name == "PostEffect")
                {
                    ReadPostEffect();
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Transition")
                {
                    ReadTransition();
                }
            }
        }

        private void ReadEffect()
        {
            string effectName;
            int effectTrack;
            float startTime;
            float endTime;
            ReadNameTrack(out effectName, out effectTrack, out startTime, out endTime);
            effectBuilder.AddEffect(effectName, effectTrack, startTime, endTime);
            ReadParameters();
        }

        private void ReadPostEffect()
        {
            string effectName;
            int effectTrack;
            float startTime;
            float endTime;
            ReadNameTrack(out effectName, out effectTrack, out startTime, out endTime);
            effectBuilder.AddPostEffect(effectName, effectTrack, startTime, endTime);
            ReadParameters();
        }

        private void ReadTransition()
        {
            string effectName = reader.GetAttribute("name");
            string track = reader.GetAttribute("destinationTrack");
            int destinationTrack;
            if (track != null)
            {
                destinationTrack = int.Parse(track);
            }
            else
            {
                destinationTrack = 0;
            }
            effectBuilder.AddTransition(effectName, destinationTrack);
            ReadParameters();
        }

        private void ReadNameTrack(out string effectName, out int effectTrack, out float startTime, out float endTime)
        {
            effectName = reader.GetAttribute("name");
            string track = reader.GetAttribute("track");
            string start = reader.GetAttribute("startTime");
            string end = reader.GetAttribute("endTime");
            effectTrack = 0;
            startTime = 0.0F;
            endTime = 0.0F;
            if (track != null)
            {
                effectTrack = int.Parse(track);
            }
            if (start != null)
            {
                startTime = ParseFloat(start);
            }
            if (end != null)
            {
                endTime = ParseFloat(end);
            }
        }

        public void ReadParameters()
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    (reader.Name == "Effect" ||
                    reader.Name == "PostEffect" ||
                    reader.Name == "Transition"))
                {
                    break;
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Parameter")
                {
                    ReadParameter();
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SetupCall")
                {
                    ReadSetupCall();
                }
                else
                {
                    throw new DDXXException("Unknown tag in XML file.");
                }
            }
        }

        private void ReadParameter()
        {
            bool keepOn = reader.MoveToFirstAttribute();
            TweakableType parameterType = TweakableType.Unknown;
            string parameterName = null;
            string parameterValue = "";
            while (keepOn)
            {
                string name = reader.Name;
                string value = reader.Value;
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
                keepOn = reader.MoveToNextAttribute();
            }
            if (parameterName != null && parameterType != TweakableType.Unknown)
            {
                AddParameter(parameterName, parameterType, parameterValue);
            }
            else
            {
                throw new XmlException("Failed to parse parameter");
            }
        }

        private void ReadSetupCall()
        {
            bool keepOn = reader.MoveToFirstAttribute();
            if (!keepOn || reader.Name != "name")
                throw new DDXXException("Could not parse setup call.");
            string name = reader.Value;
            List<Object> parameters = new List<Object>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    (reader.Name == "SetupCall"))
                {
                    break;
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Parameter")
                {
                    if (!reader.MoveToFirstAttribute())
                        throw new DDXXException("Failed to parse parameters for setup call.");
                    switch (GetParameterType(reader.Name))
                    {
                        case TweakableType.Float:
                            parameters.Add(ParseFloat(reader.Value));
                            break;
                        case TweakableType.Integer:
                            parameters.Add(int.Parse(reader.Value));
                            break;
                        case TweakableType.String:
                            parameters.Add(reader.Value);
                            break;
                        case TweakableType.Vector3:
                            string[] s = reader.Value.Split(new char[] { ',' }, 3);
                            Vector3 v = new Vector3(ParseFloat(s[0]), ParseFloat(s[1]), ParseFloat(s[2]));
                            parameters.Add(v);
                            break;
                        case TweakableType.Color:
                            parameters.Add(Color.FromName(reader.Value));
                            break;
                        default:
                            throw new XmlException("Unknown internal parameter type");
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
                    throw new XmlException("Unknown internal parameter type");
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

        #region debugging
        private void PrintElement()
        {
            Console.WriteLine("{0}: {1} = {2}",
                reader.NodeType.ToString(), reader.Name, reader.Value);
            for (int i = 0, c = reader.AttributeCount; i < c; i++)
            {
                reader.MoveToAttribute(i);
                Console.WriteLine("  {0} = {1} ", reader.Name, reader.Value);
            }
            reader.MoveToElement();
        }
        #endregion

    }
}
