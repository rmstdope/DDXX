using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoPostEffect : IRegisterable, ITweakableContainer
    {
        void Initialize(IPostProcessor postProcessor);
        void Render();
    }
}
