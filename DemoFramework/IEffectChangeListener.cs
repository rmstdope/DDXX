using System;
namespace Dope.DDXX.DemoFramework
{
    public interface IEffectChangeListener
    {
        void ColorParamChanged(string effect, string param, string value);
        void EndTimeChanged(string effectName, float value);
        void FloatParamChanged(string effect, string param, string value);
        void IntParamChanged(string effect, string param, string value);
        void StartTimeChanged(string effectName, float value);
        void StringParamChanged(string effect, string param, string value);
        void Vector3ParamChanged(string effect, string param, string value);
    }
}
