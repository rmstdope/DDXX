using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public interface IMaterialHandler
    {
        IEffect Effect { get; set; }
        void SetupRendering(Matrix[] worldMatrices, Matrix viewMatrix, Matrix projectionMatrx, Color ambientLight, LightState lightState);
        Color AmbientColor { get; set; }
        Color DiffuseColor { get; set; }
        Color SpecularColor { get; set; }
        float SpecularPower { get; set; }
        float Shininess { get; set; }
        ITexture2D DiffuseTexture { get; set; }
        ITexture2D NormalTexture { get; set; }
        ITextureCube ReflectiveTexture { get; set; }
        float ReflectiveFactor { get; set; }
        BlendFunction BlendFunction { get; set; }
        Blend SourceBlend { get; set; }
        Blend DestinationBlend { get; set; }
    }
}
