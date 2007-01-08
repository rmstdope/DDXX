using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics.Skinning
{
    public class FrameAdapter : IFrame
    {
        private Frame frame;
        private ModelMaterial[] modelMaterials;

        public ModelMaterial[] ModelMaterials
        {
            get { return modelMaterials; }
            set { modelMaterials = value; }
        }

        public FrameAdapter(Frame frame)
        {
            this.frame = frame;
        }

        #region IFrame Members

        public IFrame FrameFirstChild
        {
            get 
            { 
                if (frame.FrameFirstChild == null)
                    return null;
                else
                    return new FrameAdapter(frame.FrameFirstChild);
            }
        }

        public IFrame FrameSibling
        {
            get 
            { 
                if (frame.FrameSibling == null)
                    return null;
                else
                    return new FrameAdapter(frame.FrameSibling);
            }
        }

        public IMeshContainer MeshContainer
        {
            get
            {
                if (frame.MeshContainer == null)
                    return null;
                else
                    return new MeshContainerAdapter(frame.MeshContainer);
            }
            set
            {
                frame.MeshContainer = ((MeshContainerAdapter)value).DXMeshContainer;
            }
        }

        public string Name
        {
            get
            {
                return frame.Name;
            }
            set
            {
                frame.Name = value;
            }
        }

        public Matrix TransformationMatrix
        {
            get
            {
                return frame.TransformationMatrix;
            }
            set
            {
                frame.TransformationMatrix = value;
            }
        }

        #endregion
    }
}
