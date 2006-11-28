using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;

namespace Dope.DDXX.Utility
{
    public interface IDemoEffectBuilder
    {

        void AddEffect(string effectName, int effectTrack, float startTime, float endTime);
        void AddPostEffect(string effectName, int effectTrack, float startTime, float endTime);
        void AddTransition(string effectName, int destinationTrack);

        void AddFloatParameter(string name, float value);
        void AddIntParameter(string name, int value);
        void AddStringParameter(string name, string value);
        void AddVector3Parameter(string name, Vector3 value);
        void AddColorParameter(string parameterName, Color color);

        void AddSetupCall(string name, List<object> parameters);
    }
}
