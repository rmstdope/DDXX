using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoTransition : IRegisterable, ITweakableContainer
    {
        int DestinationTrack { get; set; }
        void Initialize(IPostProcessor postProcessor);
        ITexture Combine(ITexture fromTexture, ITexture toTexture);
    }
}
