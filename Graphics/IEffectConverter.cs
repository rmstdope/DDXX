using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IEffectConverter
    {
        void Convert(Effect oldEffect, Effect newEffect);
    }
}
