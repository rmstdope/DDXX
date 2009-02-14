using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.GameFramework
{
    public interface IFsa
    {
        void Initialize(IGraphicsFactory graphicsFactory);
        void Step();
        void Render();
    }
}
