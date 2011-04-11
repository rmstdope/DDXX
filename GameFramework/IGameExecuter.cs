using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.GameFramework
{
    public interface IGameExecuter
    {
        void Initialize(IGameCallback game, IFsa startFsa, IGraphicsFactory graphicsFactory, IInputDriver inputDriver, TextureFactory textureFactory, EffectFactory effectFactory, PostProcessor postProcessor);
        void Step();
        void Render();
    }
}
