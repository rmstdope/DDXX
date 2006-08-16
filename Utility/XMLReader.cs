using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Dope.DDXX.Utility
{
    public enum ParameterType
    {
        Unknown = 0, Integer, Float, String
    }

    public struct Parameter
    {
        public string Name;
        public string Value;
        public ParameterType Type;
        public Parameter(string name, ParameterType type, string value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }
    }

    public class XMLReader
    {
        private Stream inputStream;
        private XmlReader reader;
        private string effectName;
        private int effectTrack;

        public string EffectName
        {
            get { return effectName; }
        }

        public int EffectTrack
        {
            get { return effectTrack; }
        }

        public void Start(Stream input)
        {
            this.inputStream = input;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            reader = XmlReader.Create(inputStream, settings);
            //reader.MoveToContent();
            while (reader.Read())
            {
                // PrintElement(reader);
                if (reader.NodeType != XmlNodeType.Element)
                    continue;
                if (reader.Name == "Effects")
                {
                    return;
                }
                else
                {
                    throw new XmlException("Expected <Effects>, found <" + reader.Name + ">");
                }
            }
        }

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

        public void Close()
        {
            reader.Close();
            inputStream.Close();
        }

        public bool NextEffect()
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Effect")
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
                    return true;
                }
            }
            return false;
        }

        public Dictionary<string, Parameter> GetParameters()
        {
            Dictionary<string, Parameter> parameters = new Dictionary<string, Parameter>();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Effect")
                {
                    break;
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Parameter")
                {
                    Parameter p = ReadParameter();
                    parameters.Add(p.Name, p);
                }
            }
            return parameters;
        }

        private Parameter ReadParameter()
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
                return new Parameter(parameterName, parameterType, parameterValue);
            }
            else
            {
                throw new XmlException("Failed to parse parameter");
            }
        }

        private ParameterType GetParameterType(string name)
        {
            switch (name)
            {
                case "int": return ParameterType.Integer;
                case "float": return ParameterType.Float;
                case "string": return ParameterType.String;
                default: return ParameterType.Unknown;
            }
        }
    }
}
