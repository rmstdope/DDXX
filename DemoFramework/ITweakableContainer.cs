using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakableContainer
    {
        TweakableType GetTweakableType(int num);
        int GetTweakableNumber(string name);
        int GetNumTweakables();
        int GetIntValue(int num);
        float GetFloatValue(int num);
        Vector2 GetVector2Value(int num);
        Vector3 GetVector3Value(int num);
        Vector4 GetVector4Value(int num);
        string GetStringValue(int num);
        Color GetColorValue(int num);
        bool GetBoolValue(int num);
        void SetValue(int num, object value);
        void SetStepSize(int num, float size);
        float GetStepSize(int num);
        string GetTweakableName(int index);
        void UpdateListener(IEffectChangeListener changeListener);
    }
}
