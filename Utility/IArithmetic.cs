using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public interface IArithmetic
    {
        IArithmetic Zero();
        IArithmetic Add(IArithmetic value);
        IArithmetic Sub(IArithmetic value);
        IArithmetic Mul(float value);
    }
}
