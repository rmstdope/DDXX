using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    public class DdxxLoadUserData : LoadUserData
    {
        public override void LoadFrameChildData(Frame frame, XFileData xofChildData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void LoadMeshChildData(MeshContainer meshContainer, XFileData xofChildData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void LoadTopLevelData(XFileData xofChildData)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
