using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class ModelMaterial
    {
        private Material material;
        private ITexture diffuseTexture;

        public Material Material
        {
            get { return material; }
            set { material = value; }
        }

        public ITexture DiffuseTexture
        {
            get { return diffuseTexture; }
            set { diffuseTexture = value; }
        }

        public ModelMaterial(Material material)
        {
            this.material = material;
            this.diffuseTexture = null;
        }

        public ModelMaterial(Material material, ITexture diffuseTexture)
        {
            this.material = material;
            this.diffuseTexture = diffuseTexture;
        }
    }
}
