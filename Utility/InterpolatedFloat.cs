using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public class InterpolatedFloat : IArithmetic
    {
        private float value;

        public InterpolatedFloat()
        {
        }
        public InterpolatedFloat(float value)
        {
            this.value = value;
        }
        public static implicit operator float(InterpolatedFloat f)
        {
            return f.value;
        }

        #region IArithmetic Members

        public IArithmetic Zero()
        {
            return new InterpolatedFloat(0.0f);
        }

        public IArithmetic Add(IArithmetic arithmetic)
        {
            if (typeof(InterpolatedFloat) != arithmetic.GetType())
                throw new ArgumentException("Argument not of type InterpolatedFloat");
            return new InterpolatedFloat(value + ((InterpolatedFloat)arithmetic).value);
        }

        public IArithmetic Sub(IArithmetic arithmetic)
        {
            if (typeof(InterpolatedFloat) != arithmetic.GetType())
                throw new ArgumentException("Argument not of type InterpolatedFloat");
            return new InterpolatedFloat(value - ((InterpolatedFloat)arithmetic).value);
        }
        
        public IArithmetic Mul(float factor)
        {
            return new InterpolatedFloat(value * factor);
        }

        #endregion
    }
}
