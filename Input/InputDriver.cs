using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Input
{
    public class InputDriver
    {
        private static InputDriver instance;
        private static IInputFactory factory = new DirectInputFactory();

        private Device keyboard;

        private List<Key> noRepeatList;

        private InputDriver()
        {
            noRepeatList = new List<Key>();
        }

        public static IInputFactory Factory
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

        public bool KeyPressedNoRepeat(Key key)
        {
            bool pressed = factory.KeyPressed(keyboard, key);
            if (noRepeatList.Contains(key))
            {
                if (!pressed)
                    noRepeatList.Remove(key);
            }
            else
            {
                if (pressed)
                {
                    noRepeatList.Add(key);
                    return true;
                }
            }
            return false;
        }
    }
}
