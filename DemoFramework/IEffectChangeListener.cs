using System;
using System.Drawing;
using Microsoft.DirectX;
namespace Dope.DDXX.DemoFramework
{
    public interface IEffectChangeListener
    {
        void SetStartTime(string effectName, float value);
        void SetEndTime(string effectName, float value);
        void SetColorParam(string effectName, string param, Color value);
        void SetFloatParam(string effectName, string param, float value);
        void SetIntParam(string effectName, string param, int value);
        void SetStringParam(string effectName, string param, string value);
        void SetVector3Param(string effectName, string param, Vector3 value);
        void SetBoolParam(string effectName, string param, bool value);
    }
}
