using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Input;
using Dope.DDXX.UserInterface;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoTweakerHandler
    {
        ITweakableFactory Factory { get; }
        bool Quit { get; }
        bool Exiting { get; }
        void Initialize(IDemoRegistrator registrator, IUserInterface userInterface, ITweakable firstTweaker);
        void Draw();
        IDemoTweaker HandleInput(IInputDriver inputDriver);
        object IdentifierToChild();
        void IdentifierFromParent(object id);
        bool ShouldSave();
        void ReadFromXmlFile(string xmlFileName);
        void WriteToXmlFile();
    }
}
