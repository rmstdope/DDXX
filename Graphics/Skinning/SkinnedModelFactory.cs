using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    public class SkinnedModelFactory : ModelFactory
    {
        private DdxxAllocateHierarchy allocateHierarchy = new DdxxAllocateHierarchy();

        public SkinnedModelFactory(IDevice device, IGraphicsFactory graphicsFactory, ITextureFactory textureFactory)
            : base(device, graphicsFactory, textureFactory)
        {
        }

        protected override IModel CreateModelFromFile(string file, Options options)
        {
            IModel model;

            if ((options & Options.SkinnedModel) == Options.SkinnedModel)
            {
                IAnimationRootFrame rootFrame = factory.SkinnedMeshFromFile(device, file, allocateHierarchy);
                model = new SkinnedModel(rootFrame, textureFactory);
            }
            else
            {
                model = base.CreateModelFromFile(file, options);
            }

            return model;
        }
    }
}
