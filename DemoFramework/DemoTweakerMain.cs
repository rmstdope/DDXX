using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Input;
using Microsoft.DirectX.DirectInput;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweakerMain : DemoTweakerBase, IDemoTweaker
    {
        private int currentTweaker;
        private IDemoTweaker[] tweakers;
        private IDemoTweakerContext context;
        private bool visable;
        private bool saveNeeded;

        public object IdentifierToChild() { return 0; }
        public void IdentifierFromParent(object id) { }

        public bool Enabled
        {
            get { return currentTweaker > -1; }
        }

        public bool Quit 
        {
            get { return currentTweaker < -1; } 
        }

        public DemoTweakerMain(IDemoTweakerContext context, IDemoTweaker[] tweakers, ITweakerSettings settings)
            : base(settings)
        {
            currentTweaker = -1;
            saveNeeded = false;
            this.tweakers = tweakers;
            this.context = context;
            visable = true;
        }

        public override void Initialize(IDemoRegistrator registrator)
        {
            base.Initialize(registrator);

            foreach (IDemoTweaker tweaker in tweakers)
                tweaker.Initialize(registrator);
        }

        public bool HandleInput(IInputDriver inputDriver)
        {
            if (Enabled)
            {
                if (tweakers[currentTweaker].HandleInput(inputDriver))
                    return true;
            }

            if (inputDriver.KeyPressedNoRepeat(Key.RightArrow))
            {
                context.JumpInTime(5.0f);
            }

            if (inputDriver.KeyPressedNoRepeat(Key.LeftArrow))
            {
                context.JumpInTime(-5.0f);
            }

            if (inputDriver.KeyPressedNoRepeat(Key.Space))
            {
                context.TogglePause();
            }

            if (inputDriver.KeyPressedNoRepeat(Key.Return))
            {
                if (currentTweaker < tweakers.Length - 1)
                {
                    saveNeeded = true;
                    if (currentTweaker != -1)
                        tweakers[currentTweaker + 1].IdentifierFromParent(tweakers[currentTweaker].IdentifierToChild());
                    currentTweaker++;
                }
                return true;
            }

            if (inputDriver.KeyPressedNoRepeat(Key.Escape))
            {
                currentTweaker--;
                return true;
            }

            if (inputDriver.KeyPressedNoRepeat(Key.F1))
                visable = !visable;

            if (inputDriver.KeyPressedNoRepeat(Key.F2))
                Settings.SetTransparency(Transparency.Low);
            if (inputDriver.KeyPressedNoRepeat(Key.F3))
                Settings.SetTransparency(Transparency.Medium);
            if (inputDriver.KeyPressedNoRepeat(Key.F4))
                Settings.SetTransparency(Transparency.High);

            if (inputDriver.KeyPressedNoRepeat(Key.F5))
                Settings.NextColorSchema();
            if (inputDriver.KeyPressedNoRepeat(Key.F6))
                Settings.PreviousColorSchema();

            return true;
        }

        public void Draw()
        {
            if (!Enabled)
                return;

            if (visable)
                tweakers[currentTweaker].Draw();
        }

        public bool ShouldSave(IInputDriver inputDriver)
        {
            if (!saveNeeded)
                return false;

            bool save = true;
            while (!inputDriver.KeyPressedNoRepeat(Key.Return))
            {
                if (inputDriver.KeyPressedNoRepeat(Key.RightArrow))
                    save = false;
                if (inputDriver.KeyPressedNoRepeat(Key.LeftArrow))
                    save = true;

                CreateBaseControls();
                BoxControl tweakableWindow = new BoxControl(new RectangleF(0.0f, 0.05f, 1.0f, 0.95f),
                    Settings.Alpha, Settings.TimeColor, MainWindow);
                new TextControl("Should old XML file be overwritten?", new RectangleF(0.0f, 0.0f, 1.0f, 0.9f), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, 1.0f, Color.White, tweakableWindow);
                Color yesColor = Color.DarkGray;
                Color noColor = Color.DarkGray;
                if (save)
                    yesColor = Color.White;
                else
                    noColor = Color.White;
                new TextControl("Yes>>>", new RectangleF(0.0f, 0.0f, 0.5f, 1.0f), DrawTextFormat.Right | DrawTextFormat.VerticalCenter, 1.0f, yesColor, tweakableWindow);
                new TextControl("<<<No", new RectangleF(0.5f, 0.0f, 0.5f, 1.0f), DrawTextFormat.Left | DrawTextFormat.VerticalCenter, 1.0f, noColor, tweakableWindow);

                D3DDriver.GetInstance().Device.BeginScene();
                UserInterface.DrawControl(MainWindow);
                D3DDriver.GetInstance().Device.EndScene();
                D3DDriver.GetInstance().Device.Present();
            }
            return save;
        }

    }
}
