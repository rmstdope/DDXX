using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Input;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoTweaker
    {
        bool Quit { get; }
        void Initialize(IDemoRegistrator registrator);
        void Draw();
        void HandleInput(IInputDriver inputDriver);
        object IdentifierToChild();
        void IdentifierFromParent(object id);
    }
}
