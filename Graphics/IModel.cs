using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface IModel
    {
        ModelMaterial[] Materials { get; set; }
        IMesh Mesh { get; set; }
        void DrawSubset(int subset);
        bool IsSkinned();
        void Draw(IEffectHandler effectHandler, ColorValue ambient, Matrix world, Matrix view, Matrix projection);
    }
}
