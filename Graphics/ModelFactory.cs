using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class ModelFactory : IModelFactory
    {
        private IGraphicsFactory factory;
        private IGraphicsDevice device;
        private ITextureFactory textureFactory;
        private List<ModelParameters> models;

        public ModelFactory(IGraphicsDevice device, IGraphicsFactory factory,
            ITextureFactory textureFactory)
        {
            this.device = device;
            this.factory = factory;
            this.textureFactory = textureFactory;
            models = new List<ModelParameters>();
        }

        public IModel FromFile(string file, string effect)
        {
            ModelParameters modelParam = models.Find(delegate(ModelParameters parameters)
            {
                return parameters.Name == file && parameters.Effect == effect;
            });
            if (modelParam != null)
                return modelParam.Model;
            IModel model = factory.ModelFromFile(file);
            foreach (IModelMesh mesh in model.Meshes)
            {
                foreach (IModelMeshPart part in mesh.MeshParts)
                {
                    IEffect newEffect = factory.EffectFromFile(effect);
                    part.Effect = newEffect;
                }
            }
            models.Add(new ModelParameters(file, effect, model));
            return model;
        }

    }
}
