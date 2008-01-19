using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakableObject : ITweakable
    {
        int NumVisableVariables { get; }
        int NumVariables { get; }
        ITweakable GetTweakableChild(int index);
        void CreateBaseControls(TweakerStatus status, ITweakerSettings settings);
        void NextIndex(TweakerStatus status);
        void IncreaseValue(TweakerStatus status);
        void DecreaseValue(TweakerStatus status);
        void SetValue(TweakerStatus status);
        void ReadFromXmlFile(XmlNode node);
        void WriteToXmlFile(XmlNode node);
    }
}
