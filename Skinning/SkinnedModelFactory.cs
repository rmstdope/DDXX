using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Skinning
{
    public class SkinnedModelFactory : ModelFactory
    {
        private SkinnedAllocateHierarchy allocateHierarchy = new SkinnedAllocateHierarchy();

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
                model = new SkinnedModel(rootFrame);
            }
            else
            {
                model = base.CreateModelFromFile(file, options);
            }

            return model;
        }
    }
}
