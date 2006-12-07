using System;

namespace Dope.DDXX.Graphics
{
    public interface IEffectFactory
    {
        IEffect CreateFromFile(string file);
    }
}
