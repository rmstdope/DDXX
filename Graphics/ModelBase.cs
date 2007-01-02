using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public abstract class ModelBase : IModel
    {
        private ModelMaterial[] materials;

        protected ModelMaterial[] CreateModelMaterials(ITextureFactory textureFactory, ExtendedMaterial[] extendedMaterials)
        {
            ModelMaterial[] modelMaterials = new ModelMaterial[extendedMaterials.Length];
            for (int i = 0; i < extendedMaterials.Length; i++)
            {
                if (extendedMaterials[i].TextureFilename == null ||
                    extendedMaterials[i].TextureFilename == "")
                    modelMaterials[i] = new ModelMaterial(extendedMaterials[i].Material3D);
                else
                    modelMaterials[i] = new ModelMaterial(extendedMaterials[i].Material3D, textureFactory.CreateFromFile(extendedMaterials[i].TextureFilename));
                modelMaterials[i].Ambient = modelMaterials[i].Diffuse;
            }
            return modelMaterials;
        }

        #region IModel Members

        public abstract IMesh Mesh
        { get; set; }

        public ModelMaterial[] Materials
        {
            get { return materials; }
            set { materials = value; }
        }

        public abstract void DrawSubset(int subset);

        public virtual bool IsSkinned()
        { 
            return false; 
        }

        public abstract void Draw();

        #endregion
    }
}
