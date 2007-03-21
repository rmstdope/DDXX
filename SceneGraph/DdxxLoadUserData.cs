using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    public class DdxxLoadUserData : LoadUserData
    {
        public override void LoadFrameChildData(Frame frame, XFileData xofChildData)
        {
            if (xofChildData.Type == XFileGuid.Camera)
                System.Diagnostics.Debug.WriteLine("Found camera " + xofChildData.Name);
            else if (xofChildData.Type == XFileGuid.Frame)
                System.Diagnostics.Debug.WriteLine("Found frame " + xofChildData.Name);
            else
                System.Diagnostics.Debug.WriteLine("Found node " + xofChildData.Name);
        }

        public override void LoadMeshChildData(MeshContainer meshContainer, XFileData xofChildData)
        {
            System.Diagnostics.Debug.WriteLine("Found mesh " + xofChildData.Name);
        }

        public override void LoadTopLevelData(XFileData xofChildData)
        {
            if (xofChildData.Type == XFileGuid.Camera)
                System.Diagnostics.Debug.WriteLine("Found camera " + xofChildData.Name);
            else if (xofChildData.Type == XFileGuid.Frame)
                System.Diagnostics.Debug.WriteLine("Found frame " + xofChildData.Name);
            else
                System.Diagnostics.Debug.WriteLine("Found node " + xofChildData.Name);
        }
    }
}
