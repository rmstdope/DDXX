using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;

namespace Dope.DDXX.Graphics
{
    public class PhysicalModel : Model 
    {
        private IBody body;

        public PhysicalModel(IMesh mesh, IBody body)
            : base(mesh)
        {
            this.body = body;
        }

        public override void Step()
        {
            //base.Step();
            body.Step();
        }
    }
}
