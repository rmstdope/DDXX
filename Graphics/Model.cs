using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class Model : ModelBase
    {
        private IMesh mesh;

        public IMesh Mesh
        {
            get { return mesh; }
            set { mesh = value; }
        }

        public Model(IMesh mesh)
        {
            this.mesh = mesh;
            Materials = new ModelMaterial[] { new ModelMaterial(new Material()) };
        }

        public Model(IMesh mesh, ModelMaterial[] materials)
        {
            this.mesh = mesh;
            Materials = materials;
        }

        public Model(IMesh mesh, ITextureFactory textureFactory, ExtendedMaterial[] extendedMaterials)
        {
            this.mesh = mesh;
            Materials = CreateModelMaterials(textureFactory, extendedMaterials);
        }

        public void DrawSubset(int subset)
        {
            mesh.DrawSubset(subset);
        }
    }
}
