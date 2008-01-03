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

        public ModelFactory(IGraphicsDevice device, IGraphicsFactory factory,
            ITextureFactory textureFactory)
        {
            this.device = device;
            this.factory = factory;
            this.textureFactory = textureFactory;
        }

        public IModel FromFile(string file, string effect)
        {
            IModel model = factory.ModelFromFile(file);
            foreach (IModelMesh mesh in model.Meshes)
            {
                foreach (IModelMeshPart part in mesh.MeshParts)
                {
                    IEffect newEffect = factory.EffectFromFile(effect);
                    part.Effect = newEffect;
                }
            }
            return model;
        }

        //private void CopyEffectParameters(IEffect fromEffect, IEffect toEffect)
        //{
        //    foreach (IEffectParameter fromParameter in fromEffect.Parameters)
        //    {
        //        IEffectParameter toParameter = toEffect.Parameters[fromParameter.Name];
        //        if (toParameter != null)
        //        {
        //            toParameter.get

        //        }
        //    }
        //}

        //public IModel FromFile(string file, MaterialEffectChooser techniqueChooser)
        //{
        //    IModel model = factory.ModelFromFile(file);
        //    return model;
        //}

        //public IModel FromFile(string file, MeshEffectChooser techniqueChooser)
        //{
        //    IModel model = factory.ModelFromFile(file);
        //    return model;
        //}

    }
}
