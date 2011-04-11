using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class ModelFactory : IModelFactory
    {
        private IGraphicsFactory factory;
        private TextureFactory textureFactory;
        private List<ModelParameters> models;

        public ModelFactory(IGraphicsFactory factory, TextureFactory textureFactory)
        {
            this.factory = factory;
            this.textureFactory = textureFactory;
            models = new List<ModelParameters>();
        }

        public CustomModel CreateFromName(string file, string effect)
        {
            ModelParameters modelParam = models.Find(delegate(ModelParameters parameters)
            {
                return parameters.Name == file && parameters.Effect == effect;
            });
            if (modelParam != null)
                return modelParam.Model;
            CustomModel model = factory.ModelFromFile(file);
            foreach (CustomModelMesh mesh in model.Meshes)
            {
                foreach (CustomModelMeshPart part in mesh.MeshParts)
                {
                    Effect newEffect = factory.EffectFromFile(effect);
                    part.MaterialHandler.Effect = newEffect;
                }
            }
            models.Add(new ModelParameters(file, effect, model));
            return model;
        }

        public List<ModelParameters> ModelParameters
        {
            get { return models; }
        }

        public void Update(ModelParameters Target)
        {
        }

    }
}
