using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class ModelMaterial
    {
        private Material material;
        private ITexture diffuseTexture;
        private ITexture normalTexture;
        private ICubeTexture reflectiveTexture;
        private float reflectiveFactor;

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
        public float Shininess
        {
            get { return material.SpecularSharpness; }
            set { material.SpecularSharpness = value; }
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

        public ICubeTexture ReflectiveTexture
        {
            get { return reflectiveTexture; }
            set { reflectiveTexture = value; }
        }

        public float ReflectiveFactor
        {
            get { return reflectiveFactor; }
            set 
            { 
                if (value < 0.0f)
                    throw new DDXXException("Can not set reflective factor to less than zero.");
                if (value > 1.0f)
                    throw new DDXXException("Can not set reflective factor to more than one.");
                reflectiveFactor = value; 
            }
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

        public ModelMaterial(Material material, ITexture diffuseTexture, ITexture normalTexture, ICubeTexture reflectiveTexture)
        {
            this.material = material;
            this.diffuseTexture = diffuseTexture;
            this.normalTexture = normalTexture;
            this.reflectiveTexture = reflectiveTexture;
        }

        public ModelMaterial(Material material, string diffuseTextureName, 
            string normalTextureName, ITextureFactory factory)
        {
            this.material = material;
            this.diffuseTexture = factory.CreateFromFile(diffuseTextureName);
            this.normalTexture = factory.CreateFromFile(normalTextureName);
        }

        public ModelMaterial(Material material, string diffuseTextureName,
            string normalTextureName, string reflectiveTextureName, ITextureFactory factory)
        {
            this.material = material;
            this.diffuseTexture = factory.CreateFromFile(diffuseTextureName);
            this.normalTexture = factory.CreateFromFile(normalTextureName);
            this.reflectiveTexture = factory.CreateCubeFromFile(reflectiveTextureName);
        }

        public ModelMaterial Clone()
        {
            ModelMaterial clone = new ModelMaterial(material);
            clone.ReflectiveFactor = ReflectiveFactor;
            clone.DiffuseTexture = DiffuseTexture;
            clone.NormalTexture = NormalTexture;
            clone.ReflectiveTexture = ReflectiveTexture;
            return clone;
        }
    }
}
