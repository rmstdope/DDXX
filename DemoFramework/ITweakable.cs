using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakable
    {
        int NumVisableVariables { get; }
        int NumVariables { get; }
        //string GetVariableName(int entry);
        ITweakable GetVariable(int index);
        void CreateBaseControls(TweakerStatus status, ITweakerSettings settings);
        void CreateVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings);
        void NextIndex(TweakerStatus status);
        void IncreaseValue(TweakerStatus status);
        void DecreaseValue(TweakerStatus status);
        void SetValue(TweakerStatus status);
    }
}
