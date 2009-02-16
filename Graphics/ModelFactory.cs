using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class ModelFactory : IModelFactory
    {
        private IGraphicsFactory factory;
        private ITextureFactory textureFactory;
        private List<ModelParameters> models;

        public ModelFactory(IGraphicsFactory factory, ITextureFactory textureFactory)
        {
            this.factory = factory;
            this.textureFactory = textureFactory;
            models = new List<ModelParameters>();
        }

        public IModel CreateFromName(string file, string effect)
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

        public List<ModelParameters> ModelParameters
        {
            get { return models; }
        }

        public void Update(ModelParameters Target)
        {
        }

    }
}
