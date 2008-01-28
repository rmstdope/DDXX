using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakable
    {
        void CreateVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings);
        void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings);
        int Dimension { get; }
        void IncreaseValue(int index);
        void DecreaseValue(int index);
        void SetFromString(string value);
        void SetFromString(int index, string value);
        PropertyInfo Property { get; }
        string GetToString();
        int NumVisableVariables { get; }
        int NumVariables { get; }
        ITweakable GetTweakableChild(int index);
        bool IsObject();
        void CreateBaseControls(TweakerStatus status, ITweakerSettings settings);
        void NextIndex(TweakerStatus status);
        void IncreaseValue(TweakerStatus status);
        void DecreaseValue(TweakerStatus status);
        void SetValue(TweakerStatus status);
        void ReadFromXmlFile(XmlNode node);
        void WriteToXmlFile(XmlDocument xmlDocument, XmlNode node);
    }
}
