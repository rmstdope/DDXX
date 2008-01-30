using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public interface ITextureGenerator
    {
        int NumInputPins { get; }
        ITextureGenerator GetInput(int inputPin);
        Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize);
        void ConnectToInput(int inputPin, ITextureGenerator outputGenerator);
    }
}
