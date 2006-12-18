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

        public DemoTweakerMain(IDemoTweakerContext context, IDemoTweaker[] tweakers)
        {
            currentTweaker = -1;
            this.tweakers = tweakers;
            this.context = context;
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
