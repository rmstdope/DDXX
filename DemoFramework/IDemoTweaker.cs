using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Input;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoTweaker
    {
        bool Quit { get; }
        bool Exiting { get; }
        void Initialize(IDemoRegistrator registrator, IGraphicsFactory graphicsFactory, ITextureFactory textureFactory);
        void Draw();
        bool HandleInput(IInputDriver inputDriver);
        object IdentifierToChild();
        void IdentifierFromParent(object id);
        bool ShouldSave();
    }
}
