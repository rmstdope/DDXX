using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Graphics
{
    public interface IEffectConverter
    {
        void Convert(IEffect oldEffect, IEffect newEffect);
    }
}
