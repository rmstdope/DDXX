using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class MeshContainer
    {
        private IMesh mesh;
        private EffectInstance[] effectInstance;

        public IMesh Mesh
        {
            get { return mesh; }
            set { mesh = value; }
        }

        public EffectInstance[] EffectInstance
        {
            get { return effectInstance; }
            set { effectInstance = value; }
        }

        public MeshContainer(IMesh mesh, EffectInstance[] effectInstance)
        {
            this.mesh = mesh;
            this.effectInstance = effectInstance;
        }
    }
}
