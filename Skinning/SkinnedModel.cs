using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.Graphics
{
    public class SkinnedModel : ModelBase
    {
        private IAnimationRootFrame rootFrame;
        public SkinnedModel(IAnimationRootFrame rootFrame)
        {
            this.rootFrame = rootFrame;
        }

        #region IModel Members

        public override IMesh Mesh
        {
            get { return rootFrame.FrameHierarchy.MeshContainer.MeshData.Mesh; }
            set { rootFrame.FrameHierarchy.MeshContainer.MeshData.Mesh = value; }
        }

        public override void DrawSubset(int subset)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
