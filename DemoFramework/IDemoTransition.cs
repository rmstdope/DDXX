using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoTransition : IRegisterable
    {
        int DestinationTrack { get; set; }
        void Initialize(IPostProcessor postProcessor, IGraphicsFactory graphicsFactory);
        IRenderTarget2D Render(IRenderTarget2D fromTexture, IRenderTarget2D toTexture);
    }
}
