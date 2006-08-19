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

        public Parameter(string name, ParameterType type, object value)
        {
            this.name = name;
            this.Type = type;
            this.value = value;
        }
    }

    public class XMLReader
    {
        private Stream inputStream;
        private XmlReader reader;
        private string effectName;
        private int effectTrack;
        private IDemoEffectBuilder effectBuilder;

        public string EffectName
        {
            get { return effectName; }
        }

        public int EffectTrack
        {
            get { return effectTrack; }
        }

        public XMLReader(IDemoEffectBuilder builder)
        {
            this.effectBuilder = builder;
        }

        public void Read(Stream input)
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
            }
        }

        private void ReadEffect()
        {
            effectName = reader.GetAttribute("name");
            string track = reader.GetAttribute("track");
            if (track != null)
            {
                effectTrack = int.Parse(track);
            }
            else
            {
                effectTrack = 0;
            }
            effectBuilder.AddEffect(effectName, effectTrack);
            ReadParameters();
        }

        public void ReadParameters()
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Effect")
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
