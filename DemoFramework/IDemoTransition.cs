using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoTransition : IRegisterable
    {
        int DestinationTrack { get; set; }
        void Initialize(PostProcessor postProcessor, IGraphicsFactory graphicsFactory);
        RenderTarget2D Render(RenderTarget2D fromTexture, RenderTarget2D toTexture);
    }
}
