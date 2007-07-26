using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoEffect : IRegisterable, ITweakableContainer
    {
        int DrawOrder { get; set; }
        void Step();
        void Render();
        void Initialize(IGraphicsFactory graphicsFactory, IEffectFactory effectFactory, 
            IDevice device, IDemoMixer mixer, IPostProcessor postProcessor);
    }
}
