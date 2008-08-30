using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.UserInterface
{
    public enum FontSize
    {
        Small,
        Medium,
        Large
    }
    public interface IUserInterface
    {
        void Initialize(IGraphicsFactory graphicsFactory, ITextureFactory textureFactory);
        void DrawControl(BaseControl control);
        void SetFont(FontSize size, ISpriteFont font);
        IDrawResources DrawResources { get; }
    }
}
