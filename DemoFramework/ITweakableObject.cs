using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakableObject : ITweakable
    {
        int NumVisableVariables { get; }
        int NumVariables { get; }
        ITweakableObject GetVariable(int index);
        void CreateBaseControls(TweakerStatus status, ITweakerSettings settings);
        void NextIndex(TweakerStatus status);
        void IncreaseValue(TweakerStatus status);
        void DecreaseValue(TweakerStatus status);
        void SetValue(TweakerStatus status);
        void UpdateListener(IEffectChangeListener changeListener);
    }
}
