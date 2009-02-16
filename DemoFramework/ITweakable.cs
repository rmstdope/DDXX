using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using Microsoft.Xna.Framework.Input;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakable
    {
        void CreateChildControl(TweakerStatus status, int index, float y, ITweakerSettings settings);
        void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings);
        int NumVisableVariables { get; }
        int NumVariables { get; }
        ITweakable GetChild(int index);
        void CreateBaseControls(TweakerStatus status, ITweakerSettings settings);
        void NextIndex(TweakerStatus status);
        void IncreaseValue(TweakerStatus status);
        void DecreaseValue(TweakerStatus status);
        void SetValue(TweakerStatus status);
        void ReadFromXmlFile(XmlNode node);
        void WriteToXmlFile(XmlDocument xmlDocument, XmlNode node);
        void Regenerate(TweakerStatus status);
        void InsertNew(TweakerStatus status);
    }
}
