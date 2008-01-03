using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Dope.DDXX.Input
{
    public interface IInputFactory
    {
        void Step();
        bool KeyPressed(Keys key);
        bool GamePadButtonA();
        bool GamePadButtonB();
        bool GamePadButtonX();
        bool GamePadButtonY();
        bool GamePadUp();
        bool GamePadDown();
        bool GamePadRight();
        bool GamePadLeft();
    }
}
