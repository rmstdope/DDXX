using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoPostEffect : IRegisterable, ITweakableContainer, ITweakable
    {
        int DrawOrder { get; set; }
        void Initialize(IGraphicsFactory graphicsFactory, IPostProcessor postProcessor, ITextureFactory textureFactory, ITextureBuilder textureBuilder/*, IGraphicsDevice device*/);
        void Render();
    }
}
