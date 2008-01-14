using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoEffect : IRegisterable
    {
        int DrawOrder { get; set; }
        void Step();
        void Render();
        void Initialize(IGraphicsFactory graphicsFactory, IEffectFactory effectFactory, ITextureFactory textureFactory,
            IDemoMixer mixer, IPostProcessor postProcessor);
    }
}
