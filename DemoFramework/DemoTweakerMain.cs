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

        public bool Enabled
        {
            get { return currentTweaker > -1; }
        }

        public bool Quit 
        {
            get { return currentTweaker < -1; } 
        }

        public DemoTweakerMain(IDemoTweaker[] tweakers)
        {
            currentTweaker = -1;
            this.tweakers = tweakers;
        }

        public void Initialize(IDemoRegistrator registrator)
        {
            foreach (IDemoTweaker tweaker in tweakers)
                tweaker.Initialize(registrator);
        }

        public void HandleInput(IInputDriver inputDriver)
        {
            if (inputDriver.KeyPressedNoRepeat(Key.Return))
            {
                if (currentTweaker < tweakers.Length - 1)
                    currentTweaker++;
                return;
            }

            if (inputDriver.KeyPressedNoRepeat(Key.Escape))
            {
                currentTweaker--;
                return;
            }

            if (!Enabled)
                return;

            tweakers[currentTweaker].HandleInput(inputDriver);
        }

        public void Draw()
        {
            if (!Enabled)
                return;

            tweakers[currentTweaker].Draw();
        }

    }
}
