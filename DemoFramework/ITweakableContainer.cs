using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakableContainer
    {
        TweakableType GetTweakableType(int num);
        int GetTweakableNumber(string name);
        int GetNumTweakables();
        float GetIntValue(int num);
        float GetFloatValue(int num);
        Vector3 GetVector3Value(int num);
        string GetStringValue(int num);
        Color GetColorValue(int num);
        void SetValue(int num, object value);
        void SetStepSize(int num, float size);
        float GetStepSize(int num);
        //void SetStepSize(string variableName, float size)
        string GetTweakableName(int index);
    }
}
