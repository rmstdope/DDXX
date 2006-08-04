using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;
using Utility;

namespace Input
{
    public class InputDriver
    {
        private static InputDriver instance;
        private static IFactory factory = new DIFactory();

        private Device keyboard;

        private InputDriver()
        {
        }

        public static IFactory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        public static InputDriver GetInstance()
        {
            if (instance == null)
            {
                instance = new InputDriver();
            }
            return instance;
        }


        public void Initialize(Control control)
        {
            keyboard = factory.Keyboard;

            if (keyboard == null)
                throw new DDXXException("Could not enumerate any attached keyboard");

            factory.SetCooperativeLevel(keyboard, control, CooperativeLevelFlags.NonExclusive | CooperativeLevelFlags.Background);

            factory.Acquire(keyboard);
        }

        public bool KeyPressed(Key key)
        {
            return factory.KeyPressed(keyboard, key);
        }
    }
}
