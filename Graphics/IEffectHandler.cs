using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public interface IEffectHandler
    {
        IEffect Effect { get; }
        EffectHandle[] Techniques { get; set;}
        void SetNodeConstants(Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix);
        void SetMaterialConstants(ColorValue ambientColor, ModelMaterial mesh, int index);
    }
}
