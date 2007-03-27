using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;

namespace Dope.DDXX.Graphics
{
    public class PhysicalModel : Model 
    {
        public PhysicalModel(IMesh mesh, IBody body)
            : base(mesh)
        {
        }

        //public static PhysicalModel CreateCloth(float width, float height)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}
    }
}
