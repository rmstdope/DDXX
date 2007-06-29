using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoEffectBuilder
    {

        void AddEffect(string effectName, int effectTrack, float startTime, float endTime);
        void AddPostEffect(string effectName, int effectTrack, float startTime, float endTime);
        void AddTransition(string effectName, int destinationTrack, float startTime, float endTime);
        void AddGenerator(string generatorName, string className);
        void AddTexture(string textureName, string generatorName, int width, int height, int mipLevels);

        void AddFloatParameter(string name, float value, float stepSize);
        void AddIntParameter(string name, int value, float stepSize);
        void AddStringParameter(string name, string value);
        void AddVector3Parameter(string name, Vector3 value, float stepSize);
        void AddColorParameter(string parameterName, Color color);
        void AddBoolParameter(string parameterName, bool color);

        void AddSetupCall(string name, List<object> parameters);

        void AddGeneratorInput(int num, string generatorName);

        void SetSong(string filename);
    }
}
