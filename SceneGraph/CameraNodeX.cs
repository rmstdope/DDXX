using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics.Skinning;
using Dope.DDXX.Utility;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public class CameraNodeX : CameraNode
    {
        private SkinnedModel model;
        private IFrame frame;

        public CameraNodeX(SkinnedModel model, String name)
            : base(name)
        {
            this.model = model;
            frame = model.GetFrame(name);
            if (frame == null)
                throw new DDXXException("Can not create CameraNodeX. No Camera named " + name + " found in model.");
            SetFOV((float)Math.PI / 2);
            SetClippingPlanes(1.0f, 100000.0f);
            WorldState.Roll(-(float)Math.PI / 2);
            WorldState.Tilt((float)Math.PI / 1.6f);
        }

        protected override void StepNode()
        {
            // 3DS -> DX conversion
            Matrix m2 = Matrix.Identity;
            m2.M11 = -1;
            m2.M22 = 0;
            m2.M23 = 1;
            m2.M33 = 0;
            m2.M32 = 1;
            Matrix m3 = frame.TransformationMatrix * m2;
            WorldState.Position = new Vector3(m3.M41, m3.M42, m3.M43) * 100.0f;
            Quaternion q = Quaternion.RotationMatrix(m3);
            WorldState.Rotation = q;
        }

    }
}
