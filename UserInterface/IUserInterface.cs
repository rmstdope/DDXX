using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.UserInterface
{
    public interface IUserInterface
    {
        void Initialize(IGraphicsFactory graphicsFactory, ITextureFactory textureFactory);
        void DrawControl(BaseControl control);
    }
}
