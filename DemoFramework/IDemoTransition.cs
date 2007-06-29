using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoTransition : IRegisterable, ITweakableContainer
    {
        int DestinationTrack { get; set; }
        void Initialize(IDevice device, IPostProcessor postProcessor);
        ITexture Render(ITexture fromTexture, ITexture toTexture);
    }
}
