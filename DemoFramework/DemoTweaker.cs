using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Dope.DDXX.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweaker : IDemoTweaker
    {
        private ITweakable tweakable;
        private TweakerStatus status;
        private ITweakerSettings settings;
        private IDemoRegistrator registrator;
        private IUserInterface userInterface;
        private BaseControl mainWindow;
        private BaseControl timelineWindow;

        public DemoTweaker(ITweakerSettings settings, ITweakable tweakable)
        {
            this.tweakable = tweakable;
            this.settings = settings;
            status = new TweakerStatus(10.0f, 1.0f / tweakable.NumVisableVariables);
        }

        public void Initialize(IDemoRegistrator registrator, IUserInterface userInterface)
        {
            this.registrator = registrator;

            this.userInterface = userInterface;

            CreateBaseControls();
            CreateTimeControls();
            status.RootControl = timelineWindow;
            status.StartTime = registrator.StartTime;
        }

        public void Draw()
        {
            CreateTimeLine(timelineWindow);
            CreateTweakableControls();
            CreateInputControls();

            userInterface.DrawControl(mainWindow);

            status.RootControl.RemoveChildren();
        }

        private void CreateInputControls()
        {
            string displayText = "<No Input>";
            BoxControl inputBox = new BoxControl(new Vector4(0, 0.97f, 1, 0.03f),
                settings.Alpha, settings.TitleColor, mainWindow);
            if (status.InputString != "")
                displayText = "Input: " + status.InputString;
            TextControl text = new TextControl(displayText, new Vector2(0.5f, 0.5f),
                TextFormatting.Center | TextFormatting.VerticalCenter, 255,
                Color.White, inputBox);
        }

        private void CreateBaseControls()
        {
            mainWindow = new BoxControl(new Vector4(0.02f, 0.02f, 0.96f, 0.96f), 0, Color.Black, null);

            BaseControl titleWindow = new BoxControl(new Vector4(0, 0, 1, 0.04f),
                settings.Alpha, settings.TitleColor, mainWindow);
            int seconds = (int)Time.CurrentTime;
            int hundreds = (int)((Time.CurrentTime - seconds) * 100);
            string titleString = "DDXX Tweaker - " + seconds.ToString("D3") + "." + hundreds.ToString("D2");
            BaseControl titleText = new TextControl(titleString, new Vector4(0, 0, 1, 1), TextFormatting.Center | TextFormatting.VerticalCenter,
                settings.TextAlpha, Color.White, titleWindow);
        }

        private void CreateTimeControls()
        {
            BaseControl timeWindow = new BoxControl(new Vector4(0, 0.04f, 1, 0.93f),
                settings.Alpha, settings.TimeColor, mainWindow);
            timelineWindow = new BoxControl(new Vector4(0.02f, 0.04f, 0.96f, 0.92f), 0, Color.Black, timeWindow);
        }

        private void CreateTimeLine(BaseControl timelineWindow)
        {
            new LineControl(new Vector4(0, 0.01f, 1, 0), settings.Alpha, Color.White, timelineWindow);
            for (int i = 0; i < 5; i++)
            {
                float x = i / 4.0f;
                float t = status.StartTime + status.TimeScale * i / 4.0f;
                new TextControl(t.ToString(), new Vector2(x, 0), TextFormatting.Bottom | TextFormatting.Center, settings.Alpha, Color.White, timelineWindow);
                new LineControl(new Vector4(x, 0, 0, 0.02f), settings.Alpha, Color.White, timelineWindow);
            }
            while ((Time.CurrentTime - status.StartTime) / status.TimeScale > 0.9f)
                status.StartTime += status.TimeScale * 0.9f;
            new LineControl(new Vector4((Time.CurrentTime - status.StartTime) / status.TimeScale, 0, 0, 1), settings.Alpha, Color.White, timelineWindow);
        }

        private void CreateTweakableControls()
        {
            float y = 0.05f;
            for (int i = DrawStart; i < DrawStart + tweakable.NumVisableVariables; i++)
            {
                if (i >= tweakable.NumVariables)
                    continue;

                tweakable.CreateChildControl(status, i, y, settings);
                y += status.VariableSpacing;
            }
            tweakable.CreateBaseControls(status, settings);
        }

        private int DrawStart
        {
            get
            {
                int start = status.Selection - (tweakable.NumVisableVariables / 2);
                while (start > 0 && start + tweakable.NumVisableVariables > tweakable.NumVariables)
                    start--;
                if (start < 0)
                    start = 0;
                return start;
            }
        }

        private Color GetBoxColor(int index)
        {
            if (index == status.Selection)
                return settings.SelectedColor;
            return settings.UnselectedColor;
        }

        private Color GetTextColor(int index)
        {
            if (index == status.Selection)
                return Color.White;
            return Color.Gray;
        }

        private float VariableHeight
        {
            get { return status.VariableSpacing * 0.9f; }
        }

        private void KeyUp()
        {
            if (status.Selection > 0)
                status.Selection--;
        }

        private void KeyDown()
        {
            if (status.Selection < tweakable.NumVariables - 1)
                status.Selection++;
        }

        public IDemoTweaker HandleInput(IInputDriver inputDriver)
        {
            IDemoTweaker tweaker = null;

            if (inputDriver.KeyPressedNoRepeat(Keys.Up))
            {
                KeyUp();
            }
            if (inputDriver.KeyPressedNoRepeat(Keys.Down))
            {
                KeyDown();
            }
            if (inputDriver.KeyPressedNoRepeat(Keys.Tab))
            {
                tweakable.NextIndex(status);
            }
            if (inputDriver.KeyPressedSlowRepeat(Keys.PageUp))
            {
                tweakable.IncreaseValue(status);
            }
            if (inputDriver.KeyPressedSlowRepeat(Keys.PageDown))
            {
                tweakable.DecreaseValue(status);
            }

            StringInput(inputDriver);

            if (inputDriver.KeyPressedNoRepeat(Keys.Enter))
            {
                if (tweakable.GetChild(status.Selection) != null)
                {
                    tweaker = new DemoTweaker(settings, tweakable.GetChild(status.Selection));
                    tweaker.Initialize(registrator, userInterface);
                }
            }

            return tweaker;
        }

        private void StringInput(IInputDriver inputDriver)
        {
            Keys[] digitKeys = new Keys[] { 
                Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, 
                Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 
            };
            Keys[] numPadDigitKeys = new Keys[] { 
                Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, 
                Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9
            };

            for (int i = 0; i < digitKeys.Length; i++)
            {
                if (inputDriver.KeyPressedNoRepeat(digitKeys[i]) ||
                    inputDriver.KeyPressedNoRepeat(numPadDigitKeys[i]))
                {
                    status.InputString += i;
                }
            }
            if (inputDriver.KeyPressedNoRepeat(Keys.Decimal) ||
                inputDriver.KeyPressedNoRepeat(Keys.OemPeriod))
            {
                status.InputString += ".";
            }
            if (inputDriver.KeyPressedNoRepeat(Keys.Subtract) ||
                inputDriver.KeyPressedNoRepeat(Keys.OemMinus))
            {
                status.InputString += "-";
            }
            if (status.InputString != "" && status.InputString != null && inputDriver.KeyPressedNoRepeat(Keys.Enter))
            {
                try
                {
                    tweakable.SetValue(status);
                }
                catch (FormatException) { }
                status.InputString = "";
            }
        }

        //public void Update(IEffectChangeListener effectChangeListener)
        //{
        //    foreach (ITrack track in tracks)
        //    {
        //        track.UpdateListener(effectChangeListener);
        //    }
        //}


        public void ReadFromXmlFile(XmlNode node)
        {
            if (node.Name != "Demo")
                throw new DDXXException("Root node of XML document should be demo");
            tweakable.ReadFromXmlFile(node);
        }

        public void WriteToXmlFile(XmlDocument xmlDocument, XmlNode node)
        {
            tweakable.WriteToXmlFile(xmlDocument, node);
        }
    }
}
