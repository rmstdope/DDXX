using System;
using System.Drawing;
using Microsoft.DirectX;
namespace Dope.DDXX.DemoFramework
{
    public interface IEffectChangeListener
    {
        void SetStartTime(string effectName, float value);
        void SetEndTime(string effectName, float value);
        void SetColorParam(string effect, string param, Color value);
        void SetFloatParam(string effect, string param, float vaule);
        void SetIntParam(string effect, string param, int value);
        void SetStringParam(string effect, string param, string value);
        void SetVector3Param(string effect, string param, Vector3 value);
    }
}
