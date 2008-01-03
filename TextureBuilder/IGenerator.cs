using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public interface IGenerator
    {
        int NumInputPins { get; }
        IGenerator GetInput(int inputPin);
        Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize);
        void ConnectToInput(int inputPin, IGenerator outputGenerator);
    }
}
