using System;
using Microsoft.Xna.Framework.Input;

namespace Dope.DDXX.Input
{
    public interface IInputDriver
    {
        void Step();
        bool KeyPressed(Keys key);
        bool KeyPressedNoRepeat(Keys key);
        bool KeyPressedSlowRepeat(Keys key);

        bool OkPressed();
        bool BackPressed();
        bool PausePressed();
        bool RightPressed();
        bool LeftPressed();
        bool UpPressed();
        bool DownPressed();

        bool OkPressedNoRepeat();
        bool BackPressedNoRepeat();
        bool PausePressedNoRepeat();
        bool RightPressedNoRepeat();
        bool LeftPressedNoRepeat();
        bool UpPressedNoRepeat();
        bool DownPressedNoRepeat();
    }
}
