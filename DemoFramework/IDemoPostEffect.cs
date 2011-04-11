using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoPostEffect : IRegisterable
    {
        int DrawOrder { get; set; }
        void Initialize(IGraphicsFactory graphicsFactory, PostProcessor postProcessor);
        void Render();
    }
}
