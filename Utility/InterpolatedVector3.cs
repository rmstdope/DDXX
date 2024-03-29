using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Utility
{
    public class InterpolatedVector3 : IArithmetic
    {
        private Vector3 value;

        public InterpolatedVector3()
        {
        }
        
        public InterpolatedVector3(Vector3 value)
        {
            this.value = value;
        }
        
        public static implicit operator Vector3(InterpolatedVector3 f)
        {
            return f.value;
        }

        public static InterpolatedVector3 operator +(InterpolatedVector3 v1, Vector3 v2)
        {
            return new InterpolatedVector3(v1.value + v2);
        }

        #region IArithmetic Members

        public IArithmetic Zero()
        {
            return new InterpolatedVector3(new Vector3(0, 0, 0));
        }

        public IArithmetic Add(IArithmetic arithmetic)
        {
            if (typeof(InterpolatedVector3) != arithmetic.GetType())
                throw new ArgumentException("Argument not of type InterpolatedVector3");
            return new InterpolatedVector3(value + ((InterpolatedVector3)arithmetic).value);
        }

        public IArithmetic Sub(IArithmetic arithmetic)
        {
            if (typeof(InterpolatedVector3) != arithmetic.GetType())
                throw new ArgumentException("Argument not of type InterpolatedVector3");
            return new InterpolatedVector3(value - ((InterpolatedVector3)arithmetic).value);
        }

        public IArithmetic Mul(float factor)
        {
            return new InterpolatedVector3(value * factor);
        }

        #endregion
    }
}
