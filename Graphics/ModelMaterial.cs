using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.Graphics
{
    public class ModelMaterial
    {
        private Material material;
        private ITexture diffuseTexture;
        private ITexture normalTexture;

        public Color Ambient 
        {
            get { return material.Ambient; } 
            set { material.Ambient = value; }
        }
        public ColorValue AmbientColor
        {
            get { return material.AmbientColor; }
            set { material.AmbientColor = value; }
        }
        public Color Diffuse
        {
            get { return material.Diffuse; } 
            set { material.Diffuse = value; }
        }
        public ColorValue DiffuseColor
        {
            get { return material.DiffuseColor; }
            set { material.DiffuseColor = value; }
        }
        public Color Specular
        {
            get { return material.Specular; }
            set { material.Specular = value; }
        }
        public ColorValue SpecularColor
        {
            get { return material.SpecularColor; }
            set { material.SpecularColor = value; }
        }

        public ITexture DiffuseTexture
        {
            get { return diffuseTexture; }
            set { diffuseTexture = value; }
        }

        public ITexture NormalTexture
        {
            get { return normalTexture; }
            set { normalTexture = value; }
        }

        public ModelMaterial(Material material)
        {
            this.material = material;
            this.diffuseTexture = null;
            this.normalTexture = null;
        }

        public ModelMaterial(Material material, ITexture diffuseTexture)
        {
            this.material = material;
            this.diffuseTexture = diffuseTexture;
            this.normalTexture = null;
        }

        public ModelMaterial(Material material, string diffuseTextureName, ITextureFactory factory)
        {
            this.material = material;
            this.diffuseTexture = factory.CreateFromFile(diffuseTextureName);
            this.normalTexture = null;
        }

        public ModelMaterial(Material material, ITexture diffuseTexture, ITexture normalTexture)
        {
            this.material = material;
            this.diffuseTexture = diffuseTexture;
            this.normalTexture = normalTexture;
        }

        public ModelMaterial(Material material, string diffuseTextureName, 
            string normalTextureName, ITextureFactory factory)
        {
            this.material = material;
            this.diffuseTexture = factory.CreateFromFile(diffuseTextureName);
            this.normalTexture = factory.CreateFromFile(normalTextureName);
        }

    }
}
