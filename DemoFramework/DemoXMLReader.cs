using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.DirectX;

namespace Dope.DDXX.Utility
{
    public enum ParameterType
    {
        Unknown = 0, Integer, Float, String, Vector3
    }

    public struct Parameter
    {
        public string name;
        public object value;
        public ParameterType Type;

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
        public Parameter(string name, ParameterType type, object value)
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
            Stream inputStream = new FileStream(filename, FileMode.Open);
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
            }
        }

        private void ReadParameter()
        {
            bool keepOn = reader.MoveToFirstAttribute();
            ParameterType parameterType = ParameterType.Unknown;
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
                        if (parameterType != ParameterType.Unknown)
                        {
                            parameterValue = value;
                        }
                        break;
                }
                keepOn = reader.MoveToNextAttribute();
            }
            if (parameterName != null && parameterType != ParameterType.Unknown)
            {
                AddParameter(parameterName, parameterType, parameterValue);
            }
            else
            {
                throw new XmlException("Failed to parse parameter");
            }
        }

        private void AddParameter(string parameterName, ParameterType parameterType, string parameterValue)
        {
            switch (parameterType)
            {
                case ParameterType.Float:
                    effectBuilder.AddFloatParameter(parameterName, ParseFloat(parameterValue));
                    break;
                case ParameterType.Integer:
                    effectBuilder.AddIntParameter(parameterName, int.Parse(parameterValue));
                    break;
                case ParameterType.String:
                    effectBuilder.AddStringParameter(parameterName, parameterValue);
                    break;
                case ParameterType.Vector3:
                    string[] s = parameterValue.Split(new char[] { ',' }, 3);
                    Vector3 v = new Vector3(ParseFloat(s[0]), ParseFloat(s[1]), ParseFloat(s[2]));
                    effectBuilder.AddVector3Parameter(parameterName, v);
                    break;
                default:
                    throw new XmlException("Unknown internal parameter type");
            }
        }

        private float ParseFloat(string s)
        {
            return float.Parse(s, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        private ParameterType GetParameterType(string name)
        {
            switch (name)
            {
                case "int": return ParameterType.Integer;
                case "float": return ParameterType.Float;
                case "string": return ParameterType.String;
                case "Vector3": return ParameterType.Vector3;
                default: return ParameterType.Unknown;
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