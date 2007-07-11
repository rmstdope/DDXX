using System;
using System.Drawing;
using Microsoft.DirectX;
namespace Dope.DDXX.DemoFramework
{
    public interface IEffectChangeListener
    {
        void SetStartTime(string className, string effectName, float value);
        void SetEndTime(string className, string effectName, float value);
        void SetColorParam(string className, string effectName, string param, Color value);
        void SetFloatParam(string className, string effectName, string param, float value);
        void SetIntParam(string className, string effectName, string param, int value);
        void SetStringParam(string className, string effectName, string param, string value);
        void SetVector3Param(string className, string effectName, string param, Vector3 value);
        void SetBoolParam(string className, string effectName, string param, bool value);
    }
}
