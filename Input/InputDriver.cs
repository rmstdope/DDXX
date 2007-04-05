using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Input
{
    public class InputDriver : IInputDriver
    {
        private class KeyInfo
        {
            private float[] slowRepeatTimes = new float[] { 0.5f, 0.1f, 0.02f };
            private int[] slowRepeatNums = new int[] { 3, 15, 200 };

            public KeyInfo()
            {
                time = Time.CurrentTime;
                numRepeats = 1;
            }
            public bool IsSlowRepeat()
            {
                float t = Time.CurrentTime;
                for (int i = 0; i < slowRepeatTimes.Length; i++)
                {
                    if (numRepeats <= slowRepeatNums[i])
                    {
                        if (t >= time + slowRepeatTimes[i])
                        {
                            numRepeats++;
                            time = t;
                            return true;
                        }
                        return false;
                    }
                }
                return true;
            }
            private float time;
            private int numRepeats;
        }
        private static InputDriver instance;
        private static IInputFactory factory = new DirectInputFactory();

        private Device keyboard;

        private Dictionary<Key, KeyInfo> pressEntries;

        private InputDriver()
        {
            Reset();
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

        public void Reset()
        {
            pressEntries = new Dictionary<Key, KeyInfo>();
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
            if (pressEntries.ContainsKey(key))
            {
                if (!pressed)
                    pressEntries.Remove(key);
            }
            else
            {
                if (pressed)
                {
                    pressEntries.Add(key, new KeyInfo());
                    return true;
                }
            }
            return false;
        }

        public bool KeyPressedSlowRepeat(Key key)
        {
            bool pressed = factory.KeyPressed(keyboard, key);
            if (pressEntries.ContainsKey(key))
            {
                if (!pressed)
                    pressEntries.Remove(key);
                else
                {
                    KeyInfo info = pressEntries[key];
                    return info.IsSlowRepeat();
                }
            }
            else
            {
                if (pressed)
                {
                    pressEntries.Add(key, new KeyInfo());
                    return true;
                }
            }
            return false;
        }
    }
}
