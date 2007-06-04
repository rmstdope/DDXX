using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace TextureBuilder
{
    public interface IGenerator
    {
        Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize);
        void ConnectToInput(int inputPin, IGenerator outputGenerator);
    }
}
