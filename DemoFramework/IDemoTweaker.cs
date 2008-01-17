using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Dope.DDXX.Input;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoTweaker
    {
        void Initialize(IDemoRegistrator registrator, IUserInterface userInterface);
        void Draw();
        IDemoTweaker HandleInput(IInputDriver inputDriver);
        void ReadFromXmlFile(XmlNode node);
    }
}
