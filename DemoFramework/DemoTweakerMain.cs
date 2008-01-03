using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweakerMain : DemoTweakerBase, IDemoTweaker
    {
        private int currentTweaker;
        private IDemoTweaker[] tweakers;
        private IDemoTweakerContext context;
        private bool visable;
        private bool saveNeeded;
        private bool saveDone;
        private bool shouldSave;

        public object IdentifierToChild() { return 0; }
        public void IdentifierFromParent(object id) { }

        public bool Enabled
        {
            get { return currentTweaker > -1; }
        }

        public bool Quit 
        {
            get { return Exiting && (!saveNeeded || saveDone); } 
        }

        public bool Exiting
        {
            get { return currentTweaker < -1; }
        }

        public DemoTweakerMain(IDemoTweakerContext context, IDemoTweaker[] tweakers, ITweakerSettings settings)
            : base(settings)
        {
            currentTweaker = -1;
            saveNeeded = false;
            saveDone = false;
            shouldSave = true;
            this.tweakers = tweakers;
            this.context = context;
            visable = true;
        }

        public override void Initialize(IDemoRegistrator registrator, IGraphicsFactory graphicsFactory, ITextureFactory textureFactory)
        {
            base.Initialize(registrator, graphicsFactory, textureFactory);

            foreach (IDemoTweaker tweaker in tweakers)
                tweaker.Initialize(registrator, graphicsFactory, textureFactory);
        }

        public bool HandleInput(IInputDriver inputDriver)
        {
            if (Exiting)
            {
                HandleExitInput(inputDriver);
                return true;
            }
            if (Enabled)
            {
                if (tweakers[currentTweaker].HandleInput(inputDriver))
                    return true;
            }

            if (inputDriver.RightPressedNoRepeat())
            {
                context.JumpInTime(5.0f);
            }

            if (inputDriver.LeftPressedNoRepeat())
            {
                context.JumpInTime(-5.0f);
            }

            if (inputDriver.PausePressedNoRepeat())
            {
                context.TogglePause();
            }

            if (inputDriver.OkPressedNoRepeat())
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

            if (inputDriver.BackPressedNoRepeat())
            {
                currentTweaker--;
                return true;
            }

            if (inputDriver.KeyPressedNoRepeat(Keys.F1))
                visable = !visable;

            if (inputDriver.KeyPressedNoRepeat(Keys.F2))
                Settings.SetTransparency(Transparency.Low);
            if (inputDriver.KeyPressedNoRepeat(Keys.F3))
                Settings.SetTransparency(Transparency.Medium);
            if (inputDriver.KeyPressedNoRepeat(Keys.F4))
                Settings.SetTransparency(Transparency.High);

            if (inputDriver.KeyPressedNoRepeat(Keys.F5))
                Settings.NextColorSchema();
            if (inputDriver.KeyPressedNoRepeat(Keys.F6))
                Settings.PreviousColorSchema();

            return true;
        }

        private void HandleExitInput(IInputDriver inputDriver)
        {
            if (inputDriver.RightPressedNoRepeat())
                shouldSave = false;
            if (inputDriver.LeftPressedNoRepeat())
                shouldSave = true;
            if (inputDriver.OkPressedNoRepeat())
                saveDone = true;
        }

        public void Draw()
        {
            if (Exiting)
            {
                HandleExitDraw();
                return;
            }
            if (!Enabled)
                return;

            if (visable)
            {
                tweakers[currentTweaker].Draw();
            }
        }

        private void HandleExitDraw()
        {
            CreateBaseControls();
            BoxControl tweakableWindow = new BoxControl(new Vector4(0, 0.05f, 1, 0.95f),
                Settings.Alpha, Settings.TimeColor, MainWindow);
            new TextControl("Should old XML file be overwritten?", new Vector4(0, 0, 1, 0.90f), TextFormatting.VerticalCenter | TextFormatting.Center, 255, Color.White, tweakableWindow);
            Color yesColor = Color.DarkGray;
            Color noColor = Color.DarkGray;
            if (shouldSave)
                yesColor = Color.White;
            else
                noColor = Color.White;
            new TextControl("Yes>>>", new Vector4(0, 0, 0.5f, 1), TextFormatting.Right | TextFormatting.VerticalCenter, 255, yesColor, tweakableWindow);
            new TextControl("<<<No", new Vector4(0.5f, 0, 0.5f, 1), TextFormatting.Left | TextFormatting.VerticalCenter, 255, noColor, tweakableWindow);

            UserInterface.DrawControl(MainWindow);
        }

        public bool ShouldSave()
        {
            return shouldSave && saveNeeded;
        }

    }
}
