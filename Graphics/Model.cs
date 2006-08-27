using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class Model// : MeshContainer
    {
        private IMesh mesh;
        ModelMaterial[] materials;

        public ModelMaterial[] Materials
        {
            get { return materials; }
            set { materials = value; }
        }

        public IMesh IMesh
        {
            get { return mesh; }
            set { mesh = value; }
        }

        public Model(IMesh mesh, ModelMaterial[] materials)
        {
            IMesh = mesh;
            Materials = materials;
        }
    }
}
