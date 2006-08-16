using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;

namespace Dope.DDXX.Input
{
    public interface IInputFactory
    {
        Device Keyboard { get; }
        void SetCooperativeLevel(Device device, Control control, CooperativeLevelFlags cooperativeLevelFlags);
        void Acquire(Device device);
        bool KeyPressed(Device keyboard, Key key);
    }
}
