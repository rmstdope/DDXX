using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoEffect : IRegisterable, ITweakableContainer
    {
        void Step();

        void Render();

        void Initialize(IGraphicsFactory graphicsFactory, IDevice device);
    }
}
