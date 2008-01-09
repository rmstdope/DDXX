using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Input;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoTweakerHandler
    {
        bool Quit { get; }
        bool Exiting { get; }
        void Initialize(IDemoRegistrator registrator, IUserInterface userInterface);
        void Draw();
        IDemoTweaker HandleInput(IInputDriver inputDriver);
        object IdentifierToChild();
        void IdentifierFromParent(object id);
        bool ShouldSave();
    }
}
