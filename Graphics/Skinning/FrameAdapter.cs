using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public class FrameAdapter : IFrame
    {
        private SkinnedFrame frame;

        public FrameAdapter(SkinnedFrame frame)
        {
            this.frame = frame;
        }

        public SkinnedFrame DxFrame
        {
            get
            {
                return frame;
            }
        }

        #region IFrame Members

        public IFrame FrameFirstChild
        {
            get 
            { 
                if (frame.FrameFirstChild == null)
                    return null;
                else
                    return new FrameAdapter(frame.FrameFirstChild as SkinnedFrame);
            }
        }

        public IFrame FrameSibling
        {
            get 
            { 
                if (frame.FrameSibling == null)
                    return null;
                else
                    return new FrameAdapter(frame.FrameSibling as SkinnedFrame);
            }
        }

        public IMeshContainer MeshContainer
        {
            get
            {
                if (frame.MeshContainer == null)
                    return null;
                else
                    return new MeshContainerAdapter(frame.MeshContainer as SkinnedMeshContainer);
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

        public IFrame Find(IFrame rootFrame, string name)
        {
            return new FrameAdapter(Frame.Find(((FrameAdapter)rootFrame).DxFrame, name) as SkinnedFrame);
        }

        public Matrix CombinedTransformationMatrix
        {
            get
            {
                return frame.CombinedTransformationMatrix;
            }
            set
            {
                frame.CombinedTransformationMatrix = value;
            }
        }

        public IMesh Mesh 
        {
            get
            {
                if (MeshContainer == null)
                    return null;
                return MeshContainer.MeshData.Mesh;
            }
        }

        public ISkinInformation SkinInformation
        {
            get
            {
                if (MeshContainer == null)
                    return null;
                return MeshContainer.SkinInformation;
            }
        }

        public ExtendedMaterial[] ExtendedMaterials
        {
            get
            {
                if (MeshContainer == null)
                    return null;
                return MeshContainer.GetMaterials(); ;
            }
        }

        #endregion
    }
}
