using System;

namespace Dope.DDXX.Graphics
{
    public interface IModel
    {
        ModelMaterial[] Materials { get; set; }
        IMesh Mesh { get; set; }
        void DrawSubset(int subset);
        bool IsSkinned();
        void Draw();
    }
}
