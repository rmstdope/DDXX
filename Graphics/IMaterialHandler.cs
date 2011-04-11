using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public interface IMaterialHandler
    {
        Effect Effect { get; set; }
        void SetupRendering(Matrix[] worldMatrices, Matrix viewMatrix, Matrix projectionMatrx, Color ambientLight, LightState lightState);
        void SetupRendering(Matrix[] worldMatrices, Matrix viewMatrix, Matrix projectionMatrx, Color ambientLight);
        Color AmbientColor { get; set; }
        Color DiffuseColor { get; set; }
        Color SpecularColor { get; set; }
        float SpecularPower { get; set; }
        float Shininess { get; set; }
        float Transparency { get; set; }
        Texture2D DiffuseTexture { get; set; }
        Texture2D NormalTexture { get; set; }
        TextureCube ReflectiveTexture { get; set; }
        float ReflectiveFactor { get; set; }
        BlendFunction BlendFunction { get; set; }
        Blend SourceBlend { get; set; }
        Blend DestinationBlend { get; set; }
    }
}
