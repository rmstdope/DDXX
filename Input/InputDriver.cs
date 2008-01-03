using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Input;

namespace Dope.DDXX.Input
{
    public class InputDriver : IInputDriver
    {
        private enum Buttons
        {
            ButtonA,
            ButtonB,
            ButtonX,
            ButtonY,
            DPadRight,
            DPadLeft,
            DPadUp,
            DPadDown
        }

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
        private Dictionary<Keys, KeyInfo> keyEntries;
        private Dictionary<Buttons, KeyInfo> buttonEntries;
        private static IInputFactory factory = new InputFactory();

        private InputDriver()
        {
            Reset();
        }

        public static IInputFactory Factory
        {
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
            keyEntries = new Dictionary<Keys, KeyInfo>();
            buttonEntries  = new Dictionary<Buttons, KeyInfo>();
        }

        public void Step()
        {
            factory.Step();
        }

        public bool KeyPressed(Keys key)
        {
            return factory.KeyPressed(key);
        }

        public bool KeyPressedNoRepeat(Keys key)
        {
            bool pressed = KeyPressed(key);
            if (keyEntries.ContainsKey(key))
            {
                if (!pressed)
                    keyEntries.Remove(key);
            }
            else
            {
                if (pressed)
                {
                    keyEntries.Add(key, new KeyInfo());
                    return true;
                }
            }
            return false;
        }

        public bool KeyPressedSlowRepeat(Keys key)
        {
            bool pressed = KeyPressed(key);
            if (keyEntries.ContainsKey(key))
            {
                if (!pressed)
                    keyEntries.Remove(key);
                else
                {
                    KeyInfo info = keyEntries[key];
                    return info.IsSlowRepeat();
                }
            }
            else
            {
                if (pressed)
                {
                    keyEntries.Add(key, new KeyInfo());
                    return true;
                }
            }
            return false;
        }

        private bool ButtonPressed(Buttons button)
        {
            switch (button)
            {
                case Buttons.ButtonA:
                    return factory.GamePadButtonA();
                case Buttons.ButtonB:
                    return factory.GamePadButtonB();
                case Buttons.ButtonX:
                    return factory.GamePadButtonX();
                case Buttons.ButtonY:
                    return factory.GamePadButtonY();
                case Buttons.DPadDown:
                    return factory.GamePadLeft();
                case Buttons.DPadLeft:
                    return factory.GamePadLeft();
                case Buttons.DPadRight:
                    return factory.GamePadRight();
                case Buttons.DPadUp:
                    return factory.GamePadUp();
            }
            return false;
        }

        private bool ButtonPressedNoRepeat(Buttons button)
        {
            bool pressed = ButtonPressed(button);
            if (buttonEntries.ContainsKey(button))
            {
                if (!pressed)
                    buttonEntries.Remove(button);
            }
            else
            {
                if (pressed)
                {
                    buttonEntries.Add(button, new KeyInfo());
                    return true;
                }
            }
            return false;
        }

        private bool ButtonPressedSlowRepeat(Buttons button)
        {
            bool pressed = ButtonPressed(button);
            if (buttonEntries.ContainsKey(button))
            {
                if (!pressed)
                    buttonEntries.Remove(button);
                else
                {
                    KeyInfo info = buttonEntries[button];
                    return info.IsSlowRepeat();
                }
            }
            else
            {
                if (pressed)
                {
                    buttonEntries.Add(button, new KeyInfo());
                    return true;
                }
            }
            return false;
        }

        public bool OkPressed()
        {
            return ButtonPressed(Buttons.ButtonA) || KeyPressed(Keys.Enter);
        }

        public bool BackPressed()
        {
            return ButtonPressed(Buttons.ButtonB) || KeyPressed(Keys.Escape);
        }

        public bool PausePressed()
        {
            return ButtonPressed(Buttons.ButtonX) || KeyPressed(Keys.Space);
        }

        public bool RightPressed()
        {
            return ButtonPressed(Buttons.DPadRight) || KeyPressed(Keys.Right);
        }

        public bool LeftPressed()
        {
            return ButtonPressed(Buttons.DPadLeft) || KeyPressed(Keys.Left);
        }

        public bool UpPressed()
        {
            return ButtonPressed(Buttons.DPadUp) || KeyPressed(Keys.Up);
        }

        public bool DownPressed()
        {
            return ButtonPressed(Buttons.DPadDown) || KeyPressed(Keys.Down);
        }

        public bool OkPressedNoRepeat()
        {
            return ButtonPressedNoRepeat(Buttons.ButtonA) || KeyPressedNoRepeat(Keys.Enter);
        }

        public bool BackPressedNoRepeat()
        {
            return ButtonPressedNoRepeat(Buttons.ButtonB) || KeyPressedNoRepeat(Keys.Escape);
        }

        public bool PausePressedNoRepeat()
        {
            return ButtonPressedNoRepeat(Buttons.ButtonX) || KeyPressedNoRepeat(Keys.Space);
        }

        public bool RightPressedNoRepeat()
        {
            return ButtonPressedNoRepeat(Buttons.DPadRight) || KeyPressedNoRepeat(Keys.Right);
        }

        public bool LeftPressedNoRepeat()
        {
            return ButtonPressedNoRepeat(Buttons.DPadLeft) || KeyPressedNoRepeat(Keys.Left);
        }

        public bool UpPressedNoRepeat()
        {
            return ButtonPressedNoRepeat(Buttons.DPadUp) || KeyPressedNoRepeat(Keys.Up);
        }

        public bool DownPressedNoRepeat()
        {
            return ButtonPressedNoRepeat(Buttons.DPadDown) || KeyPressedNoRepeat(Keys.Down);
        }

    }
}
