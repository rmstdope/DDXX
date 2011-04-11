using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IEffectFactory
    {
        BasicEffect CreateBasicEffect();
        Effect CreateFromFile(string file);
    }
}
