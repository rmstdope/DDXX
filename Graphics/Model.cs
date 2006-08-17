using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class Model
    {
        private IMesh mesh;
        private ExtendedMaterial[] materials;
        //private EffectInstance[] effectInstance;

        public IMesh Mesh
        {
            get { return mesh; }
            set { mesh = value; }
        }

        public ExtendedMaterial[] Materials
        {
            get { return materials; }
            set { materials = value; }
        }

        //public EffectInstance[] EffectInstance
        //{
        //    get { return effectInstance; }
        //    set { effectInstance = value; }
        //}

        public Model(IMesh mesh, ExtendedMaterial[] materials)
        {
            Mesh = mesh;
            Materials = materials;
        }
    }
}
