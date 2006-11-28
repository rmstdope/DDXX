using System;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;

namespace Dope.DDXX.Input
{
    public interface IInputDriver
    {
        void Initialize(Control control);
        bool KeyPressed(Key key);
        bool KeyPressedNoRepeat(Key key);
    }
}
