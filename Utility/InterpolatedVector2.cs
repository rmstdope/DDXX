using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Utility
{
    public class InterpolatedVector2 : IArithmetic
    {
        private Vector2 value;

        public InterpolatedVector2()
        {
        }
        public InterpolatedVector2(Vector2 value)
        {
            this.value = value;
        }
        public InterpolatedVector2(float x, float y)
        {
            this.value = new Vector2(x, y);
        }
        public static implicit operator Vector2(InterpolatedVector2 f)
        {
            return f.value;
        }

        #region IArithmetic Members

        public IArithmetic Zero()
        {
            return new InterpolatedVector2(new Vector2(0, 0));
        }

        public IArithmetic Add(IArithmetic arithmetic)
        {
            if (typeof(InterpolatedVector2) != arithmetic.GetType())
                throw new ArgumentException("Argument not of type InterpolatedVector2");
            return new InterpolatedVector2(value + ((InterpolatedVector2)arithmetic).value);
        }

        public IArithmetic Sub(IArithmetic arithmetic)
        {
            if (typeof(InterpolatedVector2) != arithmetic.GetType())
                throw new ArgumentException("Argument not of type InterpolatedVector2");
            return new InterpolatedVector2(value - ((InterpolatedVector2)arithmetic).value);
        }

        public IArithmetic Mul(float factor)
        {
            return new InterpolatedVector2(value * factor);
        }

        #endregion
    }
}
