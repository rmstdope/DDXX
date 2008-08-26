using System;

namespace Dope.DDXX.Graphics
{
    public interface IEffectFactory
    {
        IBasicEffect CreateBasicEffect();
        IEffect CreateFromFile(string file);
    }
}
