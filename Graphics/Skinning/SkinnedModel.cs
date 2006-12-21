using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics.Skinning
{
    public class SkinnedModel : ModelBase
    {
        private IAnimationRootFrame rootFrame;
        private IMesh mesh;

        public SkinnedModel(IAnimationRootFrame rootFrame, ITextureFactory textureFactory)
        {
            this.rootFrame = rootFrame;
            IFrame frame = rootFrame.FrameHierarchy;
            while (frame != null && frame.MeshContainer == null)
            {
                frame = frame.FrameFirstChild;
            }
            if (frame == null)
                throw new DDXXException("No MeshContainer found in the frame1 hierarchy.");

            Materials = CreateModelMaterials(textureFactory, frame.MeshContainer.GetMaterials());
            mesh = frame.MeshContainer.MeshData.Mesh;
        }

        #region IModel Members

        public override IMesh Mesh
        {
            get { return mesh; }
            set { mesh = value; }
        }

        public override void DrawSubset(int subset)
        {
            Mesh.DrawSubset(subset);
        }

        #endregion
    }
}
