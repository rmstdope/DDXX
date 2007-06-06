using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoPostEffect : IRegisterable, ITweakableContainer
    {
        void Initialize(IPostProcessor postProcessor, ITextureFactory textureFactory, ITextureBuilder textureBuilder, IDevice device);
        void Render();
    }
}
