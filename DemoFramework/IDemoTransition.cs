using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoTransition : IRegisterable, ITweakableContainer, ITweakable
    {
        int DestinationTrack { get; set; }
        void Initialize(/*IGraphicsDevice device,*/ IPostProcessor postProcessor);
        IRenderTarget2D Render(IRenderTarget2D fromTexture, IRenderTarget2D toTexture);
    }
}
