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
    public class DemoTweakerMain : IDemoTweaker
    {
        private int currentTweaker;
        private IDemoTweaker[] tweakers;
        private IDemoTweakerContext context;
        private ITweakerSettings settings;
        private bool visable;

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
        {
            currentTweaker = -1;
            this.tweakers = tweakers;
            this.context = context;
            this.settings = settings;
            visable = true;
        }

        public void Initialize(IDemoRegistrator registrator)
        {
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
                settings.SetTransparency(Transparency.Low);
            if (inputDriver.KeyPressedNoRepeat(Key.F3))
                settings.SetTransparency(Transparency.Medium);
            if (inputDriver.KeyPressedNoRepeat(Key.F4))
                settings.SetTransparency(Transparency.High);

            if (inputDriver.KeyPressedNoRepeat(Key.F5))
                settings.NextColorSchema();
            if (inputDriver.KeyPressedNoRepeat(Key.F6))
                settings.PreviousColorSchema();

            return true;
        }

        public void Draw()
        {
            if (!Enabled)
                return;

            if (visable)
                tweakers[currentTweaker].Draw();
        }

    }
}
