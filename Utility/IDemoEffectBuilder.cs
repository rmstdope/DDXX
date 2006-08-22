using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.Utility
{
    public interface IDemoEffectBuilder
    {

        void AddPostEffect(string effectName, int effectTrack);

        void AddEffect(string effectName, int effectTrack);

        void AddFloatParameter(string name, float value);
        void AddIntParameter(string name, int value);
        void AddStringParameter(string name, string value);
        void AddVector3Parameter(string name, Vector3 value);
    }
}
