using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Microsoft.DirectX.DirectInput;

namespace Input
{
    class DIFactory : IFactory
    {
        #region IFactory Members

        public Device Keyboard
        {
            get { return new Device(SystemGuid.Keyboard); }
        }

        public void SetCooperativeLevel(Device device, Control control, CooperativeLevelFlags cooperativeLevelFlags)
        {
            device.SetCooperativeLevel(control, cooperativeLevelFlags);
        }

        public void Acquire(Device device)
        {
            device.Acquire();
        }

        public bool KeyPressed(Device keyboard, Key key)
        {
            return keyboard.GetCurrentKeyboardState()[key];
        }

        #endregion
    }
}
