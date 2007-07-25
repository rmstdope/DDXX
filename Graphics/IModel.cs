using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface IModel
    {
        ModelMaterial[] Materials { get; set; }
        IMesh Mesh { get; set; }
        bool IsSkinned();
        void Render(IDevice device, IEffectHandler effectHandler, ColorValue ambient, Matrix world, Matrix view, Matrix projection);
        void Step();
        /// <summary>
        /// Clones the materials of the Model. The Mesh still remains the same Mesh for time being.
        /// </summary>
        /// <returns>The clones IModel.</returns>
        IModel Clone();
        Cull CullMode { get; set; }
        bool UseStencil { get; set; }
        int StencilReference { get; set; }
    }
}
